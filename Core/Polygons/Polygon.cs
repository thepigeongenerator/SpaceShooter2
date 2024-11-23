using System.Diagnostics;
using Core.Util;
using Microsoft.Xna.Framework;

namespace Core.Polygons;

public class Polygon2
{
    public readonly PolyNode[] vertices;

    // creates a two-dimensional polygon
    public Polygon2(params Vector2[] vertices)
    {
        if (vertices.Length < 3) throw new System.Exception("a two-dimensional polygon must have at least 3 vertices");

        this.vertices = new PolyNode[vertices.Length];

        for (int i = 0; i < vertices.Length; i++)
        {
            ref PolyNode prev = ref this.vertices[i == 0 ? vertices.Length - 1 : i - 1];
            ref PolyNode curr = ref this.vertices[i];
            ref PolyNode next = ref this.vertices[i == vertices.Length - 1 ? 0 : i + 1];

            curr = new(vertices[i], prev, next);

            // link self to the previous and next nodes (if they exist)
            if (prev != null && prev.next == null) prev.next = curr;
            if (next != null && next.prev == null) next.prev = curr;
        }
    }
}
