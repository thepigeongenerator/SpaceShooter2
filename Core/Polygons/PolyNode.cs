using Microsoft.Xna.Framework;

namespace Core.Polygons;

// polynode is a reference type
public record PolyNode
{
    public readonly Vector2 position;
    internal PolyNode next;
    internal PolyNode prev;

    // allow public readonly access to these fields
    public PolyNode Next => next;
    public PolyNode Prev => prev;

    // only allow constructor access to this assembly
    internal PolyNode(Vector2 position, PolyNode prev, PolyNode next)
    {
        this.position = position;
        this.next = next;
        this.prev = prev;
    }
}
