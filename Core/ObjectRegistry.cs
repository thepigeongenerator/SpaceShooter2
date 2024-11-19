using System;
using System.Collections;
using System.Collections.Generic;

namespace Core;

internal class ObjectRegistry
{
    // using linked lists instead of a List, because it's faster when creating new objects
    // removing makes no difference, as it only needs to climb till the correct gameObject. Where in a list it'd need to move all the elements after one forward.
    private readonly LinkedList<GameObject> objectRegistry = null;
    private readonly LinkedList<IInitialize> initializes = null;
    private readonly LinkedList<ILoadContent> loadContents = null;
    private readonly LinkedList<IUpdate> updates = null;
    private readonly LinkedList<IDraw> draws = null;

    // queues for game object deletion / creation
    private readonly Queue<GameObject> createQueue = null;      // I'm honestly sad that this is a default type, I implemented my own
    private readonly Queue<GameObject> disposeQueue = null;

    public ObjectRegistry()
    {
        // registries
        objectRegistry = new();
        initializes = new();
        loadContents = new();
        updates = new();
        draws = new();

        createQueue = new();
        disposeQueue = new();
    }

    // executes exec if obj is castable to T
    private static void ExecIfMatch<T>(object obj, Action<T> exec)
    {
        if (obj is T t)
            exec.Invoke(t);
    }

    // adds the gameObject to the registry
    public void AddGameObject(GameObject obj) => createQueue.Enqueue(obj);

    // schedules the gameObject for deletion
    public void RemoveGameObject(GameObject obj) => disposeQueue.Enqueue(obj);

    // create the queued game objects
    public void CreateGameObjects()
    {
        while (createQueue.Count > 0)
        {
            GameObject obj = createQueue.Dequeue();

            // add the object to the object registry
            objectRegistry.AddLast(obj);

            // add the object to the different "event" lists, for Initialize and LoadContent, call the method if it's already been ran
            ExecIfMatch<IInitialize>(obj, o =>
            {
                initializes.AddLast(o);
                if (Game.Instance.Initialized) o.Initialize();
            });

            ExecIfMatch<ILoadContent>(obj, o =>
            {
                loadContents.AddLast(o);
                if (Game.Instance.LoadedContent) o.LoadContent(Game.Instance.Content);
            });

            ExecIfMatch<IUpdate>(obj, o => updates.AddLast(o));
            ExecIfMatch<IDraw>(obj, o => draws.AddLast(o));
        }
    }

    // dispose of the queued game objects
    public void DisposeGameObjects()
    {
        // remove the gameObjects that are scheduled for deletion
        while (disposeQueue.Count > 0)
        {
            GameObject obj = disposeQueue.Dequeue();

            objectRegistry.Remove(obj);
            ExecIfMatch<IInitialize>(obj, o => initializes.Remove(o));
            ExecIfMatch<ILoadContent>(obj, o => loadContents.Remove(o));
            ExecIfMatch<IUpdate>(obj, o => updates.Remove(o));
            ExecIfMatch<IDraw>(obj, o => draws.Remove(o));
        }
    }

    // get an enumerator which iterates through the objects which are of the type T
    // also, yes I slightly over-engendered it
    public IEnumerable<T> GetObjectsOfType<T>() where T : class
    {
        IEnumerable objects = objectRegistry; // store the object registry as an abstracted type

        {
            // determine what types T is derrived from
            byte i = 0;
            if (typeof(T).IsAssignableFrom(typeof(IInitialize))) i |= 1;
            if (typeof(T).IsAssignableFrom(typeof(ILoadContent))) i |= 2;
            if (typeof(T).IsAssignableFrom(typeof(IUpdate))) i |= 4;
            if (typeof(T).IsAssignableFrom(typeof(IDraw))) i |= 8;

            {
                // determine which of the available lists which we can find this object in has the least items
                int len = -1; // default count = -1 (will always be less than any of these either-way)
                if ((i & 1) != 0) { len = initializes.Count; objects = initializes; }
                if ((i & 2) != 0 && len < loadContents.Count) { len = loadContents.Count; objects = loadContents; }
                if ((i & 4) != 0 && len < updates.Count) { len = updates.Count; objects = updates; }
                if ((i & 8) != 0 && len < draws.Count) objects = draws;
            }
        }

        // loop through the objects, if it's able to be cast to T, yield-return it.
        foreach (object obj in objects)
        {
            if (obj is T t)
                yield return t;
        }
    }
}
