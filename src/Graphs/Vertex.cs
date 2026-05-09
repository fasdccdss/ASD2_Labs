using System.Collections.Generic;
using System.Drawing;

public class Vertex
{
    public int index;
    public List<Vertex> previous; // stores vertices that point to this one
    public List<Vertex> next; // stores vertices that this one is pointing to

    public Point center;
    public int radius;

    public Vertex() {}
    public Vertex(int index)
    {
        this.index = index;
    }
    public Vertex(int index, int radius)
    {
        this.index = index;
        this.radius = radius;
    }
    public Vertex(int centerX, int centerY, int radius)
    {
        center.X = centerX;
        center.Y = centerY;
        this.radius = radius;
    }

    public bool Intersects(Point p)
    {
        int dx = p.X - center.X, dy = p.Y - center.Y;
        return dx * dx + dy * dy <= radius * radius;
    }
}
