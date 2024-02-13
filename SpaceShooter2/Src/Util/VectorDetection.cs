using Microsoft.Xna.Framework;
using System;
using System.Runtime.CompilerServices;

namespace SpaceShooter2.Src.Util;
internal static class VectorDetection
{
    /// <summary>gets whether a point is within a box of (<paramref name="bWidth"/>, <paramref name="bHeight"/>)</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool InBox(Vector2 point, float bWidth, float bHeight)
    {
        return
            MathF.Abs(point.X) <= bWidth &&
            MathF.Abs(point.Y) <= bHeight;
    }

    /// <summary>gets whether a point is within a circle where r = <paramref name="radius"/></summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool InCircle(Vector2 point, float radius)
    {
        //for efficiency purposes; first check whether it is within the bounding box of the circle
        if (InBox(point, radius, radius) == false)
            return false;

        //get a point on the circle of r, through normalizing the vector and multiplying it by r
        Vector2 circlePoint = point;
        circlePoint.Normalize();
        circlePoint *= radius;

        return
            MathF.Abs(point.X) <= MathF.Abs(circlePoint.X) &&
            MathF.Abs(point.Y) <= MathF.Abs(circlePoint.Y);
    }
}
