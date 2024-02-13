using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceShooter2.Src.Data;

namespace SpaceShooter2.Src;
internal static class PlayerManager {
    /// <summary>creates a new player at the centre of the screen</summary>
    public static Player CreatePlayer(Textures textures, int screenWidth, int screenHeight) {
        Player player = new() {
            health = 10,
            transform = new Transform() {
                position = new Vector2(screenWidth / 2),
                scale = Vector2.One * 4.5F,
            },
        };

        //set the correct Y position for the player
        player.transform.position.Y = screenHeight - textures.player.textures[textures.player.currentTexture].Height * player.transform.scale.Y / 2;
        return player;
    }

    /// <summary>allows the user to use input to update the player's position</summary>
    public static Player UpdatePlayer(Player player, int screenWidth) {
        //update player's position based on input
        if (Keyboard.GetState().IsKeyDown(Keys.A))
            player.transform.position.X -= 10;
        if (Keyboard.GetState().IsKeyDown(Keys.D))
            player.transform.position.X += 10;

        //flip the player's position to the other side of the screen if they go "out of boubds"
        if (player.transform.position.X > screenWidth)
            player.transform.position.X %= screenWidth;
        else if (player.transform.position.X < 0)
            player.transform.position.X += screenWidth;

        //return the updated player
        return player;
    }

    /// <summary>draws the player to the screen</summary>
    public static void DrawPlayer(Player player, Textures textures, SpriteBatch spriteBatch) {
        //draw the texture
        spriteBatch.Draw(
            textures.player.textures[textures.player.currentTexture],
            player.transform.position,
            null,
            Color.White,
            player.transform.rotation,
            new Vector2(
                textures.player.textures[textures.player.currentTexture].Width / 2,
                textures.player.textures[textures.player.currentTexture].Height / 2),
            player.transform.scale,
            SpriteEffects.None,
            0);

        //BUG: textures are switching too fast, add a delay somehow
        //finally, update the texture index
        textures.player.currentTexture++; //increase the index of the active texture

        if (textures.player.currentTexture >= textures.player.textures.Count) {
            textures.player.currentTexture = 0;
        }
    }
}
