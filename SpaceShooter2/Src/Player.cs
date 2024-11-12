using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceShooter2.Src.Data;

namespace SpaceShooter2.Src;

internal class Player
{
    readonly public Transform transform = new();
    public byte health = 0;

    // creates a new player at the centre of the screen
    public Player(Textures textures, int screenWidth, int screenHeight)
    {
        health = 10;
            transform = new Transform()
            {
                position = new Vector2(screenWidth / 2),
                scale = Vector2.One * 4.5F,
            };

        //set the correct Y position for the player
        transform.position.Y = screenHeight - textures.player.textures[textures.player.currentTexture].Height * transform.scale.Y / 2;
    }

    // allows the user to use input to update the player's position
    public bool Update(int screenWidth)
    {
        //update player's position based on input
        if (Keyboard.GetState().IsKeyDown(Keys.A))
            transform.position.X -= 10;
        if (Keyboard.GetState().IsKeyDown(Keys.D))
            transform.position.X += 10;

        //flip the player's position to the other side of the screen if they go "out of boubds"
        if (transform.position.X > screenWidth)
            transform.position.X %= screenWidth;
        else if (transform.position.X < 0)
            transform.position.X += screenWidth;

        return true;
    }

    // draws the player to the screen
    public void Draw(Textures textures, SpriteBatch spriteBatch)
    {
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

        //BUG: textures are switching too fast, add a delay somehow
        //finally, update the texture index
        textures.player.currentTexture++; //increase the index of the active texture

        if (textures.player.currentTexture >= textures.player.textures.Count)
        {
            textures.player.currentTexture = 0;
        }
    }
}
