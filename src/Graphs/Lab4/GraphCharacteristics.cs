using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

public class GraphCharacteristics : Form
{
    static int n3 = GraphParams.n3;
    static int n4 = GraphParams.n4;

    static int vertexCount = GraphParams.vertexCount;

    static int seed = GraphParams.seed;

    static double k1 = 1.0 - n3 * 0.01 - n4 * 0.01 - 0.03;
    static double k2 = 1.0 - n3 * 0.005 - n4 * 0.005 - 0.27;

    static System.Action<Graphics> currentDraw;


    public GraphCharacteristics()
    {
        this.Text = "Lab 4";

        this.Location = new Point(100, 100);
        this.Size = new Size(840, 640);

        this.BackColor = Color.GhostWhite;
        Construct();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        currentDraw?.Invoke(e.Graphics);
    }

    public void Construct()
    {
        // GRAPH 1
        double[,] adirMatrix1 = GraphWindow.BuildAdirMatrix(seed, vertexCount, k1);
        double[,] aundirMatrix1 = GraphWindow.BuildAundirMatrix(seed, vertexCount, k1);
        GraphData graphData1 = new GraphData(adirMatrix1, aundirMatrix1, true, vertexCount);

        Button graphToggle1 = null;
        graphToggle1  = UIConstructor.BuildButton("GRAPH 1", new Point(10, 10), new Size(150, 30), 
        () => 
        {
            UIConstructor.GraphToggle(ref currentDraw, this, new Pen(Color.Black, 2), graphData1);
            graphToggle1.Text = graphData1.directed ? "GRAPH 1 | Directed" : "GRAPH 1 | Undirected";
            this.Invalidate();
        });
        this.Controls.Add(graphToggle1);
        // DEGREES
        // directional matrix
        MatrixData adirMatrixData = graphData1.adirMatrixData;

        (adirMatrixData.vertexDegrees, adirMatrixData.vertexInDegrees, adirMatrixData.vertexOutDegrees)
        = VertexDegreeArray(adirMatrixData.matrix);
        // undirectional matrix
        MatrixData aundirMatrixData = graphData1.aundirMatrixData;

        (aundirMatrixData.vertexDegrees, aundirMatrixData.vertexInDegrees, aundirMatrixData.vertexOutDegrees)
        = VertexDegreeArray(aundirMatrixData.matrix);

        // DRAWING GRAPH 1 STATS
        Button matrixDataButton1 = null;
        matrixDataButton1 = UIConstructor.BuildButton("DRAW MATRIX STATS", new Point(10, 45), new Size(150, 30),
        () =>
        {
            UIConstructor.FuckThis(ref currentDraw, this, new Point(10, 80), graphData1);
            this.Invalidate();
        });
        this.Controls.Add(matrixDataButton1);

        // GRAPH 2
        double[,] adirMatrix2 = GraphWindow.BuildAdirMatrix(seed, vertexCount, k2);
        double[,] aundirMatrix2 = GraphWindow.BuildAundirMatrix(seed, vertexCount, k2);
        GraphData matrixData2 = new GraphData(adirMatrix2, aundirMatrix2, true, vertexCount);

        Button graphToggle2 = null;
        graphToggle2 = UIConstructor.BuildButton("GRAPH 2", new Point(170, 10), new Size(150, 30),
        () => 
        {
            UIConstructor.GraphToggle(ref currentDraw, this, new Pen(Color.Black, 2), matrixData2);
            graphToggle2.Text = matrixData2.directed ? "GRAPH 2 | Directed" : "GRAPH 2 | Undirected";
            this.Invalidate();
        });
        this.Controls.Add(graphToggle2);
        // DRAWING MATRIX2 STATS
    }

    /* VERTEX DEGREES */
    // full degrees
    public static (int[] vertexDegrees, int[] vertexInDegrees, int[] vertexOutDegrees) VertexDegreeArray(double[,] matrix)
    {
        int n = matrix.GetLength(0);

        int[] vertexDegrees = new int[n];
        int[] vertexInDegrees = new int[n];
        int[] vertexOutDegrees = new int[n];

        for (int x = 0; x < n; x++)
        {
            (vertexDegrees[x], vertexInDegrees[x], vertexOutDegrees[x]) = GetVertexDegree(x, matrix);
        }

        return (vertexDegrees, vertexInDegrees, vertexOutDegrees);
    }
    public static (int degree, int inDegree, int outDegree) GetVertexDegree(int vertexIndex, double[,] matrix)
    {
        int inDegree = GetVertexInDegree(vertexIndex, matrix);
        int outDegree = GetVertexOutDegree(vertexIndex, matrix);

        return (inDegree + outDegree, inDegree, outDegree);
    }
    // out-degree
    public static int GetVertexInDegree(int vertexIndex, double[,] matrix)
    {
        int outDegree = 0;

        for (int x = 0; x < matrix.GetLength(0); x++)
        {
            if (matrix[x, vertexIndex] == 1)
            {
                outDegree++;
            }
        }

        return outDegree;
    }
    // in-degree
    public static int GetVertexOutDegree(int vertexIndex, double[,] matrix)
    {
        int inDegree = 0;

        for (int y = 0; y < matrix.GetLength(1); y++)
        {
            if (matrix[vertexIndex, y] == 1)
            {
                inDegree++;
            }
        }

        return inDegree;
    }
    /* GRAPH REGULARNESS CHECK */
    public static bool isRegular(double[,] matrix, out int? degree)
    {
        int n = matrix.GetLength(0);

        int firstOut = GetVertexOutDegree(0, matrix);
        int firstIn = GetVertexInDegree(0, matrix);

        for (int x = 0; x < n; x++)
        {
            if (GetVertexOutDegree(x, matrix) != firstOut || GetVertexInDegree(x, matrix) != firstIn)
            {
                degree = null;
                return false;
            }
        }

        degree = firstOut;
        return true;
    }

    /* HANGING/ISOLATED VERTICES */
    // here we return a dictionary that we can assign to smth and later draw
    public static Dictionary<int, string> IsolatedVerts(double[,] matrix)
    {
        Dictionary<int, string> results = new Dictionary<int, string>();

        int n = matrix.GetLength(0);

        for (int x = 0; x < n; x++)
        {
            int outDegree = GetVertexOutDegree(x, matrix);
            int inDegree = GetVertexInDegree(x, matrix);

            if (outDegree == 0 && inDegree == 0)
            {
                results[x] = "hanging";
            }
            else if ((outDegree == 1 && inDegree == 0) || (outDegree == 0 && inDegree == 1))
            {
                results[x] = "isolated";
            }
            else 
                results[x] = "regular";
        }

        return results;
    }

    /* MATRIX TRANSFORMATIONS */
}