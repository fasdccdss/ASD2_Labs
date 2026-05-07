
public class MatrixData
{
    public double[,] matrix;

    public int[] vertexDegrees;
    public int[] vertexOutDegrees;
    public int[] vertexInDegrees;
}
public class GraphData
{
    public MatrixData adirMatrixData;
    public MatrixData aundirMatrixData;

    public bool directed;
    public int vertexCount;

    public GraphData() {}
    public GraphData(double[,] adirMatrix, double[,] aundirMatrix, bool directed, int vertexCount)
    {
        this.adirMatrixData = new MatrixData();
        this.aundirMatrixData = new MatrixData();

        this.adirMatrixData.matrix = adirMatrix;
        this.aundirMatrixData.matrix = aundirMatrix;
        this.directed = directed;
        this.vertexCount = vertexCount;
    }
}
