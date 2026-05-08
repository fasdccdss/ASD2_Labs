using System;
using System.Collections.Generic;
using System.Linq;

public static class MatrixOperations
{
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
                results[x] = "висяча";
            }
            else if ((outDegree == 1 && inDegree == 0) || (outDegree == 0 && inDegree == 1))
            {
                results[x] = "ізольована";
            }
            else
                results[x] = "звичайна";
        }

        return results;
    }
    /* FINDING MATRIX PATH WITH SPECIFIC LENGTH */
    
    public static List<string> FindPaths(double[,] matrix, int pathLength)
    {
        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);

        if (pathLength > rows || pathLength > cols)
            throw new Exception("path can't be longer than matrices row/column count");

        double[,] powered = Power(matrix, pathLength);
        List<string> paths = new List<string>(); // ← initialize once

        for (int x = 0; x < rows; x++)
            for (int y = 0; y < cols; y++)
                if (powered[x, y] > 0)
                    TracePaths(matrix, x, y, pathLength, new List<int> { x }, paths);

        return paths;
    }
    
    private static List<string> TracePaths(double[,] matrix, int current, int target,
        int stepsLeft, List<int> path, List<string> results)
    {
        if (stepsLeft == 0)
        {
            if (current == target)
                results.Add(string.Join(" -> ", path.Select(v => v + 1)));
            return results;
        }

        int n = matrix.GetLength(0);
        for (int next = 0; next < n; next++)
        {
            if (matrix[current, next] == 1)
            {
                path.Add(next);
                TracePaths(matrix, next, target, stepsLeft - 1, path, results);
                path.RemoveAt(path.Count - 1);
            }
        }

        return results;
    }
    /* power */
    public static double[,] Power(double[,] matrix, int power)
    {
        double[,] result = (double[,])matrix.Clone();
        
        for (int x = 1; x < power; x++)
        {
            result = Multiply(result, matrix);
        }

        return result;
    }
    /* multiply */
    public static double[,] Multiply(double[,] matrixA, double[,] matrixB)
    {
        int rows = matrixA.GetLength(0);
        int cols = matrixB.GetLength(1);
        int inner = matrixA.GetLength(1);

        double[,] result = new double[rows, cols];

        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < cols; y++)
            {
                for (int k = 0; k < inner; k++)
                    result[x, y] += matrixA[x, k] * matrixB[k, y];
            }
        }

        return result;
    }

    /* CALCULATING REACHABILITY MATRIX */
    public static double[,] ReachabilityMatrix(double[,] matrix)
    {
        int n = matrix.GetLength(0);
        double[,] result = (double[,])matrix.Clone();

        // each vertex reaches itself
        for (int i = 0; i < n; i++)
            result[i, i] = 1;

        // orshals algorithm
        for (int k = 0; k < n; k++)
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    if (result[i, k] == 1 && result[k, j] == 1)
                        result[i, j] = 1;

        return result;
    }

    /* STRONG CONNECTIVITY MATRIX */
    public static double[,] StrongConnectivity(double[,] reachabilityMatrix)
    {
        int n = reachabilityMatrix.GetLength(0);
        double[,] result = new double[n, n];

        for (int i = 0; i < n; i++)
            for (int j = 0; j < n; j++)
                if (reachabilityMatrix[i, j] == 1 && reachabilityMatrix[j, i] == 1)
                    result[i, j] = 1;

        return result;
    }
    public static List<string> StrongComponentsList(double[,] strongConnectivityMatrix)
    {
        int n = strongConnectivityMatrix.GetLength(0);
        bool[] visited = new bool[n];
        List<string> components = new List<string>();

        for (int i = 0; i < n; i++)
        {
            if (visited[i]) continue;

            List<int> component = new List<int>();
            for (int j = 0; j < n; j++)
                if (strongConnectivityMatrix[i, j] == 1)
                {
                    component.Add(j + 1);
                    visited[j] = true;
                }

            components.Add("{ " + string.Join(", ", component) + " }");
        }

        return components;
    }
    public static int[] StrongComponenetsArray(double[,] strongConnectivityMatrix)
    {
        int n = strongConnectivityMatrix.GetLength(0);
        bool[] visited = new bool[n];
        int[] vertexComponent = new int[n];
        int componentIndex = 0;

        for (int i = 0; i < n; i++)
        {
            if (visited[i]) continue;
            for (int j = 0; j < n; j++)
                if (strongConnectivityMatrix[i, j] == 1)
                {
                    vertexComponent[j] = componentIndex;
                    visited[j] = true;
                }
            componentIndex++;
        }

        return vertexComponent;
    }
    /* CONDENSATION MATRIX */
    public static double[,] CondensationMatrix(double[,] matrix, double[,] strongConnectivityMatrix)
    {
        int[] vertexComponent = StrongComponenetsArray(strongConnectivityMatrix);
        int count = vertexComponent.Max() + 1;
        double[,] condensation = new double[count, count];

        for (int i = 0; i < matrix.GetLength(0); i++)
            for (int j = 0; j < matrix.GetLength(1); j++)
                if (matrix[i, j] == 1 && vertexComponent[i] != vertexComponent[j])
                    condensation[vertexComponent[i], vertexComponent[j]] = 1;

        return condensation;
    }
}