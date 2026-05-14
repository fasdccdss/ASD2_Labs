using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

public class SpanningTree : Form
{
    static readonly double k = 1 - GraphParams.n3 * 0.01 - GraphParams.n4 * 0.005 - 0.05;

    static double[,] dirMatrix = GraphWindow.BuildAdirMatrix(GraphParams.seed, GraphParams.vertexCount, k);
    static double[,] undirMatrix = GraphWindow.BuildAundirMatrix(GraphParams.seed, GraphParams.vertexCount, k);

    static readonly double[,] B = BuildB(GraphParams.seed, GraphParams.vertexCount);
    static readonly double[,] C = BuildC(B);
    static readonly double[,] D = BuildD(C);
    static readonly double[,] H = BuildH(D);
    static readonly double[,] Tr = BuildTr(GraphParams.vertexCount);

    static double[,] W = BuildW(GraphParams.vertexCount);
    
    static List<Vertex> vertices = GraphConstructor.BuildVertexData(undirMatrix, W);

    // being populated at runtime by each step of building a spanning tree
    static List<Vertex> spanningTree = new List<Vertex>();
    static List<Edge> treeEdges = new List<Edge>();

    static double weightSum = 0;

    // drawing
    Pen treeEdgePen = new Pen(Color.Blue, 6);
    Pen treeVertexPen = new Pen(Color.Red, 6);
    //
    int currentStep = 0;

    public SpanningTree()
    {
        FormKraskalsTree(vertices, treeEdges);

        Button stepBtn = UIConstructor.BuildButton("REFRESH TREE", this, new Point(10, 10), new Size(150, 30),
        () =>
        {
            RefreshTree();
        });
        
        Button refreshTreeBtn = UIConstructor.BuildButton("PERFORM SPANNING TREE STEP", this, new Point(160, 10), new Size(150, 30),
        () =>
        {
            KraskalsStep(treeEdges, spanningTree, ref currentStep);
        });
    }
    protected override void OnPaint(PaintEventArgs e)
    {
        UIConstructor.DrawMatrix(e.Graphics, new Point(10, 50), "Undirectional matrix", undirMatrix, 40);
        UIConstructor.DrawMatrix(e.Graphics, new Point(10, 540), "Weight matrix", W, 40);
        WeightGraph.DrawWeightGraph(vertices, e.Graphics, ClientSize, false, 40, 60);
        UIConstructor.DrawList(e.Graphics, new Point(700, 10), "spanning tree", "", spanningTree);
        if (spanningTree.Count > 0)
            DrawSpanningTree(vertices, spanningTree, e.Graphics, ClientSize, false, 40, 60, null, null, treeEdgePen);
    }

    /* BUILDING MATRICES */
    private static double[,] BuildW(int vertexCount)
    {
        double[,] W = new double[vertexCount, vertexCount];

        for (int x = 0; x < vertexCount; x++)
        {
            for (int y = 0; y < vertexCount; y++)
            {
                W[x, y] = W[y, x] = (D[x, y] + H[x, y] * Tr[x, y]) * C[x, y];
            }
        }

        return W;
    }
    private static double[,] BuildB(int seed, int vertexCount)
    {
        double[,] B = new double[vertexCount, vertexCount];
        Random rng = new Random(seed);

        for (int x = 0; x < vertexCount; x++)
        {
            for (int y = 0; y < vertexCount; y++)
            {
                double element = (rng.NextDouble() * 2.0f) * k;

                B[x, y] = element;
            }
        }

        return B;
    }
    private static double[,] BuildC(double[,] B)
    {
        int n = B.GetLength(0);
        double[,] C = new double[n, n];

        for (int x = 0; x < n; x++)
        {
            for (int y = 0; y < n; y++)
            {
                C[x, y] = Math.Ceiling(B[x, y] * 100 * undirMatrix[x, y]);
            }
        }

        return C;
    }
    private static double[,] BuildD(double[,] C)
    {
        int n = C.GetLength(0);
        double[,] D = new double[n, n];

        for (int x = 0; x < n; x++)
        {
            for (int y = 0; y < n; y++)
            {
                if (C[x, y] == 0)
                    D[x, y] = 0;
                else if (C[x, y] > 0)
                    D[x, y] = 1;
            }
        }

        return D;
    }
    private static double[,] BuildH(double[,] D)
    {
        int n = D.GetLength(0);
        double[,] H = new double[n, n];

        for (int x = 0; x < n; x++)
        {
            for (int y = 0; y < n; y++)
            {
                H[x, y] = D[x, y] == D[y, x] ? 1 : 0;
            }
        }

        return H;
    }
    private static double[,] BuildTr(int vertexCount)
    {
        double[,] Tr = new double[vertexCount, vertexCount];


        for (int x = 0; x < vertexCount; x++)
        {
            for (int y = 0; y < vertexCount; y++)
            {
                Tr[x, y] = x < y ? 1 : 0;
            }
        }

        return Tr;
    }

    /* SPANNING TREE */
    private void RefreshTree()
    {
        treeEdges.Clear();
        spanningTree.Clear();

        FormKraskalsTree(vertices, treeEdges);
        currentStep = 0;

        Invalidate();
    }

    private void KraskalsStep(List<Edge> treeEdges, List<Vertex> spanningTree, ref int step)
    {
        if (step >= treeEdges.Count)
        {
            weightSum = WeightSum(spanningTree);
            Console.WriteLine(weightSum);
            return;
        }

        int s = step;
        Edge edge = treeEdges[s];

        Vertex from = spanningTree.Find(v => v.index == edge.from.index);
        if (from == null)
        {
            from = new Vertex(edge.from.index);
            from.center = edge.from.center;
            spanningTree.Add(from);
        }

        Vertex to = spanningTree.Find(v => v.index == edge.to.index);
        if (to == null)
        {
            to = new Vertex(edge.to.index);
            to.center = edge.to.center;
            spanningTree.Add(to);
        }

        from.next.Add(to);

        step++;
        Invalidate();
    }

    private void FormKraskalsTree(List<Vertex> vertices, List<Edge> treeEdges)
    {
        Edge? best = null;

        for (int x = 0; x < vertices.Count; x++)
        {
            foreach ((Vertex v, double w) in vertices[x].nextV)
            {
                if (CanReach(vertices[x], v, treeEdges))
                    continue;

                if (best == null || w < best.Value.weight)
                    best = new Edge { from = vertices[x], to = v, weight = w };
            }
        }

        if (best != null)
        {
            treeEdges.Add(best.Value);
            FormKraskalsTree(vertices, treeEdges);
        }
    }
    
    private bool CanReach(Vertex start, Vertex target, List<Edge> edges)
    {
        HashSet<Vertex> visited = new HashSet<Vertex>();
        Queue<Vertex> queue = new Queue<Vertex>();
        queue.Enqueue(start);

        while (queue.Count > 0)
        {
            Vertex current = queue.Dequeue();
            if (current == target) return true;
            if (!visited.Add(current)) continue;

            foreach (Edge edge in edges)
            {
                if (edge.from == current && !visited.Contains(edge.to))
                    queue.Enqueue(edge.to);
                if (edge.to == current && !visited.Contains(edge.from))
                    queue.Enqueue(edge.from);
            }
        }
        return false;
    }


    public void DrawSpanningTree(List<Vertex> vertices, List<Vertex> spanningTree, Graphics graphics, Size clientSize, bool directed = true,
        int vertexRadius = 60, int vertexOffset = 90, Font font = null, SolidBrush solidBrush = null, Pen pen = null)
    {
        if (font == null)
            font = new Font("Arial", vertexRadius / 2);
        if (solidBrush == null)
            solidBrush = new SolidBrush(Color.White);
        if (pen == null)
            pen = new Pen(Color.Black, 2);

        Dictionary<Color, SolidBrush> fillBrushes = new Dictionary<Color, SolidBrush>();
        Dictionary<Color, SolidBrush> fontBrushes = new Dictionary<Color, SolidBrush>();

        for (int x = 0; x < spanningTree.Count; x++)
        {
            Color fillColor = spanningTree[x].FillColor();
            if (!fillBrushes.ContainsKey(fillColor))
            {
                fillBrushes.Add(fillColor, new SolidBrush(fillColor));
            }

            Color fontColor = spanningTree[x].FontColor();
            if (!fontBrushes.ContainsKey(fontColor))
            {
                fontBrushes.Add(fontColor, new SolidBrush(fontColor));
            }

            graphics.FillEllipse(solidBrush, spanningTree[x].center.X - vertexRadius, spanningTree[x].center.Y - vertexRadius,
                2 * vertexRadius, 2 * vertexRadius);

            graphics.DrawEllipse(treeVertexPen, spanningTree[x].center.X - vertexRadius, spanningTree[x].center.Y - vertexRadius,
                2 * vertexRadius, 2 * vertexRadius);

            SizeF textSize = graphics.MeasureString(spanningTree[x].index.ToString(), font);
            graphics.DrawString(spanningTree[x].index.ToString(), font, fontBrushes[fontColor],
                spanningTree[x].center.X - textSize.Width / 2,
                spanningTree[x].center.Y - textSize.Height / 2);
        }

        FollowEdges(vertices, spanningTree, graphics, pen, vertexRadius, directed);
    }

