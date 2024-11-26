using System.Diagnostics.Tracing;
using Core;
using Core.Polygons;
using Core.Util;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceShooter2.Src.Data;
using SpaceShooter2.Src.Util;

namespace SpaceShooter2.Src;

internal class Player : TexturedGameObject, IUpdate
{
    private const uint RED = 0xFF0000FF; // monogame is ABGR
    private readonly GlobalState glob;
    public readonly Polygon2 hitbox;

    private sbyte health = 0;
    private float startColourChange = 0.0F;

    public sbyte Health => health;

    // creates a new player at the centre of the screen
    public Player(GlobalState glob) : base(glob.textures.player.textures[0])
    {
        health = Const.PLAYER_MAX_HEALTH;
        transform.position = new Vector2(Const.SCREEN_WIDTH / 2);
        transform.scale = Vector2.One * 4.5F;
        transform.origin = Vector2.One / 2.0F;

        {
            float x = Width / 2;
            float y = Height / 2;
            hitbox = new(
                new(0, -y),     // top centre
                new(x, y),      // bottom right
                new(-x, y));    // bottom left
        }

        //set the correct Y position for the player
        transform.position.Y = Const.SCREEN_HEIGHT - (Height * transform.origin.Y) - (4 * transform.scale.Y);
        this.glob = glob;
    }

    // damages the player
    public void Damage(sbyte amount = 1)
    {
        health -= amount;
        textureTint = new Color(RED);
        startColourChange = -1;

        if (health <= 0)
        {
            glob.lose = true;
            return;
        }
    }

    // allows the user to use input to update the player's position
    public void Update()
    {
        //update player's position based on input
        if (Keyboard.GetState().IsKeyDown(Keys.A))
            transform.position.X -= Const.PLAYER_SPEED * glob.gameTime.DeltaTime();
        if (Keyboard.GetState().IsKeyDown(Keys.D))
            transform.position.X += Const.PLAYER_SPEED * glob.gameTime.DeltaTime();

        // store start if it's less than 0
        if (startColourChange < 0)
            startColourChange = (float)glob.gameTime.TotalGameTime.TotalSeconds;

        // if the texture tint isn't white, lerp it to white
        if (textureTint != Color.White)
            textureTint = Color.Lerp(new Color(RED), Color.White, (float)glob.gameTime.TotalGameTime.TotalSeconds - startColourChange);

        //flip the player's position to the other side of the screen if they go "out of bounds"
        if (transform.position.X > Const.SCREEN_WIDTH)
            transform.position.X %= Const.SCREEN_WIDTH;
        else if (transform.position.X < 0)
            transform.position.X += Const.SCREEN_WIDTH;

        // set the hitbox's position to the player's position
        hitbox.position = transform.position;

        if (glob.hitboxes)
            glob.pcl.SetPolygon(hitbox, Color.Green);

        //update the texture index if the timer ran out
        TimeUtils.RunWhenTimer(glob.gameTime, ref glob.timings.playerSwitchTextureTime, 100, () =>
        {
            glob.textures.player.currentTexture++; //increase the index of the active texture
            if (glob.textures.player.currentTexture >= glob.textures.player.textures.Count)
            {
                glob.textures.player.currentTexture = 0;
            }
            texture = glob.textures.player.textures[glob.textures.player.currentTexture];
        });
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.DrawString(glob.textures.font, $"Health: {health}/{Const.PLAYER_MAX_HEALTH}", Vector2.One * 10, Color.Red, 0.0F, Vector2.Zero, 0.4F, SpriteEffects.None, 1);

        // call base's draw
        base.Draw(spriteBatch);
    }
}
