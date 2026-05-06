using System;
using System.CodeDom;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

public static class UIConstructor
{
    public static Color highlightColor = Color.PaleGreen;
    public static Color unactiveColor = Color.WhiteSmoke;

    public static Button BuildButton(string label, Point location, Size size, Action onClick)
    {
        Button btn = new Button();
        btn.Text = label;
        btn.Location = location;
        btn.Size = size;
        btn.BackColor = unactiveColor;
        btn.Click += (s, e) => onClick();
        return btn;
    }
    public static void GraphToggle(ref Action<Graphics> currentDraw, Control parent, Pen pen, MatrixData matrixData)
    {
        matrixData.directed = !matrixData.directed;

        double[,] matrix = matrixData.directed == true ? matrixData.adirMatrix : matrixData.aundirMatrix;

        currentDraw = (graphics) => GraphWindow.DrawGraph(graphics, parent.ClientSize, pen,
        matrixData.vertexCount, matrix, matrixData.directed);
    } 
}