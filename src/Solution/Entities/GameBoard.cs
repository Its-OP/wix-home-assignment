using System.Text;
using Solution.Utils;

namespace Solution.Entities;

public class GameBoard
{
    private const int EmptyTilePlaceholder = 0;
    private readonly int[,] _tilesConfiguration;
    private readonly Dictionary<int, Point> _tilesCoordinates;

    private long _deviationFromWinningConfiguration;
    public bool IsInWinningConfiguration => _deviationFromWinningConfiguration == 0;

    public GameBoard(int[,] tiles)
    {
        Guard.That(tiles.GetLength(0) == tiles.GetLength(1), "Tiles must be positioned in a square");
        var (tilesAreValid, validationError) = SetOfTilesIsValid(tiles);
        Guard.That(tilesAreValid, validationError);

        _tilesCoordinates = GetTilesCoordinates(tiles);
        _tilesConfiguration = tiles;
        
        for (var i = 0; i < tiles.GetLength(0); i++) 
        { 
            for (var j = 0; j < tiles.GetLength(1); j++) 
            {
                var tile = tiles[i, j];
                _deviationFromWinningConfiguration += GetDeviationForTile(tile, new Point(i, j), tiles);
            } 
        }
    }

    public bool MoveTile(int tile, Direction direction)
    {
        if (!TileFitsTheBoard(tile, _tilesConfiguration))
            return false;
        
        var coordinatesOfEmptyTile = _tilesCoordinates[EmptyTilePlaceholder];
        var coordinatesOfMovedTile = _tilesCoordinates[tile];

        switch (direction)
        {
            case Direction.Down when coordinatesOfEmptyTile.I == coordinatesOfMovedTile.I + 1 && coordinatesOfEmptyTile.J == coordinatesOfMovedTile.J:
            case Direction.Up when coordinatesOfEmptyTile.I == coordinatesOfMovedTile.I - 1 && coordinatesOfEmptyTile.J == coordinatesOfMovedTile.J:
            case Direction.Left when coordinatesOfEmptyTile.I == coordinatesOfMovedTile.I && coordinatesOfEmptyTile.J == coordinatesOfMovedTile.J-1:
            case Direction.Right when coordinatesOfEmptyTile.I == coordinatesOfMovedTile.I && coordinatesOfEmptyTile.J == coordinatesOfMovedTile.J+1:
                SwapTileWithEmptyOne(coordinatesOfMovedTile, tile);
                return true;
            default:
                return false;
        }
    }

    public int[,] GetTilesConfiguration()
    {
        return (int[,])_tilesConfiguration.Clone();
    }

    private Dictionary<int, Point> GetTilesCoordinates(int[,] tiles)
    {
        var coordinates = new Dictionary<int, Point>(tiles.GetLength(0) * tiles.GetLength(1));
        
        for (var i = 0; i < tiles.GetLength(0); i++) 
        { 
            for (var j = 0; j < tiles.GetLength(1); j++) 
            { 
                var tile = tiles[i, j];
                coordinates[tile] = new Point(i, j);
            } 
        }
        
        return coordinates;
    }

    private (bool, string) SetOfTilesIsValid(int[,] tiles)
    {
        var set = new HashSet<int>(tiles.GetLength(0) * tiles.GetLength(1));
        
        for (var i = 0; i < tiles.GetLength(0); i++) 
        { 
            for (var j = 0; j < tiles.GetLength(1); j++) 
            { 
                var tile = tiles[i, j]; 
                if (!TileFitsTheBoard(tile, tiles))
                    return (false, $"Tile {tile} is out of range");
                if (set.Contains(tile))
                    return (false, $"Tile {tile} is duplicated");
                
                set.Add(tile);
            } 
        }
        
        return (true, string.Empty);
    }

    private bool TileFitsTheBoard(int tile, int[,] tilesConfiguration)
    {
        var maxElement = tilesConfiguration.GetLength(0) * tilesConfiguration.GetLength(1) - 1;
        var minElement = EmptyTilePlaceholder;

        return tile >= minElement && tile <= maxElement;
    }

    private void SwapTileWithEmptyOne(Point coordinatesOfMovedTile, int movedTile)
    {
        var coordinatesOfEmptyTile = _tilesCoordinates[EmptyTilePlaceholder];

        var currentDeviationForEmptyTile = GetDeviationForTile(EmptyTilePlaceholder, _tilesCoordinates[EmptyTilePlaceholder], _tilesConfiguration);
        var currentDeviationForMovedTile = GetDeviationForTile(movedTile, _tilesCoordinates[movedTile], _tilesConfiguration);
        
        _tilesConfiguration[coordinatesOfEmptyTile.I, coordinatesOfEmptyTile.J] = movedTile;
        _tilesConfiguration[coordinatesOfMovedTile.I, coordinatesOfMovedTile.J] = EmptyTilePlaceholder;
        _tilesCoordinates[movedTile] = coordinatesOfEmptyTile;
        _tilesCoordinates[EmptyTilePlaceholder] = coordinatesOfMovedTile;
        
        var newDeviationForEmptyTile = GetDeviationForTile(EmptyTilePlaceholder, _tilesCoordinates[EmptyTilePlaceholder], _tilesConfiguration);
        var newDeviationForMovedTile = GetDeviationForTile(movedTile, _tilesCoordinates[movedTile], _tilesConfiguration);
        
        _deviationFromWinningConfiguration += newDeviationForEmptyTile + newDeviationForMovedTile - currentDeviationForEmptyTile - currentDeviationForMovedTile;
    }

    private int GetDeviationForTile(int tile, Point coordinates, int[,] tilesConfiguration)
    {
        var deviation = 0;
        if (coordinates.I != tilesConfiguration.GetLength(0) - 1 || coordinates.J != tilesConfiguration.GetLength(1) - 1)
        {
            deviation += Math.Abs(tile - (coordinates.I * tilesConfiguration.GetLength(1) + coordinates.J + 1));
        }
        else
        {
            deviation += tile;
        }
        
        return deviation;
    }

    public override string ToString()
    {
        var rows = _tilesConfiguration.GetLength(0);
        var cols = _tilesConfiguration.GetLength(1);
        
        // Find length of the largest number, so all the columns can be padded to the same width
        var columnWidth = _tilesConfiguration.Cast<int>().Max(num => num).ToString().Length;
        var result = new StringBuilder();

        for (var i = 0; i < rows; i++)
        {
            for (var j = 0; j < cols; j++)
            {
                // Format each number with padding
                var formattedNumber = _tilesConfiguration[i, j].ToString().PadLeft(columnWidth);
                result.Append(formattedNumber);
                
                if (j < cols - 1)
                {
                    result.Append(' ');
                }
            }
            if (i < rows - 1)
            {
                result.AppendLine();
            }
        }

        return result.ToString();
    }
}