using System;
using System.Collections.Generic;
using ThePigeonGenerator.MonoGame.Render;

namespace SpaceShooter2.Src.Data;

// handles the application's global state
internal struct GlobalState
{
    public Random random;
    public List<Astroid> asteroids;
    public List<Bullet> bullets;
    public Player player;
    public Timings timings;
    public Textures textures;

    // debug
    public PixelControlLayer pcl;
    public bool hitboxes;
    public bool hitboxesLock;
}
