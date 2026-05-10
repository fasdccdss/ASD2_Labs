using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Imaging;

public class GraphConstructor
{
    /* 
    to build full graph information we need to scan the adjacency matrix and build 
    vertices from this matrix, those vertices would also store information about 
    what vertices they are pointing at 
    */ 

    /* DRAWING */
    public static void DrawGraph(List<Vertex> vertices, Graphics graphics,
        int vertexRadius = 60, Font font = null, SolidBrush solidBrush = null, Pen pen = null)
    {
        if (font == null)
            font = new Font("Arial", vertexRadius / 2);
        if (solidBrush == null)
            solidBrush = new SolidBrush(Color.Black);
        if (pen == null)
            pen = new Pen(Color.Black, 2);
        
        Dictionary<Color, SolidBrush> fillBrushs = new Dictionary<Color, SolidBrush>();
        Dictionary<Color, SolidBrush> fontBrushs = new Dictionary<Color, SolidBrush>();

        for (int x = 0; x < vertices.Count; x++)
        {
            Color fillColor = vertices[x].FillColor();
            if (!fillBrushs.ContainsKey(fillColor))
            {
                fillBrushs.Add(fillColor, new SolidBrush(fillColor));
            }

            Color fontColor = vertices[x].FontColor();
            if (!fontBrushs.ContainsKey(fontColor))
            {
                fontBrushs.Add(fontColor, new SolidBrush(fontColor));
            }

            graphics.FillEllipse(fillBrushs[fillColor], vertices[x].center.X - vertexRadius, vertices[x].center.Y - vertexRadius,
                2 * vertexRadius, 2 * vertexRadius);
            
            graphics.DrawEllipse(pen, vertices[x].center.X - vertexRadius, vertices[x].center.Y - vertexRadius,
                2 * vertexRadius, 2 * vertexRadius);

            SizeF textSize = graphics.MeasureString(vertices[x].index.ToString(), font);
            graphics.DrawString(vertices[x].index.ToString(), font, fontBrushs[fontColor],
                vertices[x].center.X - textSize.Width / 2,
                vertices[x].center.Y - textSize.Height / 2);
        }
    }

    /* BUILDING PROPER VERTEX DATA */
    public static List<Vertex> BuildVertexData(double[,] matrix)
    {
        int vertexCount = matrix.GetLength(0);

        List<Vertex> vertices = new List<Vertex>(vertexCount);

        for (int x = 0; x < vertexCount; x++)
        {
            vertices.Add(new Vertex(x + 1));
        }

        for (int x = 0; x < vertexCount; x++)
        {
            for (int y = 0; y < vertexCount; y++)
            {
                if (matrix[x, y] == 1)
                {
                    vertices[x].next.Add(vertices[y]);
                    vertices[y].previous.Add(vertices[x]);
                }
            }
        }
        
        return vertices;
    }
    public static List<Vertex> BuildVertexData(double[,] matrix, Size clientSize, 
        int vertexOffset = 100)
    {
        int vertexCount = matrix.GetLength(0);

        List<Vertex> vertices = new List<Vertex>(vertexCount);

        for (int x = 0; x < vertexCount; x++)
        {
            vertices.Add(new Vertex(x + 1));
        }

        for (int x = 0; x < vertexCount; x++)
        {
            for (int y = 0; y < vertexCount; y++)
            {
                if (matrix[x, y] == 1)
                {
                    vertices[x].next.Add(vertices[y]);
                    vertices[y].previous.Add(vertices[x]);
                }
            }
        }

        PositionVertices(vertices, clientSize, vertexOffset);

        return vertices;
    }

    /* VERTEX POSITIONING */
    public static void PositionVertices(List<Vertex> vertices, Size clientSize, int offset)
    {
        // to support any graph length we have to add 3 to acount for
        //  the future deduction of the edges ((rRows-1) + (dColumns-1) + (lRows-2) + 1 = edgeCount - 3)
        int edgeCount = vertices.Count + 3; 

        int maxRows = 0;
        int lRows = 0;
        int rRows = 0;

        int maxColumns = 0;
        int uColumns = 0;
        int dColumns = 0;

        bool increaseRows = false; // determines if vertices need to be added to rows or columns
        bool l = true;

        // finding count of edge vertices in rows & columns
        for (int x = 0; x < edgeCount; x++)
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
        // calculating vertices positions
        int graphWidth = maxColumns * offset;
        int graphHeight = maxRows * offset;

        int centerX = clientSize.Width / 2;
        int centerY = clientSize.Height / 2;

        int left = centerX - graphWidth / 2;
        int top = centerY - graphHeight / 2;

        int right = left + graphWidth;
        int bottom = top + graphHeight;

        int vertIndex = 0;

        // positioning vertices
        // top edge
        for (int x = 0; x < uColumns; x++) // here x is 0 because we need to place the first corner
            vertices[vertIndex++].center = new Point(left + x * graphWidth / (uColumns - 1), top);
        // right edge
        for (int x = 1; x < rRows; x++)
            vertices[vertIndex++].center = new Point(right, top + x * graphHeight / (rRows - 1));
        // bottom edge
        for (int x = 1; x < dColumns; x++)
            vertices[vertIndex++].center = new Point(right - x * graphWidth / (dColumns - 1), bottom);
        // left edge
        for (int x = 1; x < lRows - 1; x++) // deduct 1(last edge) to prevent array overpopulation
            vertices[vertIndex++].center = new Point(left, bottom - x * graphHeight / (lRows - 1));
        // center vertex
        vertices[vertIndex++].center = new Point(centerX, centerY);
    }
}