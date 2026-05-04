using System;
using System.Drawing;
using System.Numerics;
using System.Windows.Forms;

public class GraphWindow : Form
{
    int seed = 5118;


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

    /* UNDIRECTED GRAPH */

    /* DRAWING HELPERS */
    private static void DrawArrowLine(Graphics graphics, Pen pen, Vector2 start,
     Vector2 tip, float arrowAngle = 35f)
    {


        Arrow(graphics, pen, arrowAngle, tip);
    }
    private static void Arrow(Graphics graphics, Pen pen, float angle,
     Vector2 tip)
    {
        angle = 3.1416f * (180f - angle) / 180f;

        int tipX = (int)(tip.X);
        int tipY = (int)(tip.Y);

        int lx = tipX + (int)(15 * MathF.Cos(angle + 0.3f));
        int rx = tipX + (int)(15 * MathF.Cos(angle - 0.3f));
        int ly = tipY + (int)(15 * MathF.Sin(angle + 0.3f));
        int ry = tipY + (int)(15 * MathF.Sin(angle - 0.3f));

        graphics.DrawLine(pen, lx, ly, tipX, tipY);
        graphics.DrawLine(pen, tipX, tipY, rx, ry);
    }
    private static void DrawLine(Graphics graphics, Pen pen, Vector2 start,
    Vector2 tip)
    {
        
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
            this.Invalidate(); // аналог InvalidateRect — перемальовує вікно
        };

        return switchBtn;
    }
}