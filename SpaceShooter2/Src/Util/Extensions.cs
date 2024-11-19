using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;

namespace SpaceShooter2.Src.Util;

internal static class Extensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float DeltaTime(this GameTime gameTime) => (float)gameTime.ElapsedGameTime.TotalSeconds;
}
