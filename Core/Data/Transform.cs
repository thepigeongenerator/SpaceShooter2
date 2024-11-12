using Microsoft.Xna.Framework;

namespace Core.Data;

// make transform a reference type
public record Transform {
    public Vector2 position = Vector2.Zero;
    public Vector2 scale = Vector2.One;
    public float rotation = 0F;
}
