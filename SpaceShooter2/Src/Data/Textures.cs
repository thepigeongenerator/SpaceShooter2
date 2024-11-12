using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooter2.Src.Data;
/// <summary>
/// stores the textures that might be used in the game
/// </summary>
internal record Textures // store the reference so you don't copy everything every time
{
    public Texture2D astroid = null;
    public Texture2D bullet = null;
    public TextureCollection player = new();

    public Textures()
    {
    }
}
