using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
