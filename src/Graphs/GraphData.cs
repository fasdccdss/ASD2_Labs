using System.Collections.Generic;

public class GraphData
{
    public MatrixData matrixData;

    public List<Vertex> vertices;
    public int vertexRadius; 

    public GraphData(int vertexRadius)
    {
        this.vertexRadius = vertexRadius;
        BuildVertexData(this);
    }

    public static void BuildVertexData(GraphData graphData)
    {
        int vertexCount = graphData.matrixData.matrix.GetLength(0);

        graphData.vertices = new List<Vertex>(vertexCount);
        List<Vertex> vertices = graphData.vertices;

        for (int x = 0; x < vertexCount; x++)
        {
            vertices.Add(new Vertex(x + 1));
        }

        for (int x = 0; x < vertexCount; x++)
        {
            for (int y = 0; y < vertexCount; y++)
            {
                if (graphData.matrixData.matrix[x, y] == 1)
                {
                    vertices[x].next.Add(vertices[y]);
                    vertices[y].previous.Add(vertices[x]);
                }
            }
        }
    }
}