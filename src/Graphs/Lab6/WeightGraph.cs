using System;
using System.Collections.Generic;
using System.Drawing;

public class WeightGraph
{
    public static void DrawWeightGraph(List<Vertex> vertices,
        Graphics graphics, Size clientSize, bool directed = true,
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

        GraphConstructor.PositionVertices(vertices, clientSize, vertexOffset);
        DrawEdges(vertices, graphics, pen, vertexRadius, directed);
    }

    public static void DrawEdges(List<Vertex> vertices, Graphics graphics, Pen pen,
        int vertexRadius, bool directed)
    {
        Font weightFont = new Font("Courier Bold", 15);
        SolidBrush weightBrush = new SolidBrush(Color.Purple);

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

                    Point? intersection = GraphConstructor.LineIntersectsCircle(start, end, vertices[z]);
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
                    GraphConstructor.DrawBrokenArrow(graphics, pen, start, breaks, end, GraphConstructor.arrowPen);
                else
                    GraphConstructor.DrawBrokenLine(graphics, pen, start, breaks, end);

                if (vertices[x].nextV.TryGetValue(v, out double weight))
                {
                    float dx = end.X - start.X, dy = end.Y - start.Y;
                    float len = MathF.Sqrt(dx * dx + dy * dy);
                    float nx = dx / len, ny = dy / len;

                    string text = weight.ToString();
                    SizeF sz = graphics.MeasureString(text, weightFont);

                    graphics.DrawString(text, weightFont, weightBrush,
                        start.X + nx * 20 - sz.Width / 2,
                        start.Y + ny * 20 - sz.Height / 2);

                    graphics.DrawString(text, weightFont, weightBrush,
                        end.X - nx * 20 - sz.Width / 2,
                        end.Y - ny * 20 - sz.Height / 2);
                }
            }
        }
    }
}