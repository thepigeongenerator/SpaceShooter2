using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter2.Src.Data;
using Core;
using System.Collections.Generic;

namespace SpaceShooter2.Src;

internal class Astroid : GameObject
{
    private const float MAX_SIZE = 2F;

    public readonly bool unbreakable;
    private readonly List<Astroid> astroids;

    // creates a new astroid with a random position at the top of the screen.
    // there is also a random chance this astroid will be unbreakable
    public Astroid(GlobalState glob, int screenWidth)
    {
        transform.rotation = glob.random.NextSingle() * MathF.Tau; //random rotation from 0..TAU
        transform.scale = Vector2.One * ((glob.random.NextSingle() + 1F) * (MAX_SIZE / 2F)); //make the scaling random
        /* MAX_SIZE is divided by 2 because NextSingle produces a value between 0..1.
           We add 1 to make sure we don't get any zero issues. Making the range 1..2.
           Multiplying by MAX_SIZE then would equate to the Actual max size to be 2 * MAX_SIZE. */

        // put the astroid offscreen so the player doesn't see it spawning in
        transform.position.Y = -(glob.textures.astroid.Width * transform.scale.X);

        // have a random chance that the astroid is unbreakable (the player needs to dodge these astroids)
        if (glob.random.NextSingle() < 0.10F) // 10% chance
        {
            unbreakable = true;
            transform.position.X = glob.player.transform.position.X;
        }
        else
        {
            unbreakable = false;
            transform.position.X = glob.random.Next(0, screenWidth);
        }

        astroids = glob.asteroids;
    }

    // makes the astroid move down and damages the player if it interacts
    public void Update(Player player, Textures textures, int screenHeight)
    {
        // move the astroid down
        transform.position.Y += -1 * transform.scale.X + MAX_SIZE;

        // if the astroid fell off the screen; destroy the astroid
        if (transform.position.Y - (textures.astroid.Width * transform.scale.X) > screenHeight)
        {
            Dispose();
            return;
        }

        // rotate the astroid
        transform.rotation += MathF.Tau / 360 * -1 * (transform.scale.X - MAX_SIZE);
        transform.rotation %= MathF.Tau;
    }

    // draws the astroid with the spriteBatch
    public void Draw(Textures textures, SpriteBatch spriteBatch)
    {
        Color color = unbreakable ? new Color(0xFF888888) : Color.White; //make the astroid texture slightly darker if unbreakable

        // draw the texture
        spriteBatch.Draw(
            textures.astroid,
            transform.position,
            null,
            color,
            transform.rotation,
            new Vector2( //make the origin of the astroid at the centre
                textures.astroid.Width / 2F,
                textures.astroid.Height / 2F),
            transform.scale,
            SpriteEffects.None,
            0
        );
    }

    protected override void OnDispose()
    {
        astroids.Remove(this);
    }

}
