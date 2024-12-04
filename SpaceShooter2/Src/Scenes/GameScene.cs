using Core;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter2.Src.Data;

namespace SpaceShooter2.Src.Scenes;

internal class GameScene : Scene
{
    private readonly GlobalState glob;

    public GameScene(GlobalState glob)
    {
        this.glob = glob;
    }

    public override void Initialize()
    {
        _ = new Spawner(glob);
        _ = new UI(glob);
    }

    public override void LoadContent(ContentManager content)
    {
        glob.player = new Player(glob);

    }
    public override void Update() { }
    public override void Draw(SpriteBatch spriteBatch) { }
}
