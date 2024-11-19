using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter2.Src.Data;
using Core;
using ThePigeonGenerator.MonoGame.Render;
using SpaceShooter2.Src.Util;

namespace SpaceShooter2.Src;

internal class Astroid : TexturedGameObject, IUpdate
{
    // game fields
    public readonly GlobalState glob = null;
    public Texture2D astroidTexture = null;

    // asteroid fields
    public readonly bool unbreakable;   // whether the asteroid can be destroyed by a bullet
    public readonly float radius;       // the radius of the asteroid, includes scale


    // creates a new astroid with a random position at the top of the screen.
    // there is also a random chance this astroid will be unbreakable
    public Astroid(GlobalState glob, int screenWidth) : base(glob.textures.astroid)
    {
        transform.origin = Vector2.One / 2.0F;  // put the origin in the centre
        transform.rotation = glob.random.NextSingle() * MathF.Tau; //random rotation from 0..TAU
        transform.scale = Vector2.One * ((glob.random.NextSingle() + 1F) * Const.MAX_ASTROID_SIZE / 2F); //make the scaling random
        /* MAX_SIZE is divided by 2 because NextSingle produces a value between 0..1.
           We add 1 to make sure we don't get any zero issues. Making the range 1..2.
           Multiplying by MAX_SIZE then would equate to the Actual max size to be 2 * MAX_SIZE. */

        // put the astroid offscreen so the player doesn't see it spawning in
        transform.position.Y = -(glob.textures.astroid.Width * transform.scale.X);

        // have a random chance that the astroid is unbreakable (the player needs to dodge these asteroids)
        if (glob.random.NextSingle() < 0.10F) // 10% chance
        {
            unbreakable = true;
            textureTint = new Color(0xFF888888);    // tint the texture if the asteroid is unbreakable
            transform.position.X = glob.player.transform.position.X;
        }
        else
        {
            unbreakable = false;
            transform.position.X = glob.random.Next(0, screenWidth);
        }

        radius = (glob.textures.astroid.Width / 2.0F * transform.scale.X) + (glob.textures.bullet.Width / 2.0F); // add the half of the bullet's texture's width, to compensate for
        this.glob = glob;
    }

    public bool OnAstroid(Vector2 pos)
    {
        return VectorDetection.InCircle(transform.position - pos, radius);
    }

    // makes the astroid move down and damages the player if it interacts
    public void Update()
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

    protected override void OnDispose()
    {
        glob.asteroids.Remove(this);
    }

}
