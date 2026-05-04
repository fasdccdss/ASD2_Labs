using System;
using System.Drawing;
using System.Windows.Forms;

public class GraphWindow : Form
{
    int seed = 5118;
    int vertexCount = 11;

    public GraphWindow()
    {
        this.Text = "Lab 3";

        this.Location = new Point(100, 100);
        this.Size = new Size(460, 240);

        this.BackColor = Color.White;

        this.Controls.Add(BuildSwichBtn());
    }
    static void Main()
    {
        Application.Run(new GraphWindow());

        
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        Graphics g = e.Graphics;


    }
    /* DIRECTED GRAPH */
    /// <summary>
    /// 
    /// </summary>
    private static void DrawDirectedGraph(int seed,
     int n, int minSpace = 30, int vertRadius = 5)
    {
        int minRows = 3;
        int minColumns = 3;
        int minEdges = 2 * (minRows + minColumns) - 4; // 8

        int vertices = n - 1;

        int maxRows = minRows;
        int lRows = minRows;
        int rRows = minRows;

        int maxColumns = minColumns;
        int uColumns = minColumns;
        int dColumns = minColumns;

        if (vertices > minEdges)
        {
            bool increaseRows = false;
            bool l = true;

            for (int x = 0; x < vertices - minEdges; x++)
            {
                if (x != 0 && x % 2 == 0)
                {
                    increaseRows = !increaseRows;
                }

                if (increaseRows)
                {
                    maxRows++;

                    if (l)
                    {
                        lRows++;
                        l = !l;
                    }
                    else
                    {
                        rRows++;
                        l = !l;
                    }
                }
                else
                {
                    maxColumns++;

                    if (l)
                    {
                        uColumns++;
                        l = !l;
                    }
                    else
                    {
                        dColumns++;
                        l = !l;
                    }
                }
            }
        }

        int rectWidth = maxColumns * minSpace;
        int rectHeight = maxRows * minSpace;
    }
    /* UNDIRECTED GRAPH */
    private static void DrawGraph()
    {
        
    }
    /* DRAWING HELPERS */
    private static void ConstructEdge()
    {
        
    }
    private static void DrawArrow(Graphics graphics, Pen pen, Point start,
     Point tip, float arrowAngle = 35f)
    {
        graphics.DrawLine(pen, start, tip);
        ArrowHead(graphics, pen, arrowAngle, tip);
    }
    private static void ArrowHead(Graphics graphics, Pen pen, float angle,
     Point tip)
    {
        angle = 3.1416f * (180f - angle) / 180f;

        int lx = tip.X + (int)(15 * MathF.Cos(angle + 0.3f));
        int rx = tip.X + (int)(15 * MathF.Cos(angle - 0.3f));
        int ly = tip.Y + (int)(15 * MathF.Sin(angle + 0.3f));
        int ry = tip.Y + (int)(15 * MathF.Sin(angle - 0.3f));

        graphics.DrawLine(pen, lx, ly, tip.X, tip.Y);
        graphics.DrawLine(pen, tip.X, tip.Y, rx, ry);
    }
    /* BUTTONS */
    private Button BuildSwichBtn()
    {
        bool showDirected = true;

        Button switchBtn = new Button();
        switchBtn.Text = "Directed graph";
        switchBtn.Location = new Point(10, 10);
        switchBtn.Size = new Size(150, 30);
        switchBtn.Click += (s, e) =>
        {
            showDirected = !showDirected;
            switchBtn.Text = showDirected ? "Directed graph" : "Undirected graph";
            this.Invalidate(); // Re-draws
        };

        return switchBtn;
    }
}