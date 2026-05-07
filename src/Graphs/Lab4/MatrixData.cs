using System;

public class MatrixData
{
    public double[,] matrix;

    public int[] vertexDegrees;
    public int[] vertexOutDegrees;
    public int[] vertexInDegrees;

    public bool isRegular;
    public int? regularDegree;

    public string[] isolatedVerts;

    public MatrixData() { }
    public MatrixData(double[,] matrix)
    {
        this.matrix = matrix;
        ComputeStats();
    }
    public void ComputeStats()
    {
        if (matrix == null)
        {
            throw new InvalidOperationException("matrix is null");
        }
        
        (vertexDegrees, vertexInDegrees, vertexOutDegrees) = VertexDegreeArray(matrix);

        isRegular = IsRegular(matrix, out regularDegree);

        isolatedVerts = IsolatedVerts(matrix);
    }
    /* VERTEX DEGREES */
    // full degrees
    public static (int[] vertexDegrees, int[] vertexInDegrees, int[] vertexOutDegrees) VertexDegreeArray(double[,] matrix)
    {
        int n = matrix.GetLength(0);

        int[] vertexDegrees = new int[n];
        int[] vertexInDegrees = new int[n];
        int[] vertexOutDegrees = new int[n];

        for (int x = 0; x < n; x++)
        {
            (vertexDegrees[x], vertexInDegrees[x], vertexOutDegrees[x]) = GetVertexDegree(x, matrix);
        }

        return (vertexDegrees, vertexInDegrees, vertexOutDegrees);
    }
    public static (int degree, int inDegree, int outDegree) GetVertexDegree(int vertexIndex, double[,] matrix)
    {
        int inDegree = GetVertexInDegree(vertexIndex, matrix);
        int outDegree = GetVertexOutDegree(vertexIndex, matrix);

        return (inDegree + outDegree, inDegree, outDegree);
    }
    // out-degree
    public static int GetVertexInDegree(int vertexIndex, double[,] matrix)
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
    public static int GetVertexOutDegree(int vertexIndex, double[,] matrix)
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
    public static bool IsRegular(double[,] matrix, out int? degree)
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

    /* HANGING/ISOLATED VERTICES */
    // here we return a dictionary that we can assign to smth and later draw
    public static string[] IsolatedVerts(double[,] matrix)
    {
        int n = matrix.GetLength(0);
        string[] results = new string[n];

        for (int x = 0; x < n; x++)
        {
            int outDegree = GetVertexOutDegree(x, matrix);
            int inDegree = GetVertexInDegree(x, matrix);

            if (outDegree == 0 && inDegree == 0)
            {
                results[x] = "hanging";
            }
            else if ((outDegree == 1 && inDegree == 0) || (outDegree == 0 && inDegree == 1))
            {
                results[x] = "isolated";
            }
            else
                results[x] = "regular";
        }

        return results;
    }
}