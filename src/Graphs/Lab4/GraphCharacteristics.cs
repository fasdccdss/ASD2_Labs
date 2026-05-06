using System.CodeDom;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.Design;

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
        double[,] adirMatrix1 = GraphWindow.BuildAdirMatrix(seed, vertexCount, k1);
        double[,] aundirMatrix1 = GraphWindow.BuildAundirMatrix(seed, vertexCount, k1);
        MatrixData matrixData1 = new MatrixData(adirMatrix1, aundirMatrix1, true, vertexCount);

        Button matrixToggle1 = null;
        matrixToggle1  = UIConstructor.BuildButton("GRAPH 1", new Point(10, 10), new Size(150, 30), 
        () => 
        {
            UIConstructor.GraphToggle(ref currentDraw, this, new Pen(Color.Black, 2), matrixData1);
            matrixToggle1.Text = matrixData1.directed ? "GRAPH 1 | Directed" : "GRAPH 1 | Undirected";
            this.Invalidate();
        });
        this.Controls.Add(matrixToggle1);

        double[,] adirMatrix2 = GraphWindow.BuildAdirMatrix(seed, vertexCount, k2);
        double[,] aundirMatrix2 = GraphWindow.BuildAundirMatrix(seed, vertexCount, k2);
        MatrixData matrixData2 = new MatrixData(adirMatrix2, aundirMatrix2, true, vertexCount);

        Button matrixToggle2 = null;
        matrixToggle2 = UIConstructor.BuildButton("GRAPH 2", new Point(170, 10), new Size(150, 30),
        () => 
        {
            UIConstructor.GraphToggle(ref currentDraw, this, new Pen(Color.Black, 2), matrixData2);
            matrixToggle2.Text = matrixData2.directed ? "GRAPH 2 | Directed" : "GRAPH 2 | Undirected";
            this.Invalidate();
        });
        this.Controls.Add(matrixToggle2);
    }

    /* VERTEX DEGREES */
    // full degrees
    public static int[] VertexDegreeArray(double[,] matrix)
    {
        int n = matrix.GetLength(0);

        int[] vertexDegrees = new int[n];

        for (int x = 0; x < n; x++)
        {
            vertexDegrees[x] = GetVertexDegree(x, matrix);
        }

        return vertexDegrees;
    }
    public static int GetVertexDegree(int vertexIndex, double[,] matrix)
    {
        return GetVertexInDegree(vertexIndex, matrix) + GetVertexOutDegree(vertexIndex, matrix);
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

    /* HANGING/ISOLATED VERTECISE */
    public static int IsolatedVerts(double[,] matrix)
    {
        
    }

    /* MATRIX TRANSFORMATIONS */
}