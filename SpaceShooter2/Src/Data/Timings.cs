using System;

namespace SpaceShooter2.Src.Data;
/// <summary>
/// stores different times for time-based operations
/// </summary>
internal record Timings {
    public TimeSpan bulletSpawnTime = TimeSpan.Zero;
    public TimeSpan astroidSpawnTime = TimeSpan.Zero;
    public TimeSpan playerSwitchTextureTime = TimeSpan.Zero;

    public Timings() {
    }
}
