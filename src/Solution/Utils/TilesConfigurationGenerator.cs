namespace Solution.Utils;

public static class TilesConfigurationGenerator
{
    public static int[,] Generate(int boardSize, int placeholderForEmptyTile)
    {
        Guard.That(boardSize > 0, $"Board size of {boardSize} is invalid");
        
        var rng = new Random();
        // Generate a sequence of numbers from 0 to N^2-1 and randomly shuffle them
        var tilesUnfolded = Enumerable.Range(1, (int)Math.Pow(boardSize, 2)-1).ToList();
        tilesUnfolded.Add(placeholderForEmptyTile);
        var shuffledTilesUnfolded = tilesUnfolded.OrderBy(_ => rng.Next()).ToList();
        
        var tilesConfiguration = new int[boardSize, boardSize];
        // Fold the sequence into a board of tiles
        for (var i = 0; i < boardSize; i++)
        {
            for (var j = 0; j < boardSize; j++)
            {
                tilesConfiguration[i, j] = shuffledTilesUnfolded[i * boardSize + j];
            }
        }
        
        return tilesConfiguration;
    }
}