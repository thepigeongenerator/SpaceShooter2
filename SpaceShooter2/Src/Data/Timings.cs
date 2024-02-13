using System;

namespace SpaceShooter2.Src.Data;
/// <summary>
/// stores different times for time-based operations
/// </summary>
internal struct Timings {
    public TimeSpan bulletSpawnTime = TimeSpan.Zero;
    public TimeSpan astroidSpawnTime = TimeSpan.Zero;

    public Timings() {
    }
}