    public static void FollowEdges(List<Vertex> vertices, List<Vertex> followV, Graphics graphics, Pen pen, 
    int vertexRadius, bool directed)
    {
        for (int x = 0; x < followV.Count; x++)
        {
            foreach (var v in followV[x].next)
            {
                if (followV[x].index == v.index)
                {
                    graphics.DrawEllipse(pen,
                    followV[x].center.X,
                    followV[x].center.Y - vertexRadius * 2,
                    vertexRadius, vertexRadius);
                    continue;
                }

                float angle = MathF.Atan2(
                    v.center.Y - followV[x].center.Y,
                    v.center.X - followV[x].center.X
                );

                Point start = new Point(
                    followV[x].center.X + (int)(vertexRadius * MathF.Cos(angle)),
                    followV[x].center.Y + (int)(vertexRadius * MathF.Sin(angle))
                );
                Point end = new Point(
                    v.center.X - (int)(vertexRadius * MathF.Cos(angle)),
                    v.center.Y - (int)(vertexRadius * MathF.Sin(angle))
                );
                Point?[] breaks = new Point?[vertices.Count];

                for (int z = 0; z < vertices.Count; z++)
                {
                    if (vertices[z].index == followV[x].index || vertices[z].index == v.index) continue;

                    Point? intersection = GraphConstructor.LineIntersectsCircle(start, end, vertices[z]);
                    if (intersection.HasValue)
                    {
                        float ldx = end.X - start.X, ldy = end.Y - start.Y;
                        float crossZ = ldx * (vertices[z].center.Y - start.Y)
                                     - ldy * (vertices[z].center.X - start.X);
                        int side = crossZ > 0 ? 1 : -1;

                        // tangent point from start to the blocker circle
                        float cdx = vertices[z].center.X - start.X, cdy = vertices[z].center.Y - start.Y;
                        float D2 = cdx * cdx + cdy * cdy;
                        float D = MathF.Sqrt(D2);
                        float L2 = D2 - vertexRadius * vertexRadius;
                        float d = L2 / D;

                        float underSqrt = L2 - d * d;
                        if (underSqrt < 0) continue;

                        float h = MathF.Sqrt(underSqrt);

                        float tx = start.X + (cdx * d + side * (-cdy) * h) / D;
                        float ty = start.Y + (cdy * d + side * cdx * h) / D;

                        // small clearance margin
                        float nx = (tx - vertices[z].center.X) / vertexRadius;
                        float ny = (ty - vertices[z].center.Y) / vertexRadius;
                        breaks[z] = new Point(
                            (int)(tx + nx * 5),
                            (int)(ty + ny * 5)
                        );
                        break;
                    }
                }

                if (directed)
                    GraphConstructor.DrawBrokenArrow(graphics, pen, start, breaks, end, GraphConstructor.arrowPen);
                else
                    GraphConstructor.DrawBrokenLine(graphics, pen, start, breaks, end);
            }
        }
    }

    private static double WeightSum(List<Vertex> spanningTree)
    {
        double sumW = 0;

        foreach (Vertex v in spanningTree)
        {
            foreach ((Vertex q, double w) in v.nextV)
                sumW += w;
        }

        return sumW;
    }
}