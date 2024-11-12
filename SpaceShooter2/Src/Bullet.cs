using System.Collections.Generic;
using Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter2.Src.Data;
using SpaceShooter2.Src.Util;

namespace SpaceShooter2.Src;

internal class Bullet : GameObject
{
    private const int SPEED = 10;
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
    public void Update(Textures textures, List<Astroid> asteroids)
    {
        transform.position.Y -= SPEED;

        if (transform.position.Y + (textures.bullet.Height * transform.scale.Y / 2F) < 0)
        {
            Dispose();
            return;
        }

        for (int i = 0; i < asteroids.Count; i++)
        {
            //if the bullet is inside of the radius of the astroid
            if (VectorDetection.InCircle(asteroids[i].transform.position - transform.position, textures.astroid.Width * asteroids[i].transform.scale.X))
            {
                //destroy the astroid if it isn't unbreakable
                if (asteroids[i].unbreakable == false)
                    asteroids[i].Dispose(); // destroy the astroid

                //destroy the bullet
                Dispose();
                return;
            }
        }
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
