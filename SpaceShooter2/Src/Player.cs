using Core;
using Core.Util;
using Microsoft.Xna.Framework.Input;
using SpaceShooter2.Src.Data;
using SpaceShooter2.Src.Util;

namespace SpaceShooter2.Src;

internal class Player : TexturedGameObject, IUpdate
{
    private readonly GlobalState glob;

    public byte health = 0;

    // creates a new player at the centre of the screen
    public Player(GlobalState glob) : base(glob.textures.player.textures[0])
    {
        health = 10;
        transform.position = new Vector2(Const.SCREEN_WIDTH / 2);
        transform.scale = Vector2.One * 4.5F;
        transform.origin = Vector2.One / 2.0F;

        //set the correct Y position for the player
        transform.position.Y = Const.SCREEN_HEIGHT - glob.textures.player.textures[0].Height * transform.scale.Y / 2;
        this.glob = glob;
    }

    // allows the user to use input to update the player's position
    public void Update()
    {
        //update player's position based on input
        if (Keyboard.GetState().IsKeyDown(Keys.A))
            transform.position.X -= Const.PLAYER_SPEED * glob.gameTime.DeltaTime();
        if (Keyboard.GetState().IsKeyDown(Keys.D))
            transform.position.X += Const.PLAYER_SPEED * glob.gameTime.DeltaTime();

        //flip the player's position to the other side of the screen if they go "out of bounds"
        if (transform.position.X > Const.SCREEN_WIDTH)
            transform.position.X %= Const.SCREEN_WIDTH;
        else if (transform.position.X < 0)
            transform.position.X += Const.SCREEN_WIDTH;

        //update the texture index if the timer ran out
        TimeUtils.RunWhenTimer(glob.gameTime, ref glob.timings.playerSwitchTextureTime, 100, () =>
        {
            glob.textures.player.currentTexture++; //increase the index of the active texture
            if (glob.textures.player.currentTexture >= glob.textures.player.textures.Count)
            {
                glob.textures.player.currentTexture = 0;
            }
        });
    }
}
