using Microsoft.Xna.Framework;

namespace Core.Polygons;

public class Triangle : Polygon2
{
    public Triangle(Vector2 a, Vector2 b, Vector2 c) : base(a, b, c)
    {
        // calls base constructor
    }
}

public class Rectangle : Polygon2
{
    public Rectangle(Vector2 a, Vector2 b, Vector2 c, Vector2 d) : base(a, b, c, d)
    {
        // calls base constructor
    }
}
