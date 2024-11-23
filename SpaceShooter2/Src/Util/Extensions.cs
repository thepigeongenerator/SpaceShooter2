using Core.Polygons;
using ThePigeonGenerator.MonoGame.Render;

namespace SpaceShooter2.Src.Util;

public static class Extensions
{
    public static void SetPolygon(this PixelControlLayer pcl, Polygon2 polygon, Color color)
    {
        foreach (PolyNode vertex in polygon.vertices)
        {
            pcl.SetLine(
                (int)vertex.position.X,
                (int)vertex.position.Y,
                (int)vertex.Next.position.X,
                (int)vertex.Next.position.Y,
                color);
        }
    }

    public static void SetPolygon(this PixelControlLayer pcl, Polygon2 polygon, Vector2 position, Color color)
    {
        foreach (PolyNode vertex in polygon.vertices)
        {
            pcl.SetLine(
                (int)(vertex.position.X + position.X),
                (int)(vertex.position.Y + position.Y),
                (int)(vertex.Next.position.X + position.X),
                (int)(vertex.Next.position.Y + position.Y),
                color);
        }
    }
}
