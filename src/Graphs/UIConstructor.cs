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
    /* FUNCTIONS FOR DRAWING FIRST GRAPH  */
    public static void FuckThis(ref Action<Graphics> currentDraw, Point origin, 
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

        MatrixData dirMatrixData = graphData.adirMatrixData;
        MatrixData undirMatrixData = graphData.aundirMatrixData;

        DrawMatrixData(graphics, origin, "Матриця напрямленного графу", dirMatrixData, pen);

        int newY = origin.Y + graphData.vertexCount * cellSize + 40;
        Point newOrigin = new Point(origin.X, newY);

        DrawMatrixData(graphics, newOrigin, "Матриця ненапрямленного графу", undirMatrixData, pen);
    }
    /* FUNCTIONS FOR DRAWING SECOND GRAPH */
    public static void DrawDirGraphDataAction(ref Action<Graphics> currentDraw, Point origin,
        GraphData graphData, Pen pen = null, int cellSize = 20)
    {
        if (pen == null)
        {
            pen = new Pen(Color.Black, 2);
        }

        currentDraw = (graphics) => DrawDirGraphData(graphics, origin, graphData, pen, cellSize);
    }
    private static void DrawDirGraphData(Graphics graphics, Point origin, GraphData graphData, Pen pen = null, int cellSize = 20)
    {
        if (pen == null)
        {
            pen = new Pen(Color.Black, 2);
        }

        MatrixData dirMatrixData = graphData.adirMatrixData;

        DrawMatrix(graphics, origin, "Матриця напрямленного графу", dirMatrixData.matrix, cellSize);
        int newX = origin.X + dirMatrixData.matrix.GetLength(0) * cellSize + cellSize;
        origin.X += newX;

        // degrees
        string vertDegLabel = "Степені вершин";
        DrawArray(graphics, origin, vertDegLabel, "вершина", dirMatrixData.vertexDegrees); // vertex degrees
        origin.X += (int)graphics.MeasureString(vertDegLabel, defaultFont).Width + cellSize;

        string vertInDegLabel = "Напівстепені виходу";
        DrawArray(graphics, origin, vertInDegLabel, "вершина", dirMatrixData.vertexInDegrees); // vertex IN degrees
        origin.X += (int)graphics.MeasureString(vertInDegLabel, defaultFont).Width + cellSize;

        string vertOutDegLabel = "Напівстепені виходу";
        DrawArray(graphics, origin, vertOutDegLabel, "вершина", dirMatrixData.vertexOutDegrees); // vertex OUT degrees
        origin.X += (int)graphics.MeasureString(vertOutDegLabel, defaultFont).Width + cellSize;
        // PATHS LENGTH 2
        string pathsLabel2 = "шляхи довжини 2";
        List<string> pathsLength2 = MatrixOperations.FindPaths(dirMatrixData.matrix, 2);
        DrawList(graphics, origin, "шляхи довжини 2", "", pathsLength2);
        /*
        int maxWidth1 = 0;
        for (int x = 0; x < pathsLength2.Count; x++)
        {
            int currentWidth = (int)graphics.MeasureString(pathsLength2[x], defaultFont).Width;
            if (currentWidth > maxWidth1)
                maxWidth1 = currentWidth + 20;
        }
        */
        origin.X += (int)graphics.MeasureString(pathsLabel2, defaultFont).Width;
        // PATHS LENGTH 3
        string pathsLabel3 = "шляхи довжини 3";
        List<string> pathsLength3 = MatrixOperations.FindPaths(dirMatrixData.matrix, 3);
        DrawList(graphics, origin, pathsLabel3, "", pathsLength3);
        /*
        int maxWidth2 = 0;
        for (int x = 0; x < pathsLength2.Count; x++)
        {
            int currentWidth = (int)graphics.MeasureString(pathsLength2[x], defaultFont).Width;
            if (currentWidth > maxWidth2)
                maxWidth2 = currentWidth + 20;
        }
        */
        origin.X += (int)graphics.MeasureString(pathsLabel3, defaultFont).Width;


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

        newOrigin.X += (int)graphics.MeasureString(regularnessLable, font).Width + cellSize;

        string isolatedLable = "Ізольвані/висячі вершини";
        DrawArray(graphics, newOrigin, isolatedLable, "вершина", matrixData.isolatedVerts);
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
    /* LIST */
    public static void DrawList<X>(Graphics graphics, Point origin,
    string label, string element, List<X> list, int cellSize = 20)
    {
        graphics.DrawString(label, defaultFont, defaultBrush, origin.X, origin.Y); // DRAW LABEL

        for (int x = 0; x < list.Count; x++)
        {
            int py = origin.Y + 20 + x * cellSize;

            graphics.DrawString($"{element} {x + 1}: {list[x]}", defaultFont, defaultBrush, origin.X, py);
        }
    }
}