using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;


struct Vertex
{
    public Point center;
    public int radius;

    public Vertex(int centerX, int centerY, int radius)
    {
        center.X = centerX;
        center.Y = centerY;
        this.radius = radius;
    }

    public bool Intersects(Point p)
    {
        int dx = p.X - center.X, dy = p.Y - center.Y;
        return dx * dx + dy * dy <= radius * radius;
    }
}

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
        Graphics graphics = e.Graphics;
        DrawDirectedGraph(graphics, this.ClientSize, seed, vertexCount);
    }
    /* DIRECTED GRAPH */
    private static void DrawDirectedGraph(Graphics graphics, Size clientSize,
        int seed, int n, int minSpace = 100, int vertRadius = 30)
    {
        DrawVertices(graphics, clientSize, n, minSpace, vertRadius);
    }
    /* UNDIRECTED GRAPH */
    private static void DrawGraph()
    {
        
    }
    /* ADJACENCY MATRIX */
    // here i should actually have this method return a type "Matrix" that
    // would hold all information about the matrix, so that
    // we can reference it for building arrows and shit
    private static double[,] BuildMatrix(int seed, int n)
    {
        double[,] matrix = new double[n, n];
        Random rng = new Random(seed);

        double k = 1f - 1 * 0.02f - 8 * 0.005f - 0.25f;

        for (int x = 0; x < n; x++)
        {
            for (int y = 0; y < n; y++)
            {
                double element = (rng.NextDouble() * 2.0f) * k;
                
                if (element < 1)
                    element = 0;
                else
                    element = 1;

                matrix[x, y] = element;
            }
        }

        return matrix;
    }
    /* DRAWING HELPERS */
    private static void DrawMatrix()
    {
        
    }

    private static void DrawVertices(Graphics graphics, Size clientSize, 
        int n, int minSpace = 100, int vertRadius = 30)
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
        // vertices calculations
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
        // rect calculations
        int rectWidth = maxColumns * minSpace;
        int rectHeight = maxRows * minSpace;

        int centerX = clientSize.Width / 2;
        int centerY = clientSize.Height / 2;

        int left = centerX - rectWidth / 2;
        int top = centerY - rectHeight / 2;

        int right = left + rectWidth;
        int bottom = top + rectHeight;
        //
        Vertex[] verts = new Vertex[n];
        int idx = 0;
        // placing vertices
        // top edge
        for (int x = 0; x < uColumns; x++) // here x is 0 because we need to place the first corner
            verts[idx++] = new Vertex(left + x * rectWidth / (uColumns - 1), top, vertRadius);
        // right edge
        for (int x = 1; x < rRows; x++)
            verts[idx++] = new Vertex(right, top + x * rectHeight / (rRows - 1), vertRadius);
        // bottom edge
        for (int x = 1; x < dColumns; x++)
            verts[idx++] = new Vertex(right - x * rectWidth / (dColumns - 1), bottom, vertRadius);
        // left edge
        for (int x = 1; x < lRows - 1; x++) // deduct 1(last edge) to prevent array overpopulation
            verts[idx++] = new Vertex(left, bottom - x * rectHeight / (lRows - 1), vertRadius);
        // center verticy
        verts[idx++] = new Vertex(centerX, centerY, vertRadius);
        // drawing
        using Pen pen = new Pen(Color.Black, 2);
        using Font font = new Font("Arial", vertRadius / 2);
        using SolidBrush brush = new SolidBrush(Color.Black);
        for (int x = 0; x < verts.Length; x++)
        {
            graphics.DrawEllipse(pen, verts[x].center.X - vertRadius, verts[x].center.Y - vertRadius,
                          2 * vertRadius, 2 * vertRadius);
            SizeF textSize = graphics.MeasureString((x + 1).ToString(), font);
            graphics.DrawString((x + 1).ToString(), font, brush,
                verts[x].center.X - textSize.Width / 2,
                verts[x].center.Y - textSize.Height / 2);
        }
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