using System;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;

namespace Core.Util;
public static class IntersectUtils
{
    // checks whether two rectangles intersect with one another
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool RectIntersectsRect(float x1, float y1, float w1, float h1, float x2, float y2, float w2, float h2)
    {
        return
            x1 < x2 + w2 && x1 + w1 > x2 &&   // if x1 is less than x2 + w2 and x1 + w1 is greater than x2, we're intersecting
            y1 < y2 + h2 && y1 + h1 > y2;     // same here, but then horizontally
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool RectIntersectsRect(Vector2 p1, float w1, float h1, Vector2 p2, float w2, float h2) => RectIntersectsRect(p1.X, p1.Y, h1, w1, p2.X, p2.Y, w2, h2);

    // checks whether a point intersects with a rectangle
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool PointIntersectsRect(float x1, float y1, float w1, float h1, float x2, float y2) => RectIntersectsRect(x1, y1, w1, h1, x2, y2, 0, 0);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool PointIntersectsRect(Vector2 p1, float w1, float h1, Vector2 p2) => RectIntersectsRect(p1.X, p1.Y, w1, h1, p2.X, p2.Y, p2.X, p2.Y);

    // checks whether a point intersects with a circle of radius r
    public static bool PointIntersectsCircle(float cx, float cy, float r, float x1, float y1)
    {
        {
            float d = r * 2.0F; // get diameter
            // don't bother if the box around the circle doesn't contain the point
            if (PointIntersectsRect(cx - r, cy - r, d, d, x1, y1) == false)
                return false;
        }

        // subtract the current circle position from the result to get coordinates relative to the circle
        x1 = MathF.Abs(x1 - cx);
        y1 = MathF.Abs(y1 - cy);

        // normalize the point and multiply it by the radius to make it fall on the circle
        float x, y;
        {
            float num = MathF.Sqrt(x1 * x1 + y1 * y1);
            if (num == 0) return true; // point is at centre
            num = 1f / num * r;
            x = x1 * num;
            y = y1 * num;
        }

        // if the normalized point is greater than the point that falls on the circle, it is inside the circle
        return x1 < x && y1 < y;
    }

    [Obsolete("under development", true)]
    public static bool LineIntersectsCircle(float cx, float cy, float r, float x1, float y1, float x2, float y2)
    {
        float dx = x2 - x1;
        float dy = y2 - y1;

        // somehow do this crap

        return false;
    }

    // checks whether a rectangle intersects with a circle of radius r
    [Obsolete("under development", true)]
    public static bool IntersectsCircle(float cx, float cy, float r, float x1, float y1, float w1, float h1)
    {
        // don't bother if the box around the circle doesn't intersect with the rectangle
        {
            float d = r * 2.0F; // get diameter
            if (RectIntersectsRect(cx - r, cy - r, d, d, x1, y1, w1, h1) == false)
                return false;
        }

        // if the centre of the circle lies in the rectangle, return true
        if (PointIntersectsRect(x1, y1, w1, h1, cx, cy))
            return true;

        float x2 = x1 + w1;
        float y2 = y1 + h1;

        // A---B
        // C---D
        // there is a slight issue, where if the rect is *entirely* contained within the circle (without the centre being contained within it), the value will equate to FALSE.
        return
            LineIntersectsCircle(cx, cy, r, x1, y1, x2, y1) ||  // AB
            LineIntersectsCircle(cx, cy, r, x2, y1, x2, y2) ||  // BC
            LineIntersectsCircle(cx, cy, r, x2, y2, x1, y2) ||  // CD
            LineIntersectsCircle(cx, cy, r, x1, y2, x1, y1);    // DA
    }
}
