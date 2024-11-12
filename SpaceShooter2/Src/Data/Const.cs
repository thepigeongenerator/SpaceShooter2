#if DEBUG
#endif

namespace SpaceShooter2.Src.Data;

// contains the constants
internal static class Const
{
    public const int BULLET_SPAWN_DELAY_MS = 500;   // delay in milliseconds between bullet spawns
    public const int ASTROID_SPAWN_DELAY_MS = 500;  // delay in milliseconds between astroid spawns
    public const int SCREEN_WIDTH = 980;            // width of the window
    public const int SCREEN_HEIGHT = 640;           // height of the window
    public const int BULLET_SPEED = 10;             // px/frame of the bullet
    public const float MAX_ASTROID_SIZE = 2F;       // what the maximum size percentage of the astroid is allowed to be.
}
