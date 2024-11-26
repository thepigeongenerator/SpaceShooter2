using Core.Polygons;
using ThePigeonGenerator.MonoGame.Render;

namespace SpaceShooter2.Src.Util;

public static class Extensions
{
    public static void SetPolygon(this PixelControlLayer pcl, Polygon2 polygon, Color color)
    {
        for (int i = 0; i < polygon.VertexCount; i++)
        {
            pcl.SetLine(
                (int)polygon.GetVertexPos(i).X,
                (int)polygon.GetVertexPos(i).Y,
                (int)polygon.GetVertexPos(i + 1).X,
                (int)polygon.GetVertexPos(i + 1).Y,
                color);
        }
    }
}
