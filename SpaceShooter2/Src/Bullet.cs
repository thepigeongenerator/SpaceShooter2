using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter2.Src.Data;
using SpaceShooter2.Src.Util;

namespace SpaceShooter2.Src;

internal class Bullet
{
    const int SPEED = 10;

    readonly public Transform transform;


    // creates a new bullet at the player's position
    public Bullet(Vector2 playerPosition)
    {
        transform = new Transform
        {
            position = new Vector2(playerPosition.X, playerPosition.Y - 50),
            scale = Vector2.One * 2F,
        };
    }

    // makes the bullet move upwards and destroy any astroids in it's way (destroys once no longer visible)
    public bool Update(Textures textures, List<Astroid> asteroids)
    {
        transform.position.Y -= SPEED;

        if (transform.position.Y + (textures.bullet.Height * transform.scale.Y / 2F) < 0)
            return false;

        for (int i = 0; i < asteroids.Count; i++)
        {
            //if the bullet is inside of the radius of the astroid
            if (VectorDetection.InCircle(asteroids[i].transform.position - transform.position, textures.astroid.Width * asteroids[i].transform.scale.X))
            {
                //destroy the astroid if it isn't unbreakable
                if (asteroids[i].unbreakable == false)
                    asteroids.RemoveAt(i);

                //destroy the bullet
                return false;
            }
        }

        return true;
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
}
