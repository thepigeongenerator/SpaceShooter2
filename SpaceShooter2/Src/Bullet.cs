using System.Collections.Generic;
using Core;
using Core.Util;
using SpaceShooter2.Src.Data;
using SpaceShooter2.Src.Util;
using ThePigeonGenerator.MonoGame.Render;

namespace SpaceShooter2.Src;

internal class Bullet : TexturedGameObject, IUpdate
{
    private readonly List<Bullet> bullets = null;
    private readonly GlobalState glob;

    // creates a new bullet at the player's position
    public Bullet(GlobalState glob) : base(glob.assets.bullet)
    {
        Vector2 playerPosition = glob.player.transform.position;

        transform.position = new Vector2(playerPosition.X, playerPosition.Y - 50);
        transform.scale = Vector2.One * 2.0F;
        transform.origin = Vector2.One / 2.0F;
        bullets = glob.bullets;
        this.glob = glob;
    }

    // makes the bullet move upwards and destroy any asteroids in it's way (destroys once no longer visible)
    public void Update()
    {
        // dispose ourselves if the game has been lost
        if (glob.lose)
        {
            Dispose();
            return;
        }

        List<Astroid> asteroids = glob.asteroids;
        transform.position.Y -= Const.BULLET_SPEED * glob.gameTime.DeltaTime();

        if (transform.position.Y + (Height / 2F) < 0)
        {
            Dispose();
            return;
        }

        for (int i = 0; i < asteroids.Count; i++)
        {
            //if the bullet is inside of the radius of the astroid
            if (asteroids[i].OnAstroid(transform.position))
            {
                //destroy the astroid if it isn't unbreakable
                if (asteroids[i].unbreakable == false)
                {
                    glob.score++;

                    if (glob.score > glob.highscore)
                        glob.highscore = glob.score;

                    glob.assets.destroyAsteroid.Play();
                    asteroids[i].Dispose(); // destroy the astroid
                }

                //destroy the bullet
                Dispose();
                return;
            }
        }

        if (glob.hitboxes)
            glob.pcl.SetLine((int)transform.position.X, (int)transform.position.Y, (int)transform.position.X, 0, (Color)Colour.Blue);
    }

    // cleans up the bullet references
    protected override void OnDispose()
    {
        bullets.Remove(this);
    }
}
