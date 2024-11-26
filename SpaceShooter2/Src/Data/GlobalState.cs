using System;
using System.Collections.Generic;
using ThePigeonGenerator.MonoGame.Render;

namespace SpaceShooter2.Src.Data;

// handles the application's global state
internal record GlobalState // store the reference instead of copying everything all the time
{
    public Random random;               // contains the random module to ensure things in the game remain random enough
    public List<Astroid> asteroids;     // contains the asteroids that have been spawned and are alive
    public List<Bullet> bullets;        // contains the bullets that have been spawned in and are alive
    public Player player;               // contains the player
    public Timings timings;             // stores different times for time-based operations
    public Textures textures;           // stores the textures that have been pre-loaded
    public GameTime gameTime;           // stores the current gameTime (set in Update)
    public bool lose;                   // flag whether the game should exit

    public PixelControlLayer pcl;       // stores the pixel control layer (allows you to set individual pixels)
    public bool hitboxes;               // stores whether hitboxes should be drawn or not
}
