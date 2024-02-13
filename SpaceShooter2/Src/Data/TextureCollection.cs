using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace SpaceShooter2.Src.Data;
internal struct TextureCollection {
    public int currentTexture = 0;
    public List<Texture2D> textures = new();

    public TextureCollection() {
    }
}
