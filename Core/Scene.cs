using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Core;

public abstract class Scene : IDisposable, IInitialize, ILoadContent, IUpdate, IDraw
{
    private bool disposed = false;
    internal readonly ObjectRegistry objectRegistry;

    public Scene()
    {
        objectRegistry = new();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerable<T> GetObjectsOfType<T>() where T : class
    {
        return objectRegistry.GetObjectsOfType<T>();
    }

    public void DrawObjects(SpriteBatch spriteBatch)
    {
        Draw(spriteBatch);
        foreach (IDraw draw in GetObjectsOfType<IDraw>())
            draw.Draw(spriteBatch);
    }

    public void Dispose()
    {
        // ignore extra dispose calls; we've already disposed.
        if (disposed)
            return;

        GC.SuppressFinalize(this);

        // dispose of the object registry
        objectRegistry.Dispose();

        disposed = true;
        OnDispose();
    }

    public virtual void OnDispose() { }

    public abstract void Draw(SpriteBatch spriteBatch);
    public abstract void Update();
    public abstract void LoadContent(ContentManager content);
    public abstract void Initialize();
}
