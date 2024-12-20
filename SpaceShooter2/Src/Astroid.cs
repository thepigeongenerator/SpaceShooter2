using System;
using SpaceShooter2.Src.Data;
using Core;
using Core.Util;
using ThePigeonGenerator.MonoGame.Render;
using SpaceShooter2.Src.Util;

namespace SpaceShooter2.Src;

internal class Astroid : TexturedGameObject, IUpdate
{
    // game fields
    public readonly GlobalState glob = null;

    // asteroid fields
    public readonly bool unbreakable;   // whether the asteroid can be destroyed by a bullet
    public readonly float radius;       // the radius of the asteroid, includes scale

    // multiplier from 0..1, larger asteroids are lower and smaller asteroids are higher.
    public float Multiplier => (-1 * transform.scale.X) + Const.MAX_ASTROID_SIZE;

    // creates a new astroid with a random position at the top of the screen.
    // there is also a random chance this astroid will be unbreakable
    public Astroid(GlobalState glob) : base(glob.assets.astroid)
    {
        transform.origin = Vector2.One / 2.0F;  // put the origin in the centre
        transform.rotation = glob.random.NextSingle() * MathF.Tau; //random rotation from 0..TAU
        float size = glob.random.NextSingle();
        transform.scale = Vector2.One * ((size + 1F) * Const.MAX_ASTROID_SIZE / 2F); //make the scaling random
        /* MAX_SIZE is divided by 2 because NextSingle produces a value between 0..1.
           We add 1 to make sure we don't get any zero issues. Making the range 1..2.
           Multiplying by MAX_SIZE then would equate to the Actual max size to be 2 * MAX_SIZE. */

        // put the astroid offscreen so the player doesn't see it spawning in
        transform.position.Y = -1 * glob.assets.astroid.Width * transform.scale.X;

        radius = (glob.assets.astroid.Width / 2.0F * transform.scale.X) + (glob.assets.bullet.Width / 2.0F); // add the half of the bullet's texture's width, to compensate for

        // have a random chance that the astroid is unbreakable (the player needs to dodge these asteroids)
        if (glob.random.NextSingle() < 0.10F) // 10% chance
        {
            unbreakable = true;
            textureTint = new Color(0xFF505050);    // tint the texture if the asteroid is unbreakable
            transform.position.X = glob.player.transform.position.X;
        }
        else
        {
            unbreakable = false;
            byte val = (byte)(0xAA * Multiplier + 0x55);
            textureTint = new Color(val, val, val);    // tint the texture if the asteroid is unbreakable
            transform.position.X = glob.random.Next(0, Const.SCREEN_WIDTH);
        }

        this.glob = glob;
    }

    public bool OnAstroid(Vector2 pos)
    {
        return IntersectUtils.CircleContainsPoint(transform.position.X, transform.position.Y, radius, pos.X, pos.Y);
    }

    // makes the astroid move down and damages the player if it interacts
    public void Update()
    {
        // dispose ourselves if the game has been lost
        if (glob.lose)
        {
            Dispose();
            return;
        }

        // move the astroid down (bigger asteroids move slower)
        transform.position.Y += Multiplier * Const.ASTEROID_SPEED * glob.gameTime.DeltaTime();

        // if the astroid fell off the screen; destroy the astroid
        if (transform.position.Y - radius > Const.SCREEN_HEIGHT)
        {
            Dispose();
            return;
        }

        // if we intersect the player, destroy ourselves
        if (glob.player.hitbox.PolygonIntersectsCircle(transform.position.X, transform.position.Y, radius))
        {
            glob.player.Damage();
            Dispose();
            return;
        }

        // rotate the asteroid clockwise (bigger asteroids rotate slower)
        transform.rotation += MathF.Tau / 360 * -1 * (transform.scale.X - Const.MAX_ASTROID_SIZE) * Const.ASTEROID_ROT_SPEED * glob.gameTime.DeltaTime();
        transform.rotation %= MathF.Tau; // if a rotation of 360° has been achieved, wrap it back around to 0... but with radians

        if (glob.hitboxes)
            glob.pcl.SetCircle((int)transform.position.X, (int)transform.position.Y, (int)radius, (Color)Colour.Green);
    }

    protected override void OnDispose()
    {
        glob.asteroids.Remove(this);
        base.OnDispose();
    }
}
