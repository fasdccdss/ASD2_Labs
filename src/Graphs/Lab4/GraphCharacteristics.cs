

using System.CodeDom;
using System.IO;

public class GraphCharacteristics
{
    static int n3 = GraphParams.n3;
    static int n4 = GraphParams.n4;

    static int vertexCount = GraphParams.vertexCount;

    static int seed = GraphParams.seed;

    static double k = 1.0 - n3 * 0.01 - n4 * 0.01 - 0.03;

    static double[,] adirMatrix = GraphWindow.BuildAdirMatrix(seed, vertexCount, k);
    static double[,] aundirMatrix = GraphWindow.BuildAundirMatrix(seed, vertexCount, k);

    /* VERTEX DEGREES */
    // full degrees
    public static int[] VertexDegreeArray(int[,] matrix)
    {
        int n = matrix.GetLength(0);

        int[] vertexDegrees = new int[n];

        for (int x = 0; x < n; x++)
        {
            vertexDegrees[x] = GetVertexDegree(x, matrix);
        }

        return vertexDegrees;
    }
    public static int GetVertexDegree(int vertexIndex, int[,] matrix)
    {
        return GetVertexInDegree(vertexIndex, matrix) + GetVertexOutDegree(vertexIndex, matrix);
    }
    // out-degree
    public static int GetVertexInDegree(int vertexIndex, int[,] matrix)
    {
        int outDegree = 0;

        for (int x = 0; x < matrix.GetLength(0); x++)
        {
            if (matrix[x, vertexIndex] == 1)
            {
                outDegree++;
            }
        }

        return outDegree;
    }
    // in-degree
    public static int GetVertexOutDegree(int vertexIndex, int[,] matrix)
    {
        int inDegree = 0;

        for (int y = 0; y < matrix.GetLength(1); y++)
        {
            if (matrix[vertexIndex, y] == 1)
            {
                inDegree++;
            }
        }

        return inDegree;
    }
    /* GRAPH REGULARNESS CHECK */
    public static bool isRegular(int[,] matrix, out int? degree)
    {
        int n = matrix.GetLength(0);

        int firstOut = GetVertexOutDegree(0, matrix);
        int firstIn = GetVertexInDegree(0, matrix);

        for (int x = 0; x < n; x++)
        {
            if (GetVertexOutDegree(x, matrix) != firstOut || GetVertexInDegree(x, matrix) != firstIn)
            {
                degree = null;
                return false;
            }
        }

        degree = firstOut;
        return true;
    }

    /* MATRIX TRANSFORMATIONS */
}