using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceShooter2.Src;
using SpaceShooter2.Src.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using ThePigeonGenerator.Util;

namespace SpaceShooter2;
public partial class SpaceShooter : Core.Game
{
    //monogame variables
    private readonly GraphicsDeviceManager graphics;
    protected override SpriteBatch SpriteBatch { get; set; }

    //game variables
    private readonly GlobalState glob;
    private bool f3Lock;

    // constructor
    public SpaceShooter()
    {
        graphics = new GraphicsDeviceManager(this);
        int seed = (int)DateTime.Now.TimeOfDay.TotalSeconds; //use a seed using time
        Console.WriteLine("using seed: {0}", seed);

        f3Lock = false;
        glob = new GlobalState()
        {
            random = new Random(seed), //init random with a seed (also; WHY IS THIS A SIGNED INTEGER!?)
            asteroids = new List<Astroid>(),
            bullets = new List<Bullet>(),
            assets = new(),
            timings = new(),
            lose = false,
        };

        // load the data from the binary file, if it exists
        if (File.Exists(Const.DATA_PATH))
        {
            byte[] buf = File.ReadAllBytes(Const.DATA_PATH);
            Debug.WriteLine($"read data from '{Path.GetFullPath(Const.DATA_PATH)}'");
            glob.highscore = BinarySerializer.Deserialize<ushort>(buf); // if the binary file is corrupted / failed to convert the file to the type, default to 0
        }

        // game settings
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        // apply graphics settings
        graphics.PreferredBackBufferWidth = Const.SCREEN_WIDTH;
        graphics.PreferredBackBufferHeight = Const.SCREEN_HEIGHT;
        graphics.SynchronizeWithVerticalRetrace = Const.VSYNC;
        graphics.IsFullScreen = false;
        TargetElapsedTime = TimeSpan.FromSeconds(1.0F / Const.UPDATES_PER_SECOND);
    }

    // executes keybind actions mainly for debugging purposes.
    // this has been left in the release build on purpose, to showcase these tools
    private void DebugKeybinds()
    {
        // exit if escape has been pressed
        if (glob.keyboard.IsKeyDown(Keys.Escape))
            Exit();

        // don't do anything else if we've lost the game
        if (glob.lose == true)
            return;

        // if F3 is pressed, toggle hitbox visability
        if (f3Lock == false && glob.keyboard.IsKeyDown(Keys.F3))
        {
            glob.hitboxes = !glob.hitboxes;
            f3Lock = true;
        }
        else if (glob.keyboard.IsKeyUp(Keys.F3))
        {
            f3Lock = false;
        }

        // damage the player with the health that they have if f12 has been pressed (instantly kills the player and loses the game)
        if (glob.keyboard.IsKeyDown(Keys.F12))
            glob.player.Damage(glob.player.Health);
    }

    // loads all the assets we will use for the game
    // aditionally, it initializes the objects used in the game
    protected override void LoadContent()
    {
        // create a spritebatch instance
        SpriteBatch = new SpriteBatch(GraphicsDevice);

        //load all the assets
        glob.assets.font = Content.Load<SpriteFont>(Const.TEXTURE_FONT);
        glob.assets.astroid = Content.Load<Texture2D>(Const.TEXTURE_ASTEROID);
        glob.assets.bullet = Content.Load<Texture2D>(Const.TEXTURE_BULLET);
        glob.assets.player.textures.Add(Content.Load<Texture2D>(Const.TEXTURE_SPACESHIP_0));
        glob.assets.player.textures.Add(Content.Load<Texture2D>(Const.TEXTURE_SPACESHIP_1));
        glob.assets.player.textures.Add(Content.Load<Texture2D>(Const.TEXTURE_SPACESHIP_2));
        glob.assets.damage = Content.Load<SoundEffect>(Const.SFX_DAMAGE);
        glob.assets.destroyAsteroid = Content.Load<SoundEffect>(Const.SFX_DESTROY_ASTEROID);
        glob.assets.lose = Content.Load<SoundEffect>(Const.SFX_LOSE);

        // init game objects
        glob.player = new Player(glob);
        glob.spawner = new Spawner(glob);
        glob.ui = new UI(glob);
        glob.pcl = new(GraphicsDevice);
        base.LoadContent();
    }

    // updates the game
    protected override void Update(GameTime gameTime)
    {
        // update some globals
        glob.gameTime = gameTime;
        glob.keyboard = Keyboard.GetState();

        // excute the debug keybinds to perform debug actions
        DebugKeybinds();

        // if the game has been lost, check if the enter key has been pressed, exit if so
        if (glob.lose == true && glob.keyboard.IsKeyDown(Keys.Enter))
            Exit();

        base.Update(gameTime);
    }

    // draws the game
    protected override void Draw(GameTime gameTime)
    {
        // clear what has previously been drawn
        GraphicsDevice.Clear(new Color(0xFF101010));

        // draw everything
        SpriteBatch.Begin(samplerState: SamplerState.PointClamp, sortMode: SpriteSortMode.FrontToBack);
        glob.pcl.Draw(SpriteBatch);     // draw the pixel control layer
        DrawObjects();                  // draw all the gameObjects with IDraw implemented
        SpriteBatch.End();

        // clear pcl's internal buffer *after* drawing, otherwise it won't know what to draw
        glob.pcl.ClearBuffer();

        base.Draw(gameTime);
    }

    // called when the game is exiting
    protected override void OnExiting(object sender, EventArgs args)
    {
        // note: not checking whether we've lost in case of accedentally closing
        // write the data to the binary file
        byte[] buf = BinarySerializer.Serialize(glob.highscore);                    // serialize the highscore to a buffer
        File.WriteAllBytes(Const.DATA_PATH, buf);                                   // write this buffer to the data path
        Debug.WriteLine($"saved data to '{Path.GetFullPath(Const.DATA_PATH)}'");    // log that the data has been saved

        base.OnExiting(sender, args);
    }
}
