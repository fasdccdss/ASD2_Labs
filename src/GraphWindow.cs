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
    /// to implement this we can take the N amount of "Peaks", 
    /// allocate a square of size X for each peak,
    /// than using this allocated space space, 
    /// calculate the overall size of the border the graph would be constructed inside
    /// 
    /// Graph itself is constructed using a grid that is built after the border, 
    /// each peak is assigned to its respective grid cell
    /// 
    /// It is best to draw vertices starting from the middle
    /// </summary>
    private static void DrawDirectedGraph(int seed, int wWidth, int wHeight,
     int n, int cellSize = 80)
    {
        PointF[] vertices = new PointF[n];

        int perimeter = n - 1;

        // minimum grid size that can hold all perimeter vertices
        int perSide = (int)MathF.Ceiling(perimeter / 4f);
        int columns = perSide;
        int rows = perSide;

        int requiredWidth = columns * cellSize;
        int requiredHeight = rows * cellSize;

        int startX = (wWidth - requiredWidth) / 2;
        int startY = (wHeight - requiredHeight) / 2;

        PointF Cell(int col, int row) => new PointF(
            startX + col * cellSize,
            startY + row * cellSize
        );
        // placing vertices
        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < columns; y++)
            {
                
            }
        }
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