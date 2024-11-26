using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Core;

public class TexturedGameObject : GameObject, IDraw
{
    protected Texture2D texture = null;
    protected SpriteEffects spriteEffects;
    protected Color textureTint;
    protected float textureDepth;
    protected bool runDraw;

    // constructor
    public TexturedGameObject(
        Texture2D texture,
        SpriteEffects spriteEffects = SpriteEffects.None,
        Color? textureTint = null,
        float textureDepth = 0.0F)
    {
        this.texture = texture;
        this.spriteEffects = spriteEffects;
        this.textureTint = textureTint ?? Color.White;
        this.textureDepth = textureDepth;
        this.runDraw = true;
    }

    // property shorthands
    public float Width => UnscaledWidth * transform.scale.X;
    public float Height => UnscaledHeight * transform.scale.Y;
    public float UnscaledWidth => texture.Width;
    public float UnscaledHeight => texture.Height;

    // called when the sprite is drawn
    public virtual void Draw(SpriteBatch spriteBatch)
    {
        if (runDraw == false) return;

        // draw the texture
        spriteBatch.Draw(
            texture,
            transform.position,
            null,
            textureTint,
            transform.rotation,
            new Vector2(
                texture.Width * transform.origin.X,
                texture.Height * transform.origin.Y),
            transform.scale,
            spriteEffects,
            textureDepth
        );
    }
}
