using System.Collections.Generic;

public class GraphData
{
    public MatrixData matrixData;

    public List<Vertex> vertices;
    public int vertexRadius; 

    public GraphData(int vertexRadius)
    {
        this.vertexRadius = vertexRadius;
    }
}