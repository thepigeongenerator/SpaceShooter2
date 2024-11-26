using Microsoft.Xna.Framework;

namespace Core.Polygons;

public class Polygon2
{
    public Vector2 position;
    private readonly PolyNode[] vertices;

    public int VertexCount => vertices.Length;

    private int GetVertexIndex(int i) => i < 0 ? -i % VertexCount : i % VertexCount;

    // creates a two-dimensional polygon
    public Polygon2(params Vector2[] vertices)
    {
        position = Vector2.Zero;
        if (vertices.Length < 3) throw new System.Exception("a two-dimensional polygon must have at least 3 vertices");

        this.vertices = new PolyNode[vertices.Length];

        for (int i = 0; i < vertices.Length; i++)
        {
            ref PolyNode prev = ref this.vertices[GetVertexIndex(i - 1)];
            ref PolyNode curr = ref this.vertices[i];
            ref PolyNode next = ref this.vertices[GetVertexIndex(i + 1)];

            curr = new(vertices[i], prev, next);

            // link self to the previous and next nodes (if they exist)
            if (prev != null && prev.next == null) prev.next = curr;
            if (next != null && next.prev == null) next.prev = curr;
        }
    }

    public PolyNode GetVertex(int i) => vertices[GetVertexIndex(i)];
    public Vector2 GetVertexPos(int i) => GetVertex(i).position + position;
}
