using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter2.Src.Data;
using Core;
using System.Collections.Generic;
using ThePigeonGenerator.MonoGame.Render;
using SpaceShooter2.Src.Util;

namespace SpaceShooter2.Src;

internal class Astroid : GameObject
{
    public readonly bool unbreakable;
    private readonly List<Astroid> asteroids;

    public readonly float radius;

    // creates a new astroid with a random position at the top of the screen.
    // there is also a random chance this astroid will be unbreakable
    public Astroid(GlobalState glob, int screenWidth)
    {
        transform.rotation = glob.random.NextSingle() * MathF.Tau; //random rotation from 0..TAU
        transform.scale = Vector2.One * ((glob.random.NextSingle() + 1F) * Const.MAX_ASTROID_SIZE / 2F); //make the scaling random
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

        radius = (glob.textures.astroid.Width / 2.0F * transform.scale.X) + (glob.textures.bullet.Width / 2.0F); // add the half of the bullet's texture's width, to compensate for
        asteroids = glob.asteroids;
    }

    public bool OnAstroid(Vector2 pos, GlobalState glob)
    {
        return VectorDetection.InCircle(transform.position - pos, radius);
    }

    // makes the astroid move down and damages the player if it interacts
    public void Update(GlobalState glob)
    {
        // move the astroid down
        transform.position.Y += -1 * transform.scale.X + Const.MAX_ASTROID_SIZE;

        // if the astroid fell off the screen; destroy the astroid
        if (transform.position.Y - radius > Const.SCREEN_HEIGHT)
        {
            Dispose();
            return;
        }

        // rotate the astroid
        transform.rotation += MathF.Tau / 360 * -1 * (transform.scale.X - Const.MAX_ASTROID_SIZE);
        transform.rotation %= MathF.Tau;

        if (glob.hitboxes)
            glob.pcl.SetCircle((int)transform.position.X, (int)transform.position.Y, (int)radius, (Color)Colour.Green);
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
        asteroids.Remove(this);
    }

}
