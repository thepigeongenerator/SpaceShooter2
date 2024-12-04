using System;
using Core.Data;

namespace Core;

public class GameObject : IDisposable
{
    private bool disposed = false;
    public readonly Transform transform = null;

    public GameObject()
    {
        // initialize public variables with their default values
        transform = new();
        Game.Instance.Scene.objectRegistry.AddGameObject(this);
    }

    // to get rid of the gameobject
    public void Dispose()
    {
        // ignore extra dispose calls; we've already disposed.
        if (disposed)
            return;

        GC.SuppressFinalize(this);
        Game.Instance.Scene.objectRegistry.RemoveGameObject(this);
        disposed = true;
        OnDispose();
    }

    protected virtual void OnDispose() { }
}
