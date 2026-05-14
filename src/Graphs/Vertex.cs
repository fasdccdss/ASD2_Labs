using System.Collections.Generic;
using System.Drawing;

public enum VertexState { Unvisited, InQueue, Visited }

public class Vertex
{
    public VertexState state;

    public Color FillColor() => state switch
    {
        VertexState.InQueue => Color.Gray,
        VertexState.Visited => Color.Black,
        _ => Color.White
    };
    public Color FontColor() => state switch
    {
        VertexState.InQueue => Color.Black,
        VertexState.Visited => Color.White,
        _ => Color.Black,
    };

    public int index;
    public List<Vertex> previous = new List<Vertex>(); // stores vertices that point to this one
    public List<Vertex> next = new List<Vertex>(); // stores vertices that this one is pointing to

    // in lab 6 it is best for collections "previous" and "next" to be Dictionaries,
    // as we would be able to store both the next Vertex and its Weight in relation to *this* vertex
    public Dictionary<Vertex, double> previousV = new Dictionary<Vertex, double>();
    public Dictionary<Vertex, double> nextV = new Dictionary<Vertex, double>();

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
