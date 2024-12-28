namespace Solution.Utils;

public static class TilesConfigurationGenerator
{
    public static int[,] Generate(int boardSize)
    {
        Guard.That(boardSize > 0, $"Board size of {boardSize} is invalid");
        
        var rng = new Random();
        // Generate a sequence of numbers from 0 to N^2-1 and randomly shuffle them
        var tilesUnfolded = Enumerable.Range(0, (int)Math.Pow(boardSize, 2)).OrderBy(_ => rng.Next()).ToList();
        
        var tilesConfiguration = new int[boardSize, boardSize];
        // Fold the sequence into a board of tiles
        for (var i = 0; i < boardSize; i++)
        {
            for (var j = 0; j < boardSize; j++)
            {
                tilesConfiguration[i, j] = tilesUnfolded[i * boardSize + j];
            }
        }
        
        return tilesConfiguration;
    }
}