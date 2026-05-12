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

    Queue<Vertex> bfsQueue = new Queue<Vertex>();
    Stack<Vertex> dfsStack = new Stack<Vertex>();
    
    int modeIndex = 0; // for tracking which algorithm user chose to perform

    Vertex current = null;

    // storing information for drawing indexes of the vertices
    List<(Vertex vertex, string label)> stepLabels = new List<(Vertex, string)>();
    Dictionary<Color, SolidBrush> fillBrushes = new Dictionary<Color, SolidBrush>();
    Dictionary<Color, SolidBrush> fontBrushes = new Dictionary<Color, SolidBrush>();

    // DEBUG DRAWING
    Font font = new Font("Arial", 21);
    SolidBrush brush = new SolidBrush(Color.Black);

    // tast specific
    double[,] treeMatrix;
    List<int> newIndices;

    public GraphBypass()
    {
        this.Text = "Lab 4";

        this.Location = new Point(100, 100);
        this.Size = new Size(840 * 2, 1000);

        this.BackColor = Color.WhiteSmoke;

        vertices = GraphConstructor.BuildVertexData(dirMatrix);
        PopulateDrawingDictionaries(vertices);

        RefreshGraph();

        Button bfsStepBtn = UIConstructor.BuildButton("BFS STEP", this, new Point(10, 10), new Size(150, 30), 
        () =>
        {
            if (modeIndex == 0)
                modeIndex = 1;
            else if (modeIndex != 1)
            {
                Console.WriteLine("can't perform BFS step along with DFS");
                return;
            }
            PerformBfsStep(vertices, ref runtimeIndex); 
        });

        Button dfsStepBtn = UIConstructor.BuildButton("DFS STEP", this, new Point(10, 60), new Size(150, 30),
        () =>
        {
            if (modeIndex == 0)
                modeIndex = 2;
            else if (modeIndex != 2)
            {
                Console.WriteLine("can't perform DFS step along with BFS");
                return;
            }

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
        GraphConstructor.DrawGraphNoIndex(vertices, e.Graphics, ClientSize, true, 50, 90);

        int px = 10;
        int py = 100;

        foreach (Vertex v in dfsStack)
        {
            e.Graphics.DrawString(v.index.ToString(), font, brush, px, py);
            py += 24;
        }

        foreach (var (vertex, label) in stepLabels)
        {
            SizeF size = e.Graphics.MeasureString(label, font);
            e.Graphics.DrawString(label, font,
            fontBrushes[vertex.FontColor()],
            vertex.center.X - size.Width / 2, vertex.center.Y - size.Height / 2);
        }

        UIConstructor.DrawList(e.Graphics, new Point(10, 100),
        "Список вiдповiдностi номерiв вершин i їх нової нумерацiї, на-бутої в процесi обходу",
        "", newIndices);

        UIConstructor.DrawMatrix(e.Graphics, new Point(10, 350),
         "Згенерована матриця сумiжностi напрямленого графа", dirMatrix);
        UIConstructor.DrawMatrix(e.Graphics, new Point(10, 620), "Матриця сумiжностi дерева обходу", treeMatrix);
    }

    /* POPULATING DRAWING DICTIONARIES */
    private void PopulateDrawingDictionaries(List<Vertex> vertices)
    {
        foreach (VertexState state in Enum.GetValues<VertexState>())
        {
            Vertex temp = new Vertex { state = state };

            Color fillColor = temp.FillColor();
            if (!fillBrushes.ContainsKey(fillColor))
                fillBrushes.Add(fillColor, new SolidBrush(fillColor));

            Color fontColor = temp.FontColor();
            if (!fontBrushes.ContainsKey(fontColor))
                fontBrushes.Add(fontColor, new SolidBrush(fontColor));
        }
    }

    /* REFRESH */
    private void RefreshGraph()
    {
        modeIndex = 0;
        bfsQueue.Clear();
        dfsStack.Clear();

        for (int x = 0; x < vertices.Count; x++)
        {
            vertices[x].state = VertexState.Unvisited;

            // bfsQueue.Enqueue(vertices[x]);
            // dfsQueue.Enqueue(vertices[x]);
        }
        
        stepLabels.Clear();

        treeMatrix = new double[vertices.Count, vertices.Count];
        newIndices = new List<int>();

        current = null;
        runtimeIndex = startIndex;
        Invalidate();
    }
    /* SEARCHES */
    /* BFS */
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
                
                treeMatrix[current.index - 1, current.next[x].index - 1] = 1; // TASK SPECIFIC, CAN REMOVE
            }

            current.state = VertexState.Visited;
            bfsQueue.Dequeue(); // Dequeue current
            bfsQueue.TryPeek(out current); // using TryPeak() in case Queue is empty after Dequeue
        }
        else
        {
            if (vertices[index].state != VertexState.Unvisited)
            {
                Console.WriteLine(index);
                index++;
                PerformBfsStep(vertices, ref index);
                return;
            }

            bfsQueue.Enqueue(vertices[index]);
            current = bfsQueue.Peek();
            current.state = VertexState.InQueue;
        }

        if (current != null) //
            newIndices.Add(current.index); // TASK SPECIFIC, CAN REMOVE

        index++;

        if (current != null)
            stepLabels.Add((current, index.ToString())); // drawing
        // Console.WriteLine(index);
        Invalidate();
    }

    /* DFS */
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

            if (current.next.Count == 0)
            {
                // after the initial "Pop" stack might be empty and a raw Pop would throw an error
                // thats why there's a TryPop
                dfsStack.TryPop(out current);
                current.state = VertexState.Visited;
                return;
            }

            for (int x = current.next.Count - 1; x >= 0; x--) // condition is purpousful. DO NOT REWRITE
            {
                if (current.next[x].state != VertexState.Unvisited || current == current.next[x])
                {
                    continue;
                }

                dfsStack.Push(current.next[x]);
                dfsStack.Peek().state = VertexState.InQueue;

                treeMatrix[current.index - 1, current.next[x].index - 1] = 1; // TASK SPECIFIC, CAN REMOVE
            }

            // after the initial "Pop" stack might be empty and a raw Peek would throw an error
            // thats why there's a TryPeek
            dfsStack.TryPeek(out current);
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

        if (current != null)  //
            newIndices.Add(current.index); // TASK SPECIFIC, CAN REMOVE

        index++;

        if (current != null)
            stepLabels.Add((current, index.ToString())); // drawing
        // Console.WriteLine(index);
        Invalidate();
    }
}