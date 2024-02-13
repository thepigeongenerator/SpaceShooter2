using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceShooter2.Src;
using SpaceShooter2.Src.Data;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace SpaceShooter2;
public class SpaceShooter : Game {
    //constants
    private const int BULLET_SPAWN_DELAY_MS = 500;  //the delay in miliseconds between bullet spawns
    private const int ASTROID_SPAWN_DELAY_MS = 500; //the delay in miliseconds between astroid spawns
    private const int SCREEN_WIDTH = 980;   //the width of the window
    private const int SCREEN_HEIGHT = 640;  //the height of the window

    //monogame variables
    private readonly GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    //game variables
    private GlobalState _globalState;

    #region constructor
    public SpaceShooter() {
        _graphics = new GraphicsDeviceManager(this);
#if DEBUG
        int seed = 0;
#else
        int seed = (int)DateTime.Now.TimeOfDay.TotalSeconds; //use a seed using time
#endif

        _globalState = new GlobalState() {
            random = new Random(seed), //init random with a seed (also; WHY IS THIS A SIGNED INTEGER!?)
            astroids = new List<Astroid>(),
            bullets = new List<Bullet>(),
            textures = new(),
            timings = new(),
        };

        Content.RootDirectory = "Content";
        IsMouseVisible = false;
        _graphics.PreferredBackBufferWidth = SCREEN_WIDTH;
        _graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;
        _graphics.IsFullScreen = false;

        Console.WriteLine("using seed: {0}", seed);
    }
    #endregion //constructor

    #region game phases
    #region initialize
    protected override void Initialize() {
        base.Initialize();
    }
    #endregion //initialize

    #region loadcontent
    protected override void LoadContent() {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        //load all the textures
        _globalState.textures.astroid = Content.Load<Texture2D>("astroid");
        _globalState.textures.bullet = Content.Load<Texture2D>("bullet");
        _globalState.textures.player.textures.Add(Content.Load<Texture2D>("spaceship/spaceship_0"));
        _globalState.textures.player.textures.Add(Content.Load<Texture2D>("spaceship/spaceship_1"));
        _globalState.textures.player.textures.Add(Content.Load<Texture2D>("spaceship/spaceship_2"));

        //create a player
        _globalState.player = PlayerManager.CreatePlayer(_globalState.textures, SCREEN_WIDTH, SCREEN_HEIGHT);
    }
    #endregion //loadcontent

    #region update
    protected override void Update(GameTime gameTime) {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape) || _globalState.player.health <= 0F) {
            Exit();
        }

        _globalState.player = PlayerManager.UpdatePlayer(_globalState.player, SCREEN_WIDTH);
        UpdateObjects(_globalState.astroids, (astroid) => Astroids.UpdateAstroid(astroid, _globalState.player, _globalState.textures, SCREEN_HEIGHT));
        UpdateObjects(_globalState.bullets, (bullet) => Bullets.UpdateBullet(bullet, _globalState.textures, _globalState.astroids));

        //create new bullet
        RunWhenTimer(gameTime, ref _globalState.timings.bulletSpawnTime, BULLET_SPAWN_DELAY_MS, () =>
            _globalState.bullets.Add(Bullets.CreateBullet(_globalState.player.transform.position)));

        //create new astroid
        RunWhenTimer(gameTime, ref _globalState.timings.astroidSpawnTime, ASTROID_SPAWN_DELAY_MS, () =>
            _globalState.astroids.Add(Astroids.CreateAstroid(_globalState.random, _globalState.player, _globalState.textures, SCREEN_WIDTH)));

        base.Update(gameTime);
    }
    #endregion //update

    #region draw
    protected override void Draw(GameTime gameTime) {
        GraphicsDevice.Clear(new Color(0xFF101010));

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        //draw textures
        PlayerManager.DrawPlayer(_globalState.player, _globalState.textures, _spriteBatch);
        _globalState.astroids.ForEach((astroid) => Astroids.DrawAstroid(astroid, _globalState.textures, _spriteBatch));
        _globalState.bullets.ForEach((bullet) => Bullets.DrawBullet(bullet, _globalState.textures, _spriteBatch));

        _spriteBatch.End();

        base.Draw(gameTime);
    }
    #endregion //draw
    #endregion //game phases

    #region utillity
    /// <summary>
    /// executes an action based on a certain delay
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void RunWhenTimer(GameTime gameTime, ref TimeSpan timing, int msDelay, Action execute) {
        if ((gameTime.TotalGameTime - timing).Milliseconds > msDelay) {
            execute.Invoke();
            timing = gameTime.TotalGameTime;
        }
    }

    /// <summary>
    /// updates the objects through executing a function on them
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void UpdateObjects<T>(List<T> objects, Func<T, T?> func) where T : struct {
        List<int> tRemoveIndices = new();
        for (int i = 0; i < objects.Count; i++) {
            T? t = func.Invoke(objects[i]);

            if (t == null) {
                tRemoveIndices.Add(i);
            }
            else {
                //update T in the list
                objects[i] = (T)t;
            }
        }

        //remove the Ts that needed to be removed
        //indices are in accending order, removing elements will cause the others to change.
        //Reversing the list solves this problem
        tRemoveIndices.Reverse();
        foreach (int i in tRemoveIndices) {
            objects.RemoveAt(i);
        }
    }
    #endregion //utillity

    /// <summary>handles the application's global state</summary>
    private struct GlobalState {
        public Random random;
        public List<Astroid> astroids;
        public List<Bullet> bullets;
        public Player player;
        public Timings timings;
        public Textures textures;
    }
}
