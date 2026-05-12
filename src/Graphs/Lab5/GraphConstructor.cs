using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml.Serialization;

public class GraphConstructor
{
    /* 
    to build full graph information we need to scan the adjacency matrix and build 
    vertices from this matrix, those vertices would also store information about 
    what vertices they are pointing at 
    */

    public static Pen arrowPen = new Pen(Color.Red, 2);

    /* DRAWING */
    public static void DrawGraph(List<Vertex> vertices, Graphics graphics, Size clientSize, bool directed = true,
        int vertexRadius = 60, int vertexOffset = 90, Font font = null, SolidBrush solidBrush = null, Pen pen = null)
    {
        if (font == null)
            font = new Font("Arial", vertexRadius / 2);
        if (solidBrush == null)
            solidBrush = new SolidBrush(Color.Black);
        if (pen == null)
            pen = new Pen(Color.Black, 2);
        
        Dictionary<Color, SolidBrush> fillBrushes = new Dictionary<Color, SolidBrush>();
        Dictionary<Color, SolidBrush> fontBrushes = new Dictionary<Color, SolidBrush>();

        for (int x = 0; x < vertices.Count; x++)
        {
            Color fillColor = vertices[x].FillColor();
            if (!fillBrushes.ContainsKey(fillColor))
            {
                fillBrushes.Add(fillColor, new SolidBrush(fillColor));
            }

            Color fontColor = vertices[x].FontColor();
            if (!fontBrushes.ContainsKey(fontColor))
            {
                fontBrushes.Add(fontColor, new SolidBrush(fontColor));
            }

            graphics.FillEllipse(fillBrushes[fillColor], vertices[x].center.X - vertexRadius, vertices[x].center.Y - vertexRadius,
                2 * vertexRadius, 2 * vertexRadius);
            
            graphics.DrawEllipse(pen, vertices[x].center.X - vertexRadius, vertices[x].center.Y - vertexRadius,
                2 * vertexRadius, 2 * vertexRadius);

            SizeF textSize = graphics.MeasureString(vertices[x].index.ToString(), font);
            graphics.DrawString(vertices[x].index.ToString(), font, fontBrushes[fontColor],
                vertices[x].center.X - textSize.Width / 2,
                vertices[x].center.Y - textSize.Height / 2);
        }

        PositionVertices(vertices, clientSize, vertexOffset);
        DrawEdges(graphics, pen, vertices, vertexRadius, directed);
    }

    /* */
    public static void DrawGraphNoIndex(List<Vertex> vertices, Graphics graphics, Size clientSize, bool directed = true,
        int vertexRadius = 60, int vertexOffset = 90, SolidBrush solidBrush = null, Pen pen = null)
    {
        if (solidBrush == null)
            solidBrush = new SolidBrush(Color.Black);
        if (pen == null)
            pen = new Pen(Color.Black, 2);

        Dictionary<Color, SolidBrush> fillBrushes = new Dictionary<Color, SolidBrush>();
        Dictionary<Color, SolidBrush> fontBrushes = new Dictionary<Color, SolidBrush>();

        for (int x = 0; x < vertices.Count; x++)
        {
            Color fillColor = vertices[x].FillColor();
            if (!fillBrushes.ContainsKey(fillColor))
            {
                fillBrushes.Add(fillColor, new SolidBrush(fillColor));
            }

            Color fontColor = vertices[x].FontColor();
            if (!fontBrushes.ContainsKey(fontColor))
            {
                fontBrushes.Add(fontColor, new SolidBrush(fontColor));
            }

            graphics.FillEllipse(fillBrushes[fillColor], vertices[x].center.X - vertexRadius, vertices[x].center.Y - vertexRadius,
                2 * vertexRadius, 2 * vertexRadius);

            graphics.DrawEllipse(pen, vertices[x].center.X - vertexRadius, vertices[x].center.Y - vertexRadius,
                2 * vertexRadius, 2 * vertexRadius);
        }

        PositionVertices(vertices, clientSize, vertexOffset);
        DrawEdges(graphics, pen, vertices, vertexRadius, directed);
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

        return vertices;
    }
    // this method expects an undirectional matrix as first argument and
    // both matrices to be of the same size
    public static List<Vertex> BuildVertexData(double[,] matrix, double[,] weightMatrix)
    {
        int vertexCount = matrix.GetLength(0);

        List<Vertex> vertices = new List<Vertex>(vertexCount);

        for (int x = 0; x < vertexCount; x++)
        {
            vertices.Add(new Vertex(x + 1));
        }

        // because matrix is undirectional we want to prevent any duplicates in
        // nextV and previousV so we need to store values that we already cycled through and
        // skip adding anything to the mirrored values
        HashSet<(int row, int column)> cycledPairs = new HashSet<(int, int)>();

        for (int x = 0; x < vertexCount; x++)
        {
            for (int y = 0; y < vertexCount; y++)
            {
                if (matrix[x, y] == 1 && !cycledPairs.Contains((y, x)))
                {
                    vertices[x].nextV.Add(vertices[y], weightMatrix[x, y]);
                    vertices[y].previousV.Add(vertices[x], weightMatrix[y, x]);

                    cycledPairs.Add((x, y));
                }
            }
        }

        return vertices;
    }

