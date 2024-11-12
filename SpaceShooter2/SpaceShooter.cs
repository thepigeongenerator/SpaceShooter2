using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceShooter2.Src;
using SpaceShooter2.Src.Data;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace SpaceShooter2;
public class SpaceShooter : Game
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

    #region constructor
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
    #endregion //constructor

    #region game phases
    #region initialize
    protected override void Initialize()
    {
        base.Initialize();
    }
    #endregion //initialize

    #region loadcontent
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
    #endregion //loadcontent

    #region update
    protected override void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape) || globalState.player.health <= 0F)
        {
            Exit();
        }

        globalState.player.Update(SCREEN_WIDTH);
        UpdateObjects(globalState.asteroids, (astroid) => astroid.Update(globalState.player, globalState.textures, SCREEN_HEIGHT));
        UpdateObjects(globalState.bullets, (bullet) => bullet.Update(globalState.textures, globalState.asteroids));

        //create new bullet
        RunWhenTimer(gameTime, ref globalState.timings.bulletSpawnTime, BULLET_SPAWN_DELAY_MS, () =>
            globalState.bullets.Add(new Bullet(globalState.player.transform.position)));

        //create new astroid
        RunWhenTimer(gameTime, ref globalState.timings.astroidSpawnTime, ASTROID_SPAWN_DELAY_MS, () =>
            globalState.asteroids.Add(new Astroid(globalState.random, globalState.player, globalState.textures, SCREEN_WIDTH)));

        base.Update(gameTime);
    }
    #endregion //update

    #region draw
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(new Color(0xFF101010));

        spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        //draw textures
        globalState.player.Draw(globalState.textures, spriteBatch);
        globalState.asteroids.ForEach((astroid) => astroid.Draw(globalState.textures, spriteBatch));
        globalState.bullets.ForEach((bullet) => bullet.Draw(globalState.textures, spriteBatch));

        spriteBatch.End();

        base.Draw(gameTime);
    }
    #endregion //draw
    #endregion //game phases

    #region utillity
    /// <summary>
    /// executes an action based on a certain delay
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void RunWhenTimer(GameTime gameTime, ref TimeSpan timing, int msDelay, Action execute)
    {
        if ((gameTime.TotalGameTime - timing).Milliseconds > msDelay)
        {
            execute.Invoke();
            timing = gameTime.TotalGameTime;
        }
    }

    /// <summary>
    /// updates the objects through executing a function on them
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void UpdateObjects<T>(List<T> objects, Func<T, bool> func)
    {
        List<int> tRemoveIndices = new();
        for (int i = 0; i < objects.Count; i++)
        {
            // add the index if the function returns FALSE
            if (func.Invoke(objects[i]) == false)
            {
                tRemoveIndices.Add(i);
            }
        }

        //remove the Ts that needed to be removed
        //indices are in accending order, removing elements will cause the others to change.
        //Reversing the list solves this problem
        tRemoveIndices.Reverse();
        for (int i = tRemoveIndices.Count - 1; i >= 0; i--)
        {
            objects.RemoveAt(tRemoveIndices[i]);
        }
    }
    #endregion //utillity

    /// <summary>handles the application's global state</summary>
    private struct GlobalState
    {
        public Random random;
        public List<Astroid> asteroids;
        public List<Bullet> bullets;
        public Player player;
        public Timings timings;
        public Textures textures;
    }
}
