using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

public static class UIConstructor
{
    public static Color highlightColor = Color.PaleGreen;
    public static Color unactiveColor = Color.WhiteSmoke;
    private static readonly Font defaultFont = new Font("Arial", 9);
    private static readonly Brush defaultBrush = Brushes.Black;

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
    public static void GraphToggle(ref Action<Graphics> currentDraw, Control parent, Pen pen, GraphData graphData)
    {
        graphData.directed = !graphData.directed;

        double[,] matrix = graphData.directed ? graphData.adirMatrix.matrix : graphData.aundirMatrix.matrix;

        currentDraw = (graphics) => GraphWindow.DrawGraph(graphics, parent.ClientSize, pen,
        graphData.vertexCount, matrix, graphData.directed);
    } 
    public static void DrawMatrixData(Graphics graphics, Point origin, GraphData matrixData, Pen pen = null, int cellSize = 20)
    {
        if (pen == null)
        {
            pen = new Pen(Color.Black, 2);
        }
        
        // DIRECTED MATRIX
        DrawMatrix(graphics, origin, "Directed matrix", matrixData.adirMatrix.matrix, cellSize); // matrix

        Point currentOrigin1 = new Point(origin.X + matrixData.adirMatrix.matrix.GetLength(0) * cellSize + cellSize, origin.Y);
        DrawArray(graphics, currentOrigin1, "Directed vertices degrees", "vertex", matrixData.adirMatrix.vertexDegrees); // vertex degrees
        // UNDIRECTED
        Point origin2 = new Point(origin.X, origin.Y + matrixData.vertexCount * cellSize);
        DrawMatrix(graphics, origin2, "Undirected matrix", matrixData.aundirMatrix.matrix); // matrix

        origin2 = new Point(origin2.X - matrixData.aundirMatrix.matrix.GetLength(0) * cellSize + cellSize, origin2.Y);
        
    }
    /* MATRIX */
    public static void DrawMatrix<X>(Graphics graphics, Point origin, 
    string label, X[,] matrix, int cellSize = 20)
    {
        graphics.DrawString(label, defaultFont, defaultBrush, origin.X, origin.Y); // DRAW LABEL

        for (int x = 0; x < matrix.GetLength(0); x++)
        {
            for (int y = 0; y < matrix.GetLength(1); y++)
            {
                int px = origin.X + y * cellSize;
                int py = origin.Y + 20 + x * cellSize;

                graphics.DrawString(matrix[x, y].ToString(), defaultFont, defaultBrush, px, py);
            }
        }
    }
    /* DICTIONARY */
    public static void DrawDictionary<X, Y>(Graphics graphics, Point origin,
    string label, Dictionary<X, Y> dictionary, Pen pen, int cellSize = 20)
    {
        graphics.DrawString(label, defaultFont, defaultBrush, origin.X, origin.Y); // DRAW LABEL
    }
    /* ARRAY */
    public static void DrawArray<X>(Graphics graphics, Point origin,
    string label, string element, X[] array, int cellSize = 20)
    {
        graphics.DrawString(label, defaultFont, defaultBrush, origin.X, origin.Y); // DRAW LABEL

        for (int x = 0; x < array.Length; x++)
        {
            int py = origin.Y + 20 + x * cellSize;

            graphics.DrawString($"{element}{x + 1}: {array[x]}", defaultFont, defaultBrush, origin.X, py);
        }
    }
}