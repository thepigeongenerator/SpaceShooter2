using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter2.Src.Data;
using System;
namespace SpaceShooter2.Src;
internal static class Astroids {
    const float MAX_SIZE = 2F;

    /// <summary>creates a new astroid with a random position at the top of the screen. There is also a random chance this astroid will have <see cref="Astroid.unbreakable"/> set to <see langword="true"/></summary>
    public static Astroid CreateAstroid(Random rand, Player player, Textures textures, int screenWidth) {
        Astroid astroid = new() {
            transform = new Transform {
                rotation = rand.NextSingle() * MathF.Tau, //random rotation from 0..TAU
                scale = Vector2.One * ((rand.NextSingle() + 1F) * (MAX_SIZE / 2F)), //make the scaling random
                /* MAX_SIZE is devided by 2 because NextSingle produces a value between 0..1.
                   We add 1 to make sure we don't get any zero issues. Making the range 1..2.
                   Multiplying by MAX_SIZE then would equate to the Actual max size to be 2 * MAX_SIZE. */
            },
        };

        //put the astroid offscreen so the player doesn't see it spawning in
        astroid.transform.position.Y = -(textures.astroid.Width * astroid.transform.scale.X);

        //have a random chance that the astroid is unbreakable (the player needs to dodge these astroids)
        if (rand.NextSingle() < 0.10F) { //10% chance
            astroid.unbreakable = true;
            astroid.transform.position.X = player.transform.position.X;
        }
        else {
            astroid.unbreakable = false;
            astroid.transform.position.X = rand.Next(0, screenWidth);
        }

        return astroid;
    }

    /// <summary>makes the astroid move down and damages the player if it interacts</summary>
    public static Astroid? UpdateAstroid(Astroid astroid, Player player, Textures textures, int screenHeight) {
        //move the astroid down
        astroid.transform.position.Y += -astroid.transform.scale.X + MAX_SIZE;

        //if the astroid fell off the screen; destroy the astoid
        if (astroid.transform.position.Y - (textures.astroid.Width * astroid.transform.scale.X) > screenHeight)
            return null;

        //rotate the astroid
        astroid.transform.rotation += MathF.Tau / 360 * -(astroid.transform.scale.X - MAX_SIZE);
        astroid.transform.rotation %= MathF.Tau;
        return astroid;
    }

    /// <summary>draws the astroid with the spriteBatch, make sure to first set the astroid texture using <see cref="SetAstroidTexture(Texture2D)"/></summary>
    public static void DrawAstroid(Astroid astroid, Textures textures, SpriteBatch spriteBatch) {
        Color color = astroid.unbreakable ? new Color(0xFF888888) : Color.White; //make the astroid texture slightly darker if unbreakable

        //draw the texture
        spriteBatch.Draw(
            textures.astroid,
            astroid.transform.position,
            null,
            color,
            astroid.transform.rotation,
            new Vector2( //make the origin of the astroid at the centre
                textures.astroid.Width / 2F,
                textures.astroid.Height / 2F),
            astroid.transform.scale,
            SpriteEffects.None,
            0);
    }
}
