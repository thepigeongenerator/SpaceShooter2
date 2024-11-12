using System;
using System.Collections.Generic;

namespace SpaceShooter2.Src.Data;
/// <summary>handles the application's global state</summary>
internal struct GlobalState
{
    public Random random;
    public List<Astroid> asteroids;
    public List<Bullet> bullets;
    public Player player;
    public Timings timings;
    public Textures textures;
}
