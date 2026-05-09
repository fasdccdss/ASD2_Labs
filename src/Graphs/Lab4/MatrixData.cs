using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;

public class MatrixData
{
    List<Vertex> vertices;


    public double[,] matrix;
    public double[,] reachabilityMatrix;
    public double[,] strongConnectivityMatrix;
    public double [,] condensationMatrix;

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
        
        (vertexDegrees, vertexInDegrees, vertexOutDegrees) = MatrixOperations.VertexDegreeArray(matrix);

        isRegular = MatrixOperations.IsRegular(matrix, out regularDegree);

        isolatedVerts = MatrixOperations.IsolatedVerts(matrix);

        reachabilityMatrix = MatrixOperations.ReachabilityMatrix(matrix);
        strongConnectivityMatrix = MatrixOperations.StrongConnectivity(matrix);
        condensationMatrix = MatrixOperations.CondensationMatrix(matrix, strongConnectivityMatrix);
    }

}