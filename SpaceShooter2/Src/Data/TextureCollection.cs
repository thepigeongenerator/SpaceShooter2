using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace SpaceShooter2.Src.Data;
internal record TextureCollection // store the reference, so you can modify the values more easily
{
    public int currentTexture = 0;
    public List<Texture2D> textures = new();
}
