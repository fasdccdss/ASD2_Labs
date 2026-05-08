using System;
using System.CodeDom;
using System.Drawing;
using System.Reflection.Metadata;
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
    static bool showDirected = true;

    static int seed = GraphParams.seed;
    static int vertexCount = 11;
    static double k = 1f - 1 * 0.02f - 8 * 0.005f - 0.25f;

    static double[,] adirMatrix = BuildAdirMatrix(seed, vertexCount, k);
    static double[,] aundirMatrix = BuildAundirMatrix(seed, vertexCount, k);

    public GraphWindow()
    {
        this.Text = "Lab 3";

        this.Location = new Point(100, 100);
        this.Size = new Size(460, 240);

        this.BackColor = Color.White;

        this.Controls.Add(BuildSwichBtn());
        this.Controls.Add(BuildMatrixButton());
    }
    /*
    private static void Main()
    {
        Application.Run(new GraphWindow());
    }
    */
    protected override void OnPaint(PaintEventArgs e)
    {
        Graphics graphics = e.Graphics;
        Pen pen = new Pen(Color.Black, 2.0f);

        double[,] currentMatrix = showDirected == true ? adirMatrix : aundirMatrix;

        DrawChosenGraph(graphics, this.ClientSize, pen, vertexCount, showDirected);
    }
    /* GRAPH */
    private static void DrawChosenGraph(Graphics graphics, Size clientSize, Pen pen,
        int n, bool directed, int minSpace = 100, int vertRadius = 30)
    {
        Vertex[] verts = DrawVertices(graphics, clientSize, pen, n, minSpace, vertRadius);
        if (directed)
            DrawEdges(graphics, pen, verts, n, adirMatrix, vertRadius, directed);
        else
            DrawEdges(graphics, pen, verts, n, aundirMatrix, vertRadius, directed);
    }

    public static void DrawGraph(Graphics graphics, Size clientSize, Pen pen,
        int n, double[,] matrix, bool directed, int minSpace = 100, int vertRadius = 30)
    {
        Vertex[] verts = DrawVertices(graphics, clientSize, pen, n, minSpace, vertRadius);
        DrawEdges(graphics, pen, verts, n, matrix, vertRadius, directed);
    }

    private static void DrawEdges(Graphics graphics, Pen pen, Vertex[] verts,
        int n, double[,] matrix, int vertRadius, bool directed)
    {
        for (int x = 0; x < n; x++)
        {
            for (int y = 0; y < n; y++)
            {
                if (matrix[x, y] == 1)
                {
                    if (x == y)
                    {
                        graphics.DrawEllipse(pen,
                            verts[x].center.X,
                            verts[x].center.Y - vertRadius * 2,
                            vertRadius, vertRadius);
                        continue;
                    }

                    float angle = MathF.Atan2(
                        verts[y].center.Y - verts[x].center.Y,
                        verts[y].center.X - verts[x].center.X
                    );

                    Point start = new Point(
                        verts[x].center.X + (int)(vertRadius * MathF.Cos(angle)),
                        verts[x].center.Y + (int)(vertRadius * MathF.Sin(angle))
                    );
                    Point end = new Point(
                        verts[y].center.X - (int)(vertRadius * MathF.Cos(angle)),
                        verts[y].center.Y - (int)(vertRadius * MathF.Sin(angle))
                    );
                    Point?[] breaks = new Point?[n];

                    // find any possible intersection points and offset them
                    // to store as a break
                    for (int z = 0; z < n; z++)
                    {
                        if (z == x || z == y) continue;

                        Point? intersection = LineIntersectsCircle(start, end, verts[z]);
                        if (intersection.HasValue)
                        {
                            float ldx = end.X - start.X, ldy = end.Y - start.Y;
                            float crossZ = ldx * (verts[z].center.Y - start.Y)
                                         - ldy * (verts[z].center.X - start.X);
                            int side = crossZ > 0 ? 1 : -1;

                            // tangent point from start to the blocker circle
                            float cdx = verts[z].center.X - start.X, cdy = verts[z].center.Y - start.Y;
                            float D2 = cdx * cdx + cdy * cdy;
                            float D = MathF.Sqrt(D2);
                            float L2 = D2 - verts[z].radius * verts[z].radius;
                            float d = L2 / D;
                            float h = MathF.Sqrt(L2 - d * d);

                            float tx = start.X + (cdx * d + side * (-cdy) * h) / D;
                            float ty = start.Y + (cdy * d + side * cdx * h) / D;

                            // small clearance margin
                            float nx = (tx - verts[z].center.X) / verts[z].radius;
                            float ny = (ty - verts[z].center.Y) / verts[z].radius;
                            breaks[z] = new Point(
                                (int)(tx + nx * 5),
                                (int)(ty + ny * 5)
                            );
                            break;
                        }
                    }

                    if (directed)
                        DrawBrokenArrow(graphics, pen, start, breaks, end);
                    else
                        DrawBrokenLine(graphics, pen, start, breaks, end);
                }
            }
        }
    }
    /* ADJACENCY MATRIX */
    private Button BuildMatrixButton()
    {
        {
            Form matrixForm = null;

            Button btn = new Button();
            btn.Text = "Show Matrix";
            btn.Location = new Point(170, 10);
            btn.Size = new Size(150, 30);
            btn.Click += (s, e) =>
            {
                if (matrixForm == null || matrixForm.IsDisposed)
                {
                    matrixForm = new Form();
                    matrixForm.Text = "Adjacency Matrix";
                    matrixForm.Size = new Size(600, 300);
                    matrixForm.BackColor = Color.White;
                    matrixForm.Paint += (s2, e2) => 
                        DrawMatrix(e2.Graphics, adirMatrix, vertexCount, new Point(10, 10));
                    matrixForm.Paint += (s2, e2) =>
                        DrawMatrix(e2.Graphics, aundirMatrix, vertexCount, new Point(310, 10));
                    matrixForm.Show();
                    btn.Text = "Hide Matrix";
                    matrixForm.FormClosed += (s2, e2) => btn.Text = "Show Matrix";
                }
                else
                {
                    matrixForm.Close();
                    btn.Text = "Show Matrix";
                }
            };

            return btn;
        }
    }
    private void ShowMatrixWindow(double[,] matrix, int n)
    {
        Form matrixForm = new Form();
        matrixForm.Text = "Adjacency Matrix";
        matrixForm.Size = new Size(300, 300);
        matrixForm.BackColor = Color.White;

        matrixForm.Paint += (s, e) =>
        {
            DrawMatrix(e.Graphics, matrix, n, new Point(10, 10));
        };

        matrixForm.Show();
    }
    public static void DrawMatrix(Graphics graphics, double[,] matrix, int n, Point origin)
    {
        using Font font = new Font("Comic Sans MS", 10); // :)
        using SolidBrush brush = new SolidBrush(Color.Black);
        int cellSize = 20;

        for (int x = 0; x < n; x++)
            for (int y = 0; y < n; y++)
            {
                string val = matrix[x, y] == 1 ? "1" : "0";
                graphics.DrawString(val, font, brush,
                    origin.X + y * cellSize,
                    origin.Y + x * cellSize);
            }
    }
    public static double[,] BuildAdirMatrix(int seed, int n, double k)
    {
        double[,] matrix = new double[n, n];
        Random rng = new Random(seed);

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
    public static double[,] BuildAundirMatrix(int seed, int n, double k)
    {
        double[,] matrix = new double[n, n];
        Random rng = new Random(seed);

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
                matrix[y, x] = element;
            }
        }

        return matrix;
    }
    /* DRAWING HELPERS */
    private static Vertex[] DrawVertices(Graphics graphics, Size clientSize, Pen pen,
        int rows, int columns, int minSpace = 100, int vertRadius = 30)
    {
        int minRows = 2;
        int minColumns = 2;
        int minEdges = 2 * (rows + columns) - 4; // 8

        int vertices = 2 * (rows + columns) - 4; ;

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
        Vertex[] verts = new Vertex[vertices];
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

        return verts;
    }

    private static Point? LineIntersectsCircle(Point a, Point b, Vertex v)
    {
        float dx = b.X - a.X;
        float dy = b.Y - a.Y;

        float fx = a.X - v.center.X;
        float fy = a.Y - v.center.Y;

        float aVal = dx * dx + dy * dy;
        float bVal = 2 * (fx * dx + fy * dy);
        float c = fx * fx + fy * fy - v.radius * v.radius;

        float discriminant = bVal * bVal - 4 * aVal * c;
        if (discriminant < 0) return null;

        float t = (-bVal - MathF.Sqrt(discriminant)) / (2 * aVal);
        if (t < 0 || t > 1) return null; // intersection outside the segment

        return new Point(
            (int)(a.X + t * dx),
            (int)(a.Y + t * dy)
        );
    }

    private static void DrawBrokenArrow(Graphics graphics, Pen pen,
        Point start, Point?[] breaks, Point end)
    {
        Point lastPoint = start;
        for (int x = 0; x < breaks.Length; x++)
        {
            if (breaks[x] != null)
            {
                graphics.DrawLine(pen, lastPoint, breaks[x].Value);
                lastPoint = breaks[x].Value;
            }
        }
        float arrowAngle = MathF.Atan2(end.Y - lastPoint.Y, end.X - lastPoint.X) * 180f / MathF.PI;

        graphics.DrawLine(pen, lastPoint, end);
        ArrowHead(graphics, pen, arrowAngle, end);
    }
    private static void DrawBrokenLine(Graphics graphics, Pen pen,
    Point start, Point?[] breaks, Point end)
    {
        Point lastPoint = start;
        for (int x = 0; x < breaks.Length; x++)
        {
            if (breaks[x] != null)
            {
                graphics.DrawLine(pen, lastPoint, breaks[x].Value);
                lastPoint = breaks[x].Value;
            }
        }

        graphics.DrawLine(pen, lastPoint, end);
    }
    private static void DrawArrow(Graphics graphics, Pen pen, Point start,
     Point tip, float arrowAngle)
    {
        graphics.DrawLine(pen, start, tip);
        ArrowHead(graphics, pen, arrowAngle, tip);
    }

    private static void ArrowHead(Graphics graphics, Pen pen, float angle,
     Point tip)
    {
        angle = 3.1416f * (180f + angle) / 180f;

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