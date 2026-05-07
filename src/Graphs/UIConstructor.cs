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

        DrawMatrixData(graphics, origin, "Матриця напрямленного графу", adirMatrixData, pen);
        /*
        // DIRECTED MATRIX
        DrawMatrix(graphics, origin, "Directed matrix", adirMatrixData.matrix, cellSize); // matrix

        int newX = origin.X + adirMatrixData.matrix.GetLength(0) * cellSize + cellSize;
        Point newOrigin = new Point(newX, origin.Y);
        // degrees
        string vertDegLabel = "Vertex degrees";
        DrawArray(graphics, newOrigin, vertDegLabel, "vertex", adirMatrixData.vertexDegrees); // vertex degrees
        newOrigin.X += (int) graphics.MeasureString(vertDegLabel, defaultFont).Width + cellSize;

        string vertInDegLabel = "Vertex In-degrees";
        DrawArray(graphics, newOrigin, vertInDegLabel, "vertex", adirMatrixData.vertexInDegrees); // vertex IN degrees
        newOrigin.X += (int)graphics.MeasureString(vertInDegLabel, defaultFont).Width + cellSize;

        string vertOutDegLabel = "Vertex Out-degrees";
        DrawArray(graphics, newOrigin, vertOutDegLabel, "vertex", adirMatrixData.vertexOutDegrees); // vertex OUT degrees
        newOrigin.X += (int)graphics.MeasureString(vertOutDegLabel, defaultFont).Width + cellSize;
        // regular matrix?
        // isolated/peak/regulat vertex


        // UNDIRECTED
        int newY = origin.Y + graphData.vertexCount * cellSize + 40;
        Point origin2 = new Point(origin.X, newY);

        DrawMatrix(graphics, origin2, "Undirected matrix", aundirMatrixData.matrix); // matrix

        origin2.X += aundirMatrixData.matrix.GetLength(0) * cellSize + cellSize;

        DrawArray(graphics, origin2, vertDegLabel, "vertex", aundirMatrixData.vertexDegrees); // vertex degrees
        origin2.X += (int)graphics.MeasureString(vertDegLabel, defaultFont).Width + cellSize;

        DrawArray(graphics, origin2, vertInDegLabel, "vertex", aundirMatrixData.vertexInDegrees); // vertex IN degrees
        origin2.X += (int)graphics.MeasureString(vertInDegLabel, defaultFont).Width + cellSize;

        DrawArray(graphics, origin2, vertOutDegLabel, "vertex", aundirMatrixData.vertexOutDegrees); // vertex OUT degrees
        newOrigin.X += (int)graphics.MeasureString(vertOutDegLabel, defaultFont).Width + cellSize;
        */

    }
    
    /* MATRIX */
    public static void DrawMatrixData(Graphics graphics, Point origin, string label,
        MatrixData matrixData, Pen pen = null, Font font = null, int cellSize = 20)
    {
        if (pen == null)
            pen = new Pen(Color.Black, 2);
        if (font == null)
            font = defaultFont;

        DrawMatrix(graphics, origin, label, matrixData.matrix, cellSize); // matrix

        int newX = origin.X + matrixData.matrix.GetLength(0) * cellSize + cellSize;
        Point newOrigin = new Point(newX, origin.Y);
        // degrees
        string vertDegLabel = "Степені вершин";
        DrawArray(graphics, newOrigin, vertDegLabel, "вершина", matrixData.vertexDegrees); // vertex degrees
        newOrigin.X += (int)graphics.MeasureString(vertDegLabel, font).Width + cellSize;

        string vertInDegLabel = "Напівстепені виходу";
        DrawArray(graphics, newOrigin, vertInDegLabel, "вершина", matrixData.vertexInDegrees); // vertex IN degrees
        newOrigin.X += (int)graphics.MeasureString(vertInDegLabel, font).Width + cellSize;

        string vertOutDegLabel = "Напівстепені виходу";
        DrawArray(graphics, newOrigin, vertOutDegLabel, "вершина", matrixData.vertexOutDegrees); // vertex OUT degrees
        newOrigin.X += (int)graphics.MeasureString(vertOutDegLabel, font).Width + cellSize;

        // regular degree
        string regularnessLable = "Граф є однорідним?";
        graphics.DrawString($"{regularnessLable}", font, defaultBrush, newOrigin);
        string answer = matrixData.isRegular ? "Так" : "Ні";
        graphics.DrawString($"{answer}", font, defaultBrush, newOrigin.X, newOrigin.Y + cellSize);

        if (matrixData.isRegular)
        {
            string regularDegree = matrixData.regularDegree.Value.ToString();
            graphics.DrawString($"Степінь однорідності: {regularDegree}", 
            defaultFont, defaultBrush, newOrigin.X, newOrigin.Y + cellSize * 2);
        }
    }

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

            graphics.DrawString($"{element} {x + 1}: {array[x]}", defaultFont, defaultBrush, origin.X, py);
        }
    }
}