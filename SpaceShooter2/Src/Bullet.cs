using System.Collections.Generic;
using Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter2.Src.Data;
using SpaceShooter2.Src.Util;
using ThePigeonGenerator.MonoGame.Render;

namespace SpaceShooter2.Src;

internal class Bullet : GameObject
{
    private readonly List<Bullet> bullets = null;

    // creates a new bullet at the player's position
    public Bullet(GlobalState glob)
    {
        Vector2 playerPosition = glob.player.transform.position;

        transform.position = new Vector2(playerPosition.X, playerPosition.Y - 50);
        transform.scale = Vector2.One * 2F;
        bullets = glob.bullets;
    }

    // makes the bullet move upwards and destroy any astroids in it's way (destroys once no longer visible)
    public void Update(GlobalState glob)
    {
        int w = glob.textures.bullet.Width;
        int h = glob.textures.bullet.Height;
        List<Astroid> asteroids = glob.asteroids;
        transform.position.Y -= Const.BULLET_SPEED;

        if (transform.position.Y + (h * transform.scale.Y / 2F) < 0)
        {
            Dispose();
            return;
        }

        for (int i = 0; i < asteroids.Count; i++)
        {
            //if the bullet is inside of the radius of the astroid
            if (asteroids[i].OnAstroid(transform.position, glob))
            {
                //destroy the astroid if it isn't unbreakable
                if (asteroids[i].unbreakable == false)
                    asteroids[i].Dispose(); // destroy the astroid

                //destroy the bullet
                Dispose();
                return;
            }
        }

#if DEBUG
        if (glob.hitboxes)
            glob.pcl.SetLine((int)transform.position.X, (int)transform.position.Y, (int)transform.position.X, 0, (Color)Colour.Green);
#endif
    }

    // draws the bullet to the screen
    public void Draw(Textures textures, SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(
            textures.bullet,
            transform.position,
            null,
            Color.White,
            transform.rotation,
            new Vector2(
                textures.bullet.Width / 2,
                textures.bullet.Height / 2
                ),
            transform.scale,
            SpriteEffects.None,
            0.0F);
    }

    // cleans up the bullet references
    protected override void OnDispose()
    {
        bullets.Remove(this);
    }
}
