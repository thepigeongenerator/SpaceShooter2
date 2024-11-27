namespace SpaceShooter2.Src.Data;

// contains the constants
internal static class Const
{
    // game settings
    public const bool VSYNC = true;                 // whether frames should be rendered based on the monitor's refresh rate
    public const int UPDATES_PER_SECOND = 60;       // how many times per second the game should call Update
    public const int SCREEN_WIDTH = 980;            // width of the window
    public const int SCREEN_HEIGHT = 640;           // height of the window

    // gameplay settings
    public const int PLAYER_SPEED = 600;            // px/second of the player
    public const sbyte PLAYER_MAX_HEALTH = 10;      // the maximum amount of health that the player has
    public const int BULLET_SPEED = 600;            // px/second of the bullet
    public const int BULLET_SPAWN_DELAY_MS = 500;   // delay in milliseconds between bullet spawns
    public const int ASTEROID_SPEED = 60;           // px/second of the base asteroid speed
    public const int ASTEROID_ROT_SPEED = 60;       // deg/second of the base asteroid's rotation
    public const float MAX_ASTROID_SIZE = 2F;       // what the maximum size percentage of the astroid is allowed to be.
    public const int ASTROID_SPAWN_DELAY_MS = 500;  // delay in milliseconds between astroid spawns

    // texture names
    public const string TEXTURE_ASTEROID = "astroid";
    public const string TEXTURE_BULLET = "bullet";
    public const string TEXTURE_SPACESHIP_0 = "spaceship/spaceship_0";
    public const string TEXTURE_SPACESHIP_1 = "spaceship/spaceship_1";
    public const string TEXTURE_SPACESHIP_2 = "spaceship/spaceship_2";
    public const string TEXTURE_FONT = "font";

    // other
    public const string DATA_PATH = "./data.bin";
}
