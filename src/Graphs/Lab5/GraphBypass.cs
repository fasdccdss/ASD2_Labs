using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

public class GraphBypass : Form
{
    static double k = 1 - GraphParams.n3 * 0.01 - GraphParams.n4 * 0.005 - 0.15;

    static double[,] dirMatrix = GraphWindow.BuildAdirMatrix(GraphParams.seed, GraphParams.vertexCount, k);
    List<Vertex> vertices;

    static int startIndex = 0;
    int runtimeIndex = startIndex;

    Queue<Vertex> bfsQueue = new Queue<Vertex>();
    Queue<Vertex> dfsQueue = new Queue<Vertex>();

    Vertex current = null;

    public GraphBypass()
    {
        this.Text = "Lab 4";

        this.Location = new Point(100, 100);
        this.Size = new Size(840 * 2, 1000);

        this.BackColor = Color.WhiteSmoke;

        vertices = GraphConstructor.BuildVertexData(dirMatrix);

        UIConstructor.BuildButton("BFS STEP", this, new Point(10, 10), new Size(150, 30), 
        () =>
        {
           PerformBfsStep(vertices, ref runtimeIndex); 
        });
    }
    protected override void OnPaint(PaintEventArgs e)
    {
        GraphConstructor.DrawGraph(vertices, e.Graphics, ClientSize, true, 30, 60);
    }

    /* REFRESH */
    private void RefreshGraph()
    {
        for (int x = 0; x < vertices.Count; x++)
        {
            vertices[x].state = VertexState.Unvisited;

            bfsQueue.Enqueue(vertices[x]);
            dfsQueue.Enqueue(vertices[x]);
        }

        current = null;
        runtimeIndex = 0;
    }

    // that's the bad one, here because instead of using a pre-populated queue
    private void PerformBfsStep(List<Vertex> vertices, ref int runtimeIndex)
    {
        if (runtimeIndex > vertices.Count)
            runtimeIndex = startIndex;

        if (bfsQueue.Count != 0)
        {
            for (int x = 0; x < current.next.Count; x++)
            {
                if (current.next[x].state != VertexState.Visited)
                {
                    bfsQueue.Enqueue(current.next[x]);
                    current.next[x].state = VertexState.InQueue;
                }
            }

            current.state = VertexState.Visited;
            bfsQueue.Dequeue(); // Dequeue current
            current = bfsQueue.Peek();
        }
        else
        {
            bfsQueue.Enqueue(vertices[runtimeIndex]);
            current = bfsQueue.Peek();
            current.state = VertexState.InQueue;
            runtimeIndex++;
            // bfsQueue.Peek().state = VertexState.InQueue;
            /*
            if (vertices[startIndex].next.Count == 0)
            {
                bfsQueue.Peek().state = VertexState.Visited;
                bfsQueue.Dequeue();
                startIndex++;
            }
            */
        }

        Invalidate();
    }
    private void PerformDfsStep()
    {
        
    }
}