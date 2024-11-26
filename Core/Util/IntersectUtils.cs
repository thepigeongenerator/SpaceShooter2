using System;
using System.Runtime.CompilerServices;
using Core.Polygons;

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

    // checks whether a point is within with a rectangle
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool RectContainsPoint(float x1, float y1, float w1, float h1, float x2, float y2) => RectIntersectsRect(x1, y1, w1, h1, x2, y2, 0, 0);

    // checks whether a point is within a circle of radius r
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool CircleContainsPoint(float cx, float cy, float r, float x1, float y1)
    {
        // make the coordinates relative to the circle's coordinates (circle's pos becomes (0,0))
        x1 -= cx;
        y1 -= cy;

        // x^2 + y^2 = r^2 (circle equation)
        // so if x^2 + y^2 is less than r^2, the point must be inside the circle, etc.
        return (x1 * x1) + (y1 * y1) < r * r;
    }

    // checks whether a line inter
    public static bool LineIntersectsCircle(float cx, float cy, float r, float ax, float ay, float bx, float by)
    {
        // make the coordinates relative to the circle's coordinates (circle's pos becomes (0,0))
        ax -= cx;
        ay -= cy;
        bx -= cx;
        by -= cy;
        cx = 0;
        cy = 0;

        // if either A xor B are inside the circle, the line must intersect the circle
        if (CircleContainsPoint(cx, cy, r, ax, ay) ^ CircleContainsPoint(cx, cy, r, bx, by))
            return true;

        // calculate the quadratic coefficients
        float a, b, c;
        {
            float lx = bx - ax;                     // get the length on the X axis
            float ly = by - ay;                     // same, but for Y
            a = (lx * lx) + (ly * ly);              // get the squared length of the line segment (direction and magnitude)
            b = ((ax * lx) + (ay * ly)) * 2.0F;     // get how the line segment relates to (0,0)
            c = (ax * ax) + (ay * ay) - (r * r);    // pythagoras circle equation, shifted to A
        }


        // calculate the discriminant
        float d;
        {
            // gets how many intersections the line has with the circle
            float d2 = (b * b) - (4.0F * a * c);

            // if d2 > 0, the line crosses at 2 points, d2 = 0 when it only crosses once and d2 < 0 when it doesn't cross
            // d2 cannot be equal to 0, as it'd need to have one of the points within the circle for that, and we don't handle this case
            if (d2 <= float.Epsilon) // using epsilon instead of 0 to prevent floating-point imprecision errors
                return false;

            // get the square root of the discriminant
            d = MathF.Sqrt(d2);
        }

        // because d2 > 0, there are only two possible values for t
        // which tell us where the intersections occur according to (x,y) = A + t(B - A) (which is line AB plotted along t=0..1)
        float t1 = (-b + d) / (2 * a);
        float t2 = (-b - d) / (2 * a);

        // so, if one of the t's is in between 0..1, the point occurs on the line, thus return true
        if ((0 < t1 && t1 < 1) || (0 < t2 && t2 < 1))
            return true;

        return false;
    }

    // checks whether a polygon intersects with a circle of radius r
    public static bool PolygonIntersectsCircle(this Polygon2 polygon, float cx, float cy, float r)
    {
        // loops through the polygon's vertices, and check whether the line intersects with our circle
        for (int i = 0; i < polygon.VertexCount; i++)
        {
            if (LineIntersectsCircle(
                cx, cy, r,
                polygon.GetVertexPos(i).X,
                polygon.GetVertexPos(i).Y,
                polygon.GetVertexPos(i + 1).X,
                polygon.GetVertexPos(i + 1).Y))
            {
                return true; // return true if we intersect; we don't need to check further
            }
        }

        return false;
    }
}
