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

        double[,] matrix = graphData.directed ? graphData.adirMatrixData.matrix : graphData.aundirMatrixData.matrix;

        currentDraw = (graphics) => GraphWindow.DrawGraph(graphics, parent.ClientSize, pen,
        graphData.vertexCount, matrix, graphData.directed);
    } 
    /* MATRIX */
    public static void FuckThis(ref Action<Graphics> currentDraw, Control parent, Point origin, 
    GraphData graphData, Pen pen = null, int cellSize = 20)
    {
        if (pen == null)
        {
            pen = new Pen(Color.Black, 2);
        }

        currentDraw = (graphics) => DrawGraphData(graphics, origin, graphData, pen, cellSize);
    }
    public static void DrawGraphData(Graphics graphics, Point origin, GraphData graphData, Pen pen = null, int cellSize = 20)
    {
        if (pen == null)
        {
            pen = new Pen(Color.Black, 2);
        }

        MatrixData adirMatrixData = graphData.adirMatrixData;
        MatrixData aundirMatrixData = graphData.aundirMatrixData;

        // DIRECTED MATRIX
        DrawMatrix(graphics, origin, "Directed matrix", adirMatrixData.matrix, cellSize); // matrix

        int newX = origin.X + adirMatrixData.matrix.GetLength(0) * cellSize + cellSize;
        Point newOrigin = new Point(newX, origin.Y);

        string vertDegLabel = "Vertex degrees";
        DrawArray(graphics, newOrigin, vertDegLabel, "vertex", adirMatrixData.vertexDegrees); // vertex degrees
        newOrigin.X += (int) graphics.MeasureString(vertDegLabel, defaultFont).Width;

        string vertInDegLabel = "Vertex In-degrees";
        DrawArray(graphics, newOrigin, vertInDegLabel, "vertex", adirMatrixData.vertexInDegrees); // vertex IN degrees
        newOrigin.X += 40;

        DrawArray(graphics, newOrigin, "Vertex Out-degrees", "vertex", adirMatrixData.vertexOutDegrees); // vertex OUT degrees
        newOrigin.X += 40;


        // UNDIRECTED
        int newY = origin.Y + graphData.vertexCount * cellSize + 40;
        Point origin2 = new Point(origin.X, newY);

        DrawMatrix(graphics, origin2, "Undirected matrix", aundirMatrixData.matrix); // matrix

        origin2.X += aundirMatrixData.matrix.GetLength(0) * cellSize + cellSize;

        DrawArray(graphics, origin2, "Vertex degrees", "vertex", aundirMatrixData.vertexDegrees); // vertex degrees
        origin2.X += 40;

        DrawArray(graphics, origin2, "Vertex In-degrees", "vertex", aundirMatrixData.vertexInDegrees); // vertex IN degrees
        origin2.X += 40;

        DrawArray(graphics, origin2, "Vertex Out-degrees", "vertex", aundirMatrixData.vertexOutDegrees); // vertex OUT degrees
        newOrigin.X += 40;

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