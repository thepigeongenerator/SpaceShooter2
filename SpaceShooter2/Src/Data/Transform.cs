using Microsoft.Xna.Framework;

namespace SpaceShooter2.Src.Data;
internal struct Transform {
    public Vector2 position = Vector2.Zero;
    public Vector2 scale = Vector2.One;
    public float rotation = 0F;

    public Transform() {
    }
}
