using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Core;

public abstract class Game : Microsoft.Xna.Framework.Game
{
    private static Game instance = null;
    private bool initialized = false;
    private bool loadedContent = false;

    internal static Game Instance => instance ?? throw new NullReferenceException("Tried to get the game's instance before the game has been made");

    internal readonly ObjectRegistry objectRegistry;
    internal bool Initialized => initialized;
    internal bool LoadedContent => loadedContent;

    protected abstract SpriteBatch SpriteBatch { get; set; }

    public Game()
    {
        if (instance != null) throw new InvalidOperationException("there is only allowed to be one game!");
        instance = this;

        objectRegistry = new();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IEnumerable<T> GetObjectsOfType<T>() where T : class
    {
        return Instance.objectRegistry.GetObjectsOfType<T>();
    }

    // executes the scheduled actions DO NOT CALL UNLESS YOU KNOW WHAT YOU'RE DOING
    protected void ExecuteScheduled()
    {
        objectRegistry.CreateGameObjects();
        objectRegistry.DisposeGameObjects();
    }

    protected override void Initialize()
    {
        initialized = true;
        foreach (IInitialize initialize in GetObjectsOfType<IInitialize>())
            initialize.Initialize();

        ExecuteScheduled();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        loadedContent = true;
        foreach (ILoadContent loadContent in GetObjectsOfType<ILoadContent>())
            loadContent.LoadContent(Content);

        ExecuteScheduled();

        base.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        foreach (IUpdate update in GetObjectsOfType<IUpdate>())
            update.Update();

        ExecuteScheduled();

        base.Update(gameTime);
    }

    protected void DrawObjects()
    {
        foreach (IDraw draw in GetObjectsOfType<IDraw>())
            draw.Draw(SpriteBatch);
    }
}
