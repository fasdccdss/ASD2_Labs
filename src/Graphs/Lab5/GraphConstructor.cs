
using System.Collections.Generic;
using System.Windows.Forms;

public class GraphConstructor
{
    /* 
    to build full graph information we need to scan the adjacency matrix and build
    vertices from this matrix, those vertices would also store information about 
    what vertices they are pointing at
    */
    public static GraphData BuildGraphFromMatrixData(MatrixData matrixData)
    {
        GraphData graph = new GraphData();

        int vertexCount = matrixData.matrix.GetLength(0);
        List<Vertex> vertices = new List<Vertex>(vertexCount);

        for (int x = 0; x < vertexCount; x++)
        {
            vertices.Add(new Vertex(x + 1));
        }

        for (int x = 0; x < vertexCount; x++)
        {
            for (int y = 0; y < vertexCount; y++)
            {
                if (matrixData.matrix[x, y] == 1)
                {
                    vertices[x].next.Add(vertices[y]);
                    vertices[y].previous.Add(vertices[x]);
                }
            }
        }
        
        return graph;
    }
}