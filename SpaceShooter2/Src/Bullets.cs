using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ShapeDrawer;
using SpaceShooter2.Src.Data;
using SpaceShooter2.Src.Util;
using System.Collections.Generic;

namespace SpaceShooter2.Src;
internal static class Bullets {
    const int SPEED = 10;

    /// <summary>creates a new bullet at the player's position</summary>
    public static Bullet CreateBullet(Vector2 playerPosition) {
        return new Bullet() {
            transform = {
                position = new Vector2(playerPosition.X, playerPosition.Y - 50),
                scale = Vector2.One * 2F,
            },
        };
    }

    /// <summary>makes the bullet move upwards and destroy any astroids in it's way (destroys once no longer visible)</summary>
    public static Bullet? UpdateBullet(Bullet bullet, Textures textures, List<Astroid> astroids) {
        bullet.transform.position.Y -= SPEED;

        if (bullet.transform.position.Y + (textures.bullet.Height * bullet.transform.scale.Y / 2F) < 0) {
            return null; //there is no more bullet to be updated / must be destroid
        }

        for (int i = 0; i < astroids.Count; i++) {
            //if the bullet is inside of the radius of the astroid
            if (VectorDetection.InCircle(astroids[i].transform.position - bullet.transform.position, textures.astroid.Width * astroids[i].transform.scale.X)) {
                //destroy the astroid if it isn't unbreakable
                if (astroids[i].unbreakable == false) {
                    astroids.RemoveAt(i);
                }

                //destroy the bullet
                return null;
            }
        }

        return bullet;
    }

    /// <summary>draws the bullet to the screen</summary>
    public static void DrawBullet(Bullet bullet, Textures textures, SpriteBatch spriteBatch) {
        spriteBatch.Draw(
            textures.bullet,
            bullet.transform.position,
            null,
            Color.White,
            bullet.transform.rotation,
            new Vector2(
                textures.bullet.Width / 2,
                textures.bullet.Height / 2
                ),
            bullet.transform.scale,
            SpriteEffects.None,
            0f);

#if VISUALIZE_DEBUG
        DrawShape.Line(spriteBatch, bullet.transform.position - (new Vector2(0,1) * 15), bullet.transform.position + (new Vector2(0,1) * 15), new Color(0xFF0000FF));
        DrawShape.Line(spriteBatch, bullet.transform.position - (new Vector2(1,0) * 15), bullet.transform.position + (new Vector2(1,0) * 15), new Color(0xFF0000FF));
#endif
    }
}
