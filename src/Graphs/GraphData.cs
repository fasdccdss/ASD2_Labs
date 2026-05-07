
public class MatrixData
{
    public double[,] matrix;

    public int[] vertexDegrees;
    public int[] vertexOutDegrees;
    public int[] vertexInDegrees;
}
public class GraphData
{
    public MatrixData adirMatrix;
    public MatrixData aundirMatrix;

    public bool directed;
    public int vertexCount;

    public GraphData() {}
    public GraphData(double[,] adirMatrix, double[,] aundirMatrix, bool directed, int vertexCount)
    {
        this.adirMatrix.matrix = adirMatrix;
        this.aundirMatrix.matrix = aundirMatrix;
        this.directed = directed;
        this.vertexCount = vertexCount;
    }
}
