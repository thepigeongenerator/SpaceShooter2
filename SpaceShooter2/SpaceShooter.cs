using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceShooter2.Src;
using SpaceShooter2.Src.Data;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace SpaceShooter2;
public partial class SpaceShooter : Core.Game
{
    //constants
    private const int BULLET_SPAWN_DELAY_MS = 500;  //the delay in milliseconds between bullet spawns
    private const int ASTROID_SPAWN_DELAY_MS = 500; //the delay in milliseconds between astroid spawns
    private const int SCREEN_WIDTH = 980;   //the width of the window
    private const int SCREEN_HEIGHT = 640;  //the height of the window

    //monogame variables
    private readonly GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch;

    //game variables
    private GlobalState globalState;

    public SpaceShooter()
    {
        graphics = new GraphicsDeviceManager(this);
#if DEBUG
        int seed = 0;
#else
        int seed = (int)DateTime.Now.TimeOfDay.TotalSeconds; //use a seed using time
#endif

        globalState = new GlobalState()
        {
            random = new Random(seed), //init random with a seed (also; WHY IS THIS A SIGNED INTEGER!?)
            asteroids = new List<Astroid>(),
            bullets = new List<Bullet>(),
            textures = new(),
            timings = new(),
        };

        // game settings
        Content.RootDirectory = "Content";
        IsMouseVisible = false;
        graphics.PreferredBackBufferWidth = SCREEN_WIDTH;
        graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;
        graphics.IsFullScreen = false;

        Console.WriteLine("using seed: {0}", seed);
    }


    protected override void Initialize() => base.Initialize();

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);

        //load all the textures
        globalState.textures.astroid = Content.Load<Texture2D>("astroid");
        globalState.textures.bullet = Content.Load<Texture2D>("bullet");
        globalState.textures.player.textures.Add(Content.Load<Texture2D>("spaceship/spaceship_0"));
        globalState.textures.player.textures.Add(Content.Load<Texture2D>("spaceship/spaceship_1"));
        globalState.textures.player.textures.Add(Content.Load<Texture2D>("spaceship/spaceship_2"));

        //create a player
        globalState.player = new Player(globalState.textures, SCREEN_WIDTH, SCREEN_HEIGHT);
    }

    protected override void Update(GameTime gameTime)
    {
#if DEBUG
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
#endif

        globalState.player.Update(SCREEN_WIDTH);
        ForEachObject<Astroid>((astroid) => astroid.Update(globalState.player, globalState.textures, SCREEN_HEIGHT));
        ForEachObject<Bullet>((bullet) => bullet.Update(globalState.textures, globalState.asteroids));

        //create new bullet
        RunWhenTimer(gameTime, ref globalState.timings.bulletSpawnTime, BULLET_SPAWN_DELAY_MS, () =>
            globalState.bullets.Add(new Bullet(globalState)));

        //create new astroid
        RunWhenTimer(gameTime, ref globalState.timings.astroidSpawnTime, ASTROID_SPAWN_DELAY_MS, () =>
            globalState.asteroids.Add(new Astroid(globalState, SCREEN_WIDTH)));

        base.Update(gameTime);
    }

    // draws the game
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(new Color(0xFF101010));

        spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        //draw textures
        globalState.player.Draw(globalState.textures, spriteBatch);
        ForEachObject<Astroid>((astroid) => astroid.Draw(globalState.textures, spriteBatch));
        ForEachObject<Bullet>((bullet) => bullet.Draw(globalState.textures, spriteBatch));

        spriteBatch.End();

        base.Draw(gameTime);
    }

    // executes an action based on a certain delay
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void RunWhenTimer(GameTime gameTime, ref TimeSpan timing, int msDelay, Action execute)
    {
        if ((gameTime.TotalGameTime - timing).Milliseconds > msDelay)
        {
            execute.Invoke();
            timing = gameTime.TotalGameTime;
        }
    }
}
