using System;
using System.Drawing;
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
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        Graphics g = e.Graphics;


    }

    private void Arrow(float fi, int px, int py)
    {
        fi = 3.1416f * (180f - fi) / 180f;

        int lx = px + (int)(15 * MathF.Cos(fi + 0.3f));
        int rx = px + (int)(15 * MathF.Cos(fi - 0.3f));
        int ly = py + (int)(15 * MathF.Sin(fi + 0.3f));
        int ry = py + (int)(15 * MathF.Sin(fi - 0.3f));


    }

    static void Main()
    {
        Application.Run(new GraphWindow());
    }
}