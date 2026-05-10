using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

public class GraphBypass : Form
{
    static double k = 1 - GraphParams.n3 * 0.01 - GraphParams.n4 * 0.005 - 0.15;

    static double[,] dirMatrix = GraphWindow.BuildAdirMatrix(GraphParams.seed, GraphParams.vertexCount, k);
    List<Vertex> vertices;

    int startIndex = 0;
    Queue<Vertex> bfsQueue = new Queue<Vertex>();
    Queue<Vertex> dfsQueue = new Queue<Vertex>();

    Vertex current = null;

    public GraphBypass()
    {
        this.Text = "Lab 4";

        this.Location = new Point(100, 100);
        this.Size = new Size(840 * 2, 1000);

        this.BackColor = Color.WhiteSmoke;

        vertices = GraphConstructor.BuildVertexData(dirMatrix, ClientSize);

        UIConstructor.BuildButton("BFS STEP", this, new Point(10, 10), new Size(150, 30), 
        () =>
        {
           PerformBfsStep(vertices, ref startIndex); 
        });
    }
    protected override void OnPaint(PaintEventArgs e)
    {
        GraphConstructor.DrawGraph(vertices, e.Graphics);
    }

    private DataGridViewTopLeftHeaderCell 

    // that's the bad one, here because instead of using a pre-populated queue
    private void PerformBfsStep(List<Vertex> vertices, ref int startIndex)
    {
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
            bfsQueue.Enqueue(vertices[startIndex]);
            current = bfsQueue.Peek();
            current.state = VertexState.InQueue;
            startIndex++;
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