using Solution.Utils;

namespace Solution.Entities;

public class GameBoard
{
    const int EmptyTilePlaceholder = 0;
    private readonly int[,] _board;

    public GameBoard(int[,] tiles)
    {
        Guard.That(tiles.GetLength(0) == tiles.GetLength(1), "Tiles must be positioned in a square");
        var (tilesAreValid, validationError) = SetOfTilesIsValid(tiles);
        Guard.That(tilesAreValid, validationError);
        
        _board = tiles;
    }

    public int[,] GetTiles()
    {
        return (int[,])_board.Clone();
    }

    private (bool, string) SetOfTilesIsValid(int[,] tiles)
    {
        var range = tiles.GetLength(0) * tiles.GetLength(1);
        var set = new HashSet<int>(range);
        var maxElement = range - 1;
        var minElement = EmptyTilePlaceholder;
        
        for (var i = 0; i < tiles.GetLength(0); i++) 
        { 
            for (var j = 0; j < tiles.GetLength(1); j++) 
            { 
                var tile = tiles[i, j]; 
                if (tile < minElement || tile > maxElement)
                    return (false, $"Tile {tile} is out of range");
                if (set.Contains(tile))
                    return (false, $"Tile {tile} is duplicated");
                
                set.Add(tile);
            } 
        }
        
        return (true, string.Empty);
    }
}