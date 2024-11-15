using Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceShooter2.Src.Data;
using SpaceShooter2.Src.Util;

namespace SpaceShooter2.Src;

internal class Player : GameObject
{
    public byte health = 0;

    // creates a new player at the centre of the screen
    public Player(Textures textures)
    {
        health = 10;
        transform.position = new Vector2(Const.SCREEN_WIDTH / 2);
        transform.scale = Vector2.One * 4.5F;

        //set the correct Y position for the player
        transform.position.Y = Const.SCREEN_HEIGHT - textures.player.textures[textures.player.currentTexture].Height * transform.scale.Y / 2;
    }

    // allows the user to use input to update the player's position
    public void Update()
    {
        //update player's position based on input
        if (Keyboard.GetState().IsKeyDown(Keys.A))
            transform.position.X -= 10;
        if (Keyboard.GetState().IsKeyDown(Keys.D))
            transform.position.X += 10;

        //flip the player's position to the other side of the screen if they go "out of boubds"
        if (transform.position.X > Const.SCREEN_WIDTH)
            transform.position.X %= Const.SCREEN_WIDTH;
        else if (transform.position.X < 0)
            transform.position.X += Const.SCREEN_WIDTH;
    }

    // draws the player to the screen
    public void Draw(GlobalState glob, GameTime gameTime, SpriteBatch spriteBatch)
    {
        Textures textures = glob.textures;

        //draw the texture
        spriteBatch.Draw(
            textures.player.textures[textures.player.currentTexture],
            transform.position,
            null,
            Color.White,
            transform.rotation,
            new Vector2(
                textures.player.textures[textures.player.currentTexture].Width / 2,
                textures.player.textures[textures.player.currentTexture].Height / 2),
            transform.scale,
            SpriteEffects.None,
            0.0F);

        //update the texture index
        TimeUtils.RunWhenTimer(gameTime, ref glob.timings.playerSwitchTextureTime, 100, () => {
            textures.player.currentTexture++; //increase the index of the active texture
            if (textures.player.currentTexture >= textures.player.textures.Count)
            {
                textures.player.currentTexture = 0;
            }
        });
    }
}