    /* DRAWING EDGES */
    private static void DrawEdges(Graphics graphics, Pen pen, List<Vertex> vertices,
        int vertexRadius, bool directed)
    {
        for (int x = 0; x < vertices.Count; x++)
        {
            foreach (var v in vertices[x].next)
            {
                if (vertices[x] == v)
                {
                    graphics.DrawEllipse(pen,
                    vertices[x].center.X,
                    vertices[x].center.Y - vertexRadius * 2,
                    vertexRadius, vertexRadius);
                    continue;
                }

                float angle = MathF.Atan2(
                    v.center.Y - vertices[x].center.Y,
                    v.center.X - vertices[x].center.X
                );

                Point start = new Point(
                    vertices[x].center.X + (int)(vertexRadius * MathF.Cos(angle)),
                    vertices[x].center.Y + (int)(vertexRadius * MathF.Sin(angle))
                );
                Point end = new Point(
                    v.center.X - (int)(vertexRadius * MathF.Cos(angle)),
                    v.center.Y - (int)(vertexRadius * MathF.Sin(angle))
                );
                Point?[] breaks = new Point?[vertices.Count]; // type List<> suits the use case better 

                // find any possible intersection points and offset them
                // to store as a break
                for (int z = 0; z < vertices.Count; z++)
                {
                    if (z == x || z == v.index - 1) continue;

                    Point? intersection = LineIntersectsCircle(start, end, vertices[z]);
                    if (intersection.HasValue)
                    {
                        float ldx = end.X - start.X, ldy = end.Y - start.Y;
                        float crossZ = ldx * (vertices[z].center.Y - start.Y)
                                     - ldy * (vertices[z].center.X - start.X);
                        int side = crossZ > 0 ? 1 : -1;

                        // tangent point from start to the blocker circle
                        float cdx = vertices[z].center.X - start.X, cdy = vertices[z].center.Y - start.Y;
                        float D2 = cdx * cdx + cdy * cdy;
                        float D = MathF.Sqrt(D2);
                        float L2 = D2 - vertexRadius * vertexRadius;
                        float d = L2 / D;

                        float underSqrt = L2 - d * d;
                        if (underSqrt < 0) continue;

                        float h = MathF.Sqrt(underSqrt);

                        float tx = start.X + (cdx * d + side * (-cdy) * h) / D;
                        float ty = start.Y + (cdy * d + side * cdx * h) / D;

                        // small clearance margin
                        float nx = (tx - vertices[z].center.X) / vertexRadius;
                        float ny = (ty - vertices[z].center.Y) / vertexRadius;
                        breaks[z] = new Point(
                            (int)(tx + nx * 5),
                            (int)(ty + ny * 5)
                        );
                        break;
                    }
                }

                if (directed)
                    DrawBrokenArrow(graphics, pen, start, breaks, end, arrowPen);
                else
                    DrawBrokenLine(graphics, pen, start, breaks, end);
            }
        }
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

    private static void DrawBrokenArrow(Graphics graphics, Pen linePen,
        Point start, Point?[] breaks, Point end, Pen arrowPen = null)
    {
        if (arrowPen == null)
        {
            arrowPen = linePen;
        }

        Point lastPoint = start;
        for (int x = 0; x < breaks.Length; x++)
        {
            if (breaks[x] != null)
            {
                graphics.DrawLine(linePen, lastPoint, breaks[x].Value);
                lastPoint = breaks[x].Value;
            }
        }
        float arrowAngle = MathF.Atan2(end.Y - lastPoint.Y, end.X - lastPoint.X) * 180f / MathF.PI;

        graphics.DrawLine(linePen, lastPoint, end);
        ArrowHead(graphics, arrowPen, arrowAngle, end);
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
     Point tip, float arrowSize = 20)
    {
        angle = 3.1416f * (180f + angle) / 180f;

        int lx = tip.X + (int)(arrowSize * MathF.Cos(angle + 0.3f));
        int rx = tip.X + (int)(arrowSize * MathF.Cos(angle - 0.3f));
        int ly = tip.Y + (int)(arrowSize * MathF.Sin(angle + 0.3f));
        int ry = tip.Y + (int)(arrowSize * MathF.Sin(angle - 0.3f));

        graphics.DrawLine(pen, lx, ly, tip.X, tip.Y);
        graphics.DrawLine(pen, tip.X, tip.Y, rx, ry);
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