using System;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;

namespace SpaceShooter2.Src.Util;

internal static class TimeUtils
{
    // executes an action based on a certain delay
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void RunWhenTimer(GameTime gameTime, ref TimeSpan timing, int msDelay, Action execute)
    {
        if ((gameTime.TotalGameTime - timing).TotalMilliseconds > msDelay)
        {
            execute.Invoke();
            timing = gameTime.TotalGameTime;
        }
    }
}
