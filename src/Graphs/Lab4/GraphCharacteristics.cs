using System;
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

    static Action<Graphics> currentDraw;


    public GraphCharacteristics()
    {
        this.Text = "Lab 4";

        this.Location = new Point(100, 100);
        this.Size = new Size(840, 640);

        this.BackColor = Color.WhiteSmoke;
        Construct();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        currentDraw?.Invoke(e.Graphics);
    }

    public void Construct()
    {
        MatrixData adirMatrixData = new MatrixData(GraphWindow.BuildAdirMatrix(seed, vertexCount, k1));
        MatrixData aundirMatrixData = new MatrixData(GraphWindow.BuildAundirMatrix(seed, vertexCount, k1));

        AltGraphData graphData1 = new AltGraphData(adirMatrixData, aundirMatrixData, true, vertexCount);
        // GRAPH TOGGLE 1
        Button graphToggle1 = null;
        graphToggle1 = UIConstructor.BuildButton("GRAPH 1", new Point(10, 10), new Size(150, 30),
        () =>
        {
            UIConstructor.GraphToggle(ref currentDraw, this, new Pen(Color.Black, 2), graphData1);
            graphToggle1.Text = graphData1.directed ? "GRAPH 1 | Directed" : "GRAPH 1 | Undirected";
            this.Invalidate();
        });
        this.Controls.Add(graphToggle1);
        // STATS 1
        Button matrixDataButton1 = null;
        matrixDataButton1 = UIConstructor.BuildButton("DRAW GRAPH 1 STATS", new Point(10, 45), new Size(150, 30),
        () =>
        {
            UIConstructor.DrawGraphAction(ref currentDraw, new Point(10, 80), graphData1);
            this.Invalidate();
        });
        this.Controls.Add(matrixDataButton1);

        // SECOND GRAPH
        MatrixData adirMatrixData2 = new MatrixData(GraphWindow.BuildAdirMatrix(seed, vertexCount, k2));
        MatrixData aundirMatrixData2 = new MatrixData(GraphWindow.BuildAundirMatrix(seed, vertexCount, k2));

        AltGraphData graphData2 = new AltGraphData(adirMatrixData2, aundirMatrixData2, true, vertexCount);
        // GRAPH TOGGLE 2
        Button graphToggle2 = null;
        graphToggle2 = UIConstructor.BuildButton("GRAPH 2", new Point(160, 10), new Size(150, 30),
        () =>
        {
            UIConstructor.GraphToggle(ref currentDraw, this, new Pen(Color.Black, 2), graphData2);
            graphToggle2.Text = graphData2.directed ? "GRAPH 2 | Directed" : "GRAPH 2 | Undirected";
            this.Invalidate();
        });
        this.Controls.Add(graphToggle2);
        // STATS 2
        Button matrixDataButton2 = null;
        matrixDataButton2 = UIConstructor.BuildButton("DRAW GRAPH 2 STATS", new Point(160, 45), new Size(150, 30),
        () =>
        {
            UIConstructor.DrawDirGraphDataAction(ref currentDraw, new Point(10, 80), graphData2);
            this.Invalidate();
        });
        this.Controls.Add(matrixDataButton2);

        // CONDENSATION GRAPH
        Button condesationGraphButton = null;
        condesationGraphButton = UIConstructor.BuildButton("DRAW CONDENSATION GRAPH", new Point(320, 10), new Size(150, 30), 
        () =>
        {
            Console.WriteLine(adirMatrixData2.condensationMatrix.GetLength(0));
            currentDraw = (graphics) => GraphWindow.DrawGraph(graphics, ClientSize, new Pen(Color.Black, 2),
                adirMatrixData2.condensationMatrix.GetLength(0), adirMatrixData2.condensationMatrix, true);
            this.Invalidate();
        });
        this.Controls.Add(condesationGraphButton);
    }
}