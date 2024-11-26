﻿using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceShooter2.Src;
using SpaceShooter2.Src.Data;
using SpaceShooter2.Src.Util;
using System;
using System.Collections.Generic;

namespace SpaceShooter2;
public partial class SpaceShooter : Core.Game
{
    //monogame variables
    private readonly GraphicsDeviceManager graphics;
    protected override SpriteBatch SpriteBatch { get; set; }

    //game variables
    private readonly GlobalState globalState;

    public SpaceShooter()
    {
        graphics = new GraphicsDeviceManager(this);
#if DEBUG
        int seed = 0;
#else
        int seed = (int)DateTime.Now.TimeOfDay.TotalSeconds; //use a seed using time
#endif
        Console.WriteLine("using seed: {0}", seed);


        globalState = new GlobalState()
        {
            random = new Random(seed), //init random with a seed (also; WHY IS THIS A SIGNED INTEGER!?)
            asteroids = new List<Astroid>(),
            bullets = new List<Bullet>(),
            textures = new(),
            timings = new(),
            exit = false,
        };

        // game settings
        Content.RootDirectory = "Content";
        IsMouseVisible = false;

        // graphics settings
        graphics.PreferredBackBufferWidth = Const.SCREEN_WIDTH;
        graphics.PreferredBackBufferHeight = Const.SCREEN_HEIGHT;
        graphics.SynchronizeWithVerticalRetrace = Const.VSYNC;
        graphics.IsFullScreen = false;
        TargetElapsedTime = TimeSpan.FromSeconds(1.0F / Const.UPDATES_PER_SECOND);
    }

    protected override void LoadContent()
    {
        SpriteBatch = new SpriteBatch(GraphicsDevice);

        //load all the textures
        globalState.textures.astroid = Content.Load<Texture2D>(Const.TEXTURE_ASTEROID);
        globalState.textures.bullet = Content.Load<Texture2D>(Const.TEXTURE_BULLET);
        globalState.textures.player.textures.Add(Content.Load<Texture2D>(Const.TEXTURE_SPACESHIP_0));
        globalState.textures.player.textures.Add(Content.Load<Texture2D>(Const.TEXTURE_SPACESHIP_1));
        globalState.textures.player.textures.Add(Content.Load<Texture2D>(Const.TEXTURE_SPACESHIP_2));

        //create a player
        globalState.player = new Player(globalState);
        globalState.pcl = new(GraphicsDevice);
    }

    protected override void Update(GameTime gameTime)
    {
#if DEBUG
        if (Keyboard.GetState().IsKeyDown(Keys.Escape) || globalState.exit == true)
            Exit();
#endif

        globalState.gameTime = gameTime;

        // if F3 is pressed, toggle hitboxes mode
        if (globalState.hitboxesLock == false && Keyboard.GetState().IsKeyDown(Keys.F3))
        {
            globalState.hitboxes = !globalState.hitboxes;
            globalState.hitboxesLock = true;
        }
        else if (Keyboard.GetState().IsKeyUp(Keys.F3))
            globalState.hitboxesLock = false;

        //create new bullet
        TimeUtils.RunWhenTimer(gameTime, ref globalState.timings.bulletSpawnTime, Const.BULLET_SPAWN_DELAY_MS, () =>
            globalState.bullets.Add(new Bullet(globalState)));

        //create new astroid
        TimeUtils.RunWhenTimer(gameTime, ref globalState.timings.astroidSpawnTime, Const.ASTROID_SPAWN_DELAY_MS, () =>
            globalState.asteroids.Add(new Astroid(globalState, Const.SCREEN_WIDTH)));


        base.Update(gameTime);
    }

    // draws the game
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(new Color(0xFF101010));

        // begin the sprite batch
        SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        // draw everything
        DrawObjects();
        globalState.pcl.Draw(SpriteBatch);

        // end the sprite batch
        SpriteBatch.End();

        globalState.pcl.ClearBuffer(); // clear the internal buffer *after* drawing, otherwise it'll fail to draw

        base.Draw(gameTime);
    }
}
