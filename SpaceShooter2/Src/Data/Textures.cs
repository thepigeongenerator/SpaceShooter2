using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooter2.Src.Data;
/// <summary>
/// stores the textures that might be used in the game
/// </summary>
internal struct Textures {
    public Texture2D astroid = null;
    public Texture2D bullet = null;
    public TextureCollection player = new();

    public Textures() {
    }
}
