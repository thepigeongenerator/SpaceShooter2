using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Core;

public abstract class Game : Microsoft.Xna.Framework.Game
{
    private readonly Type[] scenes = null;
    private static Game instance = null;
    private Scene scene;
    private bool initialized = false;
    private bool loadedContent = false;

    internal static Game Instance => instance ?? throw new NullReferenceException("Tried to get the game's instance before the game has been made");

    internal bool Initialized => initialized;
    internal bool LoadedContent => loadedContent;
    public Scene Scene => scene;

    protected abstract SpriteBatch SpriteBatch { get; set; }

    public Game()
    {
        if (instance != null) throw new InvalidOperationException("there is only allowed to be one game!");
        instance = this;

        scenes = GetScenes();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IEnumerable<T> GetObjectsOfType<T>() where T : class
    {
        return Instance.scene.objectRegistry.GetObjectsOfType<T>();
    }

    public void LoadScene(int i, params object[] args)
    {
        if (scenes[i].IsSubclassOf(typeof(Scene)) == false)
            throw new InvalidCastException($"the scene '{i}' is not derrived from '{typeof(Scene).FullName}'!");

        scene?.Dispose();
        scene = (Scene)Activator.CreateInstance(scenes[i], args);
        if (initialized) scene.Initialize();
        if (loadedContent) scene.LoadContent(Content);
    }

    // executes the scheduled actions DO NOT CALL UNLESS YOU KNOW WHAT YOU'RE DOING
    protected void ExecuteScheduled()
    {
        scene.objectRegistry.CreateGameObjects();
        scene.objectRegistry.DisposeGameObjects();
    }

    protected override void Initialize()
    {
        initialized = true;
        scene.Initialize();
        foreach (IInitialize initialize in GetObjectsOfType<IInitialize>())
            initialize.Initialize();

        ExecuteScheduled();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        loadedContent = true;
        scene.LoadContent(Content);
        foreach (ILoadContent loadContent in GetObjectsOfType<ILoadContent>())
            loadContent.LoadContent(Content);

        ExecuteScheduled();

        base.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        scene.Update();
        foreach (IUpdate update in GetObjectsOfType<IUpdate>())
            update.Update();

        ExecuteScheduled();

        base.Update(gameTime);
    }

    protected abstract Type[] GetScenes();
}
