// TODO: currently Initialize() and LoadContent() will not be called after the game has gone past these steps (currently not an issue as they go unused, but will eventually be)
using System;
using System.Collections.Generic;
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

    public IEnumerable<T> GetObjectsOfType<T>() where T : class
    {
        return objectRegistry.GetObjectsOfType<T>();
    }

    protected override void Initialize()
    {
        initialized = true;
        foreach (IInitialize initialize in objectRegistry.GetObjectsOfType<IInitialize>())
            initialize.Initialize();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        loadedContent = true;
        foreach (ILoadContent loadContent in objectRegistry.GetObjectsOfType<ILoadContent>())
            loadContent.LoadContent(Content);

        base.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {

        foreach (IUpdate update in objectRegistry.GetObjectsOfType<IUpdate>())
            update.Update();

        // dispose the gameObjects that were scheduled for deletion
        objectRegistry.DisposeGameObjects();

        base.Update(gameTime);
    }

    protected void DrawObjects()
    {
        foreach (IDraw draw in objectRegistry.GetObjectsOfType<IDraw>())
            draw.Draw(SpriteBatch);
    }
}
