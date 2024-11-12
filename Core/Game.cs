using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Core;

public abstract class Game : Microsoft.Xna.Framework.Game
{
    private static Game instance = null;
    internal static Game Instance => instance ?? throw new NullReferenceException("Tried to get the game's instance before the game has been made");

    // using LinkedList instead of a List, because it's faster when creating new objects
    // removing makes no difference, as it only needs to climb till the correct gameObject. Where in a list it'd need to move all the elements after one forward.
    private readonly Dictionary<Type, LinkedList<GameObject>> objectRegistry = null;

    // stores the gameObjects that need to be deleted
    private readonly List<GameObject> disposeObjects = null;

    public Game()
    {
        if (instance != null) throw new InvalidOperationException("there is only allowed to be one game!");
        instance = this;

        objectRegistry = new();
        disposeObjects = new();
    }

    protected override void Update(GameTime gameTime)
    {
        // remove the gameObjects that are scheduled for deletion
        while (disposeObjects.Count > 0)
        {
            Type type = disposeObjects[0].GetType();
            objectRegistry[type].Remove(disposeObjects[0]);
            disposeObjects.RemoveAt(0);
        }

        base.Update(gameTime);
    }

    public void ForEachObject<T>(Action<T> update) where T : class
    {
        // do nothing if the object registry doesn't contain the type
        if (objectRegistry.ContainsKey(typeof(T)) == false)
            return;

        // call the specified update function for each item, passing itself as an argument
        foreach (GameObject obj in objectRegistry[typeof(T)])
            update.Invoke(obj as T);
    }

    // adds the gameObject to the registry
    internal void AddGameObject(GameObject obj)
    {
        // add a new list if this type hasn't been added to the registry yet
        Type type = obj.GetType(); // if obj has been inherited, this'll prefer that type
        if (objectRegistry.ContainsKey(type) == false)
            objectRegistry.Add(type, new());

        objectRegistry[type].AddLast(obj);
    }

    // schedules the gameObject for deletion
    internal void RemoveGameObject(GameObject obj)
    {
        disposeObjects.Add(obj);
    }
}
