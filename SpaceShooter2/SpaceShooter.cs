using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceShooter2.Src;
using SpaceShooter2.Src.Data;
using SpaceShooter2.Src.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.IO;

namespace SpaceShooter2;
public partial class SpaceShooter : Core.Game
{
    //monogame variables
    private readonly GraphicsDeviceManager graphics;
    protected override SpriteBatch SpriteBatch { get; set; }

    //game variables
    private readonly GlobalState globalState;
    private bool f3Lock;

    public SpaceShooter()
    {
        graphics = new GraphicsDeviceManager(this);
#if DEBUG
        int seed = 0;
#else
        int seed = (int)DateTime.Now.TimeOfDay.TotalSeconds; //use a seed using time
#endif
        Console.WriteLine("using seed: {0}", seed);

        f3Lock = false;
        globalState = new GlobalState()
        {
            random = new Random(seed), //init random with a seed (also; WHY IS THIS A SIGNED INTEGER!?)
            asteroids = new List<Astroid>(),
            bullets = new List<Bullet>(),
            textures = new(),
            timings = new(),
            lose = false,
        };

        // loads the data stored in the binary file
        LoadData();

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

    private void LoadData()
    {
        if (File.Exists(Const.DATA_PATH) == false)
            return;

        byte[] buf = File.ReadAllBytes(Const.DATA_PATH);

        for (int i = buf.Length - 1; i > 0; i--)
        {
            globalState.highScore <<= 8;
            globalState.highScore |= buf[i];
        }
    }

    private void StoreData()
    {
        byte[] buf = new byte[sizeof(ushort)];

        for (int i = 0; i < buf.Length; i++)
        {
            ushort s = globalState.highScore;
            s &= (ushort)(0xFF << i * 8);
            buf[i] = (byte)(s >> i * 8);
        }

        File.WriteAllBytes(Const.DATA_PATH, buf);
    }

    protected override void LoadContent()
    {
        SpriteBatch = new SpriteBatch(GraphicsDevice);

        //load all the textures
        globalState.textures.font = Content.Load<SpriteFont>(Const.TEXTURE_FONT);
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
        // exit if escape has been pressed
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // if you lost the game
        if (globalState.lose == true)
        {
            // draw a bunch of red dots
            for (int i = 0; i < globalState.pcl.buffer.Length; i++)
            {
                if (i % 16 == 0)
                    globalState.pcl.buffer[i] = Color.Red;
            }

            // if any key is pressed, exit
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                Exit();

            return;
        }

        // set the game time to the current time
        globalState.gameTime = gameTime;

        // if F3 is pressed, toggle hitboxes mode
        if (f3Lock == false && Keyboard.GetState().IsKeyDown(Keys.F3))
        {
            globalState.hitboxes = !globalState.hitboxes;
            f3Lock = true;
        }
        else if (Keyboard.GetState().IsKeyUp(Keys.F3))
            f3Lock = false;

        if (Keyboard.GetState().IsKeyDown(Keys.F12))
        {
            globalState.lose = true;
        }

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
        SpriteBatch.Begin(samplerState: SamplerState.PointClamp, sortMode: SpriteSortMode.FrontToBack);

        // draw everything
        globalState.pcl.Draw(SpriteBatch);

        if (globalState.lose == false)
            DrawObjects(); // draw all the gameObjects with IDraw implemented
        else
        {
            SpriteBatch.DrawString(globalState.textures.font, "YOU LOST", new Vector2(Const.SCREEN_WIDTH / 2, Const.SCREEN_HEIGHT / 2), Color.Red, Vector2.One / 2, 1.0F, 1.0F);
            SpriteBatch.DrawString(globalState.textures.font, $"score: {globalState.score}{(globalState.score == 0 ? "" : "0")}\nhigh score: {globalState.highScore}{(globalState.highScore == 0 ? "" : "0")}", new Vector2(Const.SCREEN_WIDTH / 2, 10), Color.White, new Vector2(0.5F, 0), 0.5F, 1.0F);
            SpriteBatch.DrawString(globalState.textures.font, "press [enter] to exit", new Vector2(Const.SCREEN_WIDTH / 2, Const.SCREEN_HEIGHT / 2 + 40), Color.Red, Vector2.One / 2.0F, 0.5F, 1.0F);
        }

        // end the sprite batch
        SpriteBatch.End();

        globalState.pcl.ClearBuffer(); // clear the internal buffer *after* drawing, otherwise it'll fail to draw

        base.Draw(gameTime);
    }

    protected override void OnExiting(object sender, EventArgs args)
    {
        if (globalState.lose)
        {
            StoreData();
        }

        base.OnExiting(sender, args);
    }
}
