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
        graphics.PreferredBackBufferWidth = Const.SCREEN_WIDTH;
        graphics.PreferredBackBufferHeight = Const.SCREEN_HEIGHT;
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
        globalState.player = new Player(globalState.textures);
        globalState.pcl = new(GraphicsDevice);
    }

    protected override void Update(GameTime gameTime)
    {
#if DEBUG
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
#endif

        if (globalState.hitboxesLock == false && Keyboard.GetState().IsKeyDown(Keys.F3))
        {
            globalState.hitboxes = !globalState.hitboxes;
            globalState.hitboxesLock = true;
        }
        else if (Keyboard.GetState().IsKeyUp(Keys.F3))
            globalState.hitboxesLock = false;

        globalState.player.Update();
        ForEachObject<Astroid>((astroid) => astroid.Update(globalState));
        ForEachObject<Bullet>((bullet) => bullet.Update(globalState));

        //create new bullet
        RunWhenTimer(gameTime, ref globalState.timings.bulletSpawnTime, Const.BULLET_SPAWN_DELAY_MS, () =>
            globalState.bullets.Add(new Bullet(globalState)));

        //create new astroid
        RunWhenTimer(gameTime, ref globalState.timings.astroidSpawnTime, Const.ASTROID_SPAWN_DELAY_MS, () =>
            globalState.asteroids.Add(new Astroid(globalState, Const.SCREEN_WIDTH)));

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

        globalState.pcl.Draw(spriteBatch);

        spriteBatch.End();

        globalState.pcl.ClearBuffer(); // clear the internal buffer *after* drawing, otherwise it'll fail to draw

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
