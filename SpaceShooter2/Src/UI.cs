using Core;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter2.Src.Data;
using SpaceShooter2.Src.Util;

namespace SpaceShooter2.Src;

internal class UI : GameObject, IDraw, IUpdate
{
    private readonly GlobalState glob = null;

    public UI(GlobalState glob) : base()
    {
        this.glob = glob;
    }

    public void Update()
    {
        // if you lost the game
        if (glob.lose == true)
        {
            // draw a bunch of red dots
            for (int i = 0; i < glob.pcl.buffer.Length; i++)
                if (i % 16 == 0)
                    glob.pcl.buffer[i] = Color.Red;
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        string score = glob.score.ToString() + (glob.score == 0 ? "" : "0"); // add a 0 at the end if the number isn't already 0
        string highscore = glob.highscore.ToString() + (glob.highscore == 0 ? "" : "0");
        Vector2 centre = Vector2.One / 2;
        Vector2 topCentre = new(0.5F, 0);
        Vector2 topRight = new(1, 0);

        if (glob.lose)
        {
            spriteBatch.DrawString(glob.assets.font, "YOU LOST", new Vector2(Const.SCREEN_CENTRE_X, Const.SCREEN_CENTRE_Y), Color.Red, centre, 1.0F, 1.0F);
            spriteBatch.DrawString(glob.assets.font, $"Score: {score}\nHigh Score: {highscore}", new Vector2(Const.SCREEN_CENTRE_X, 10), Color.White, topCentre, 0.5F, 1.0F);
            spriteBatch.DrawString(glob.assets.font, "press [enter] to exit", new Vector2(Const.SCREEN_CENTRE_X, Const.SCREEN_CENTRE_Y + 40), Color.Red, centre, 0.5F, 1.0F);
            return;
        }

        spriteBatch.DrawString(glob.assets.font, $"Health: {glob.player.Health}/{Const.PLAYER_MAX_HEALTH}", new Vector2(10, 10), Color.Red, Vector2.Zero, 0.4F, 1);
        spriteBatch.DrawString(glob.assets.font, $"Score: {score}\nHigh Score: {highscore}", new Vector2(Const.SCREEN_WIDTH - 10, 10), Color.Blue, topRight, 0.4F, 1);
    }
}
