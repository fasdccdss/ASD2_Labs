using System;

public class SpanningTree
{
    static readonly double k = 1 - GraphParams.n3 * 0.01 - GraphParams.n4 * 0.005 - 0.05;

    static double[,] dirMatrix = GraphWindow.BuildAdirMatrix(GraphParams.seed, GraphParams.vertexCount, k);
    static double[,] undirMatrix = GraphWindow.BuildAundirMatrix(GraphParams.seed, GraphParams.vertexCount, k);

    static readonly double[,] B = BuildB(GraphParams.seed, GraphParams.vertexCount);
    static readonly double[,] C = BuildC(B);
    static readonly double[,] D = BuildD(C);
    static readonly double[,] H = BuildH(D);
    static readonly double[,] Tr = BuildTr(GraphParams.vertexCount);

    static double[,] W = BuildW(GraphParams.vertexCount);

    private static double[,] BuildW(int vertexCount)
    {
        double[,] W = new double[vertexCount, vertexCount];

        for (int x = 0; x < vertexCount; x++)
        {
            for (int y = 0; y < vertexCount; y++)
            {
                W[x, y] = W[y, x] = (D[x, y] + H[x, y] * Tr[x, y]) * C[x, y];
            }
        }

        return W;
    }
    private static double[,] BuildB(int seed, int vertexCount)
    {
        double[,] B = new double[vertexCount, vertexCount];
        Random rng = new Random(seed);

        for (int x = 0; x < vertexCount; x++)
        {
            for (int y = 0; y < vertexCount; y++)
            {
                double element = (rng.NextDouble() * 2.0f) * k;

                B[x, y] = element;
            }
        }

        return B;
    }
    private static double[,] BuildC(double[,] B)
    {
        int n = B.GetLength(0);
        double[,] C = new double[n, n];

        for (int x = 0; x < n; x++)
        {
            for (int y = 0; y < n; y++)
            {
                C[x, y] = Math.Ceiling(B[x, y] * 100 * undirMatrix[x, y]);
            }
        }

        return C;
    }
    private static double[,] BuildD(double[,] C)
    {
        int n = C.GetLength(0);
        double[,] D = new double[n, n];

        for (int x = 0; x < n; x++)
        {
            for (int y = 0; y < n; y++)
            {
                if (C[x, y] == 0)
                    D[x, y] = 0;
                else if (C[x, y] > 0)
                    D[x, y] = 1;
            }
        }

        return D;
    }
    private static double[,] BuildH(double[,] D)
    {
        int n = D.GetLength(0);
        double[,] H = new double[n, n];

        for (int x = 0; x < n; x++)
        {
            for (int y = 0; y < n; y++)
            {
                H[x, y] = D[x, y] == D[y, x] ? 1 : 0;
            }
        }

        return H;
    }
    private static double[,] BuildTr(int vertexCount)
    {
        double[,] Tr = new double[vertexCount, vertexCount];


        for (int x = 0; x < vertexCount; x++)
        {
            for (int y = 0; y < vertexCount; y++)
            {
                Tr[x, y] = x < y ? 1 : 0;
            }
        }

        return Tr;
    }
}