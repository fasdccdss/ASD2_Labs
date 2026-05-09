using System.Windows.Forms;

public class AltGraphData
{
    public MatrixData adirMatrixData;
    public MatrixData aundirMatrixData;

    public bool directed;
    public int vertexCount;

    public AltGraphData() {}
    public AltGraphData(MatrixData adirMatrixData, MatrixData aundirMatrixData, bool directed, int vertexCount)
    {
        this.adirMatrixData = adirMatrixData;
        this.aundirMatrixData = aundirMatrixData;
        this.directed = directed;
        this.vertexCount = vertexCount;
    }
    public AltGraphData(double[,] adirMatrix, double[,] aundirMatrix, bool directed, int vertexCount)
    {
        this.adirMatrixData = new MatrixData();
        this.aundirMatrixData = new MatrixData();

        this.adirMatrixData.matrix = adirMatrix;
        this.aundirMatrixData.matrix = aundirMatrix;
        this.directed = directed;
        this.vertexCount = vertexCount;
    }
}
