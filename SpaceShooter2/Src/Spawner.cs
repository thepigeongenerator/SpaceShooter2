using Core;
using SpaceShooter2.Src.Data;
using SpaceShooter2.Src.Util;

namespace SpaceShooter2.Src;

internal class Spawner : GameObject, IUpdate
{
    private readonly GlobalState glob;

    public Spawner(GlobalState glob)
    {
        this.glob = glob;
    }

    public void Update()
    {
        // don't do anything if the game is stopped
        if (glob.lose == true)
        {
            Dispose();
            return;
        }

        // creates new bullets with a certain delay
        TimeUtils.RunWhenTimer(glob.gameTime, ref glob.timings.bulletSpawnTime, Const.BULLET_SPAWN_DELAY_MS, () =>
            glob.bullets.Add(new Bullet(glob)));

        // creates new asteroids with a certain delay
        TimeUtils.RunWhenTimer(glob.gameTime, ref glob.timings.astroidSpawnTime, Const.ASTROID_SPAWN_DELAY_MS, () =>
            glob.asteroids.Add(new Astroid(glob)));
    }
}
