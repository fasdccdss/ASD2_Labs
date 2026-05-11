using System;
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

    List<Vertex> visited = new List<Vertex>();
    Queue<Vertex> bfsQueue = new Queue<Vertex>();
    Stack<Vertex> dfsStack = new Stack<Vertex>();

    bool bfsMode;

    Vertex current = null;

    public GraphBypass()
    {
        this.Text = "Lab 4";

        this.Location = new Point(100, 100);
        this.Size = new Size(840 * 2, 1000);

        this.BackColor = Color.WhiteSmoke;

        vertices = GraphConstructor.BuildVertexData(dirMatrix);

        RefreshGraph();

        Button bfsStepBtn = UIConstructor.BuildButton("BFS STEP", this, new Point(10, 10), new Size(150, 30), 
        () =>
        {
           PerformBfsStep(vertices, ref runtimeIndex); 
        });

        Button dfsStepBtn = UIConstructor.BuildButton("DFS STEP", this, new Point(10, 60), new Size(150, 30),
        () =>
        {
            PerformDfsStep(vertices, ref runtimeIndex);
        });

        UIConstructor.BuildButton("RE-FRESH SEARCH", this, new Point(160, 10), new Size(150, 30),
        () =>
        {
            RefreshGraph();
        });
    }
    protected override void OnPaint(PaintEventArgs e)
    {
        GraphConstructor.DrawGraph(vertices, e.Graphics, ClientSize, true, 50, 90);
    }

    /* REFRESH */
    private void RefreshGraph()
    {
        bfsQueue.Clear();
        dfsStack.Clear();

        for (int x = 0; x < vertices.Count; x++)
        {
            vertices[x].state = VertexState.Unvisited;

            // bfsQueue.Enqueue(vertices[x]);
            // dfsQueue.Enqueue(vertices[x]);
        }

        current = null;
        runtimeIndex = 0;
        Invalidate();
    }
    /* SEARCHES */
    // we start with a queue full of white vertices,
    // at the start we will take a vertex at (runtimeIndex) and we will
    // mark it for the enque and move it to the beginning
    // we want to dequeue a visited vertex
    private void PerformBfsStep(List<Vertex> vertices, ref int index)
    {
        if (index > vertices.Count)
        {
            Console.WriteLine("can't perform BFS further, refresh");
            return;
        }

        if (bfsQueue.Count != 0)
        {
            for (int x = 0; x < current.next.Count; x++)
            {
                if (current.next[x].state != VertexState.Unvisited || current == current.next[x])
                    continue;

                bfsQueue.Enqueue(current.next[x]);
                current.next[x].state = VertexState.InQueue;
            }

            current.state = VertexState.Visited;
            bfsQueue.Dequeue(); // Dequeue current
            bfsQueue.TryPeek(out current); // using TryPeak() in case Queue is empty after Dequeue
        }
        else
        {
            if (vertices[index].state != VertexState.Unvisited)
            {
                index++;
                PerformBfsStep(vertices, ref index);
                return;
            }

            bfsQueue.Enqueue(vertices[index]);
            current = bfsQueue.Peek();
            current.state = VertexState.InQueue;
        }

        index++;
        Invalidate();
    }
    private void PerformDfsStep(List<Vertex> vertices, ref int index)
    {
        if (index > vertices.Count)
        {
            Console.WriteLine("can't perform DFS further, refresh");
            return;
        }

        if (dfsStack.Count != 0)
        {

            dfsStack.Pop();
            current.state = VertexState.Visited;

            if (current.next.Count != 0)
            {
                for (int x = current.next.Count - 1; x > 0; x--)
                {
                    if (current.next[x].state != VertexState.Unvisited || current == current.next[x])
                    {
                        continue;
                    }

                    dfsStack.Push(current.next[x]);
                    vertices[x].state = VertexState.InQueue;
                }
            }
            // if stack is still empty, we can't proceed to call Peek,
            // so we instead call the method recursively, because it will fall back into an
            // else condition, because "dfsStack.Count == 0"
            if (dfsStack.Count == 0)
            {
                PerformDfsStep(vertices, ref index);
                return;
            }

            current = dfsStack.Peek();
        }
        else
        {
            if (vertices[index].state != VertexState.Unvisited)
            {
                index++;
                PerformDfsStep(vertices, ref index);
                return;
            }

            dfsStack.Push(vertices[index]);
            current = dfsStack.Peek();
            current.state = VertexState.InQueue;
        }

        index++;
        Invalidate();
    }
}