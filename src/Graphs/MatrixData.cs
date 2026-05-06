

public class MatrixData
{
    public double[,] adirMatrix;
    public double[,] aundirMatrix;

    public bool directed;
    public int vertexCount;

    public MatrixData() {}
    public MatrixData(double[,] adirMatrix, double[,] aundirMatrix, bool directed, int vertexCount)
    {
        this.adirMatrix = adirMatrix;
        this.aundirMatrix = aundirMatrix;
        this.directed = directed;
        this.vertexCount = vertexCount;
    }
}
