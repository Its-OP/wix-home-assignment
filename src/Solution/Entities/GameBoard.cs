﻿using Solution.Utils;

namespace Solution.Entities;

public class GameBoard
{
    public const int EmptyTilePlaceholder = 0;
    private readonly int[,] _tilesConfiguration;
    private readonly Dictionary<int, Point> _tilesCoordinates;
    private long _deviationFromWinningConfiguration;
    
    public int NumberOfRows => _tilesConfiguration.GetLength(0);
    public int NumberOfColumns => _tilesConfiguration.GetLength(1);
    public bool IsInWinningConfiguration => _deviationFromWinningConfiguration == 0;
    public int MaxTile => NumberOfRows * NumberOfColumns - 1;

    public GameBoard(int[,] tiles)
    {
        var (tilesAreValid, validationError) = SetOfTilesIsValid(tiles);
        Guard.That(tilesAreValid, validationError);

        _tilesCoordinates = GetTilesCoordinates(tiles);
        _tilesConfiguration = tiles;
        
        for (var i = 0; i < NumberOfRows; i++) 
        { 
            for (var j = 0; j < NumberOfColumns; j++) 
            {
                var tile = tiles[i, j];
                _deviationFromWinningConfiguration += GetDeviationForTile(tile, new Point(i, j), tiles);
            } 
        }
    }

    public bool MoveTile(int tile)
    {
        if (!HasTile(tile))
            return false;
        
        var coordinatesOfEmptyTile = _tilesCoordinates[EmptyTilePlaceholder];
        var coordinatesOfMovedTile = _tilesCoordinates[tile];

        if (TileIsAboveTheEmptyOne(coordinatesOfMovedTile, coordinatesOfEmptyTile)
            || TileIsBelowTheEmptyOne(coordinatesOfMovedTile, coordinatesOfEmptyTile)
            || TileIsToTheLeftOfTheEmptyOne(coordinatesOfMovedTile, coordinatesOfEmptyTile)
            || TileIsToTheRightOfTheEmptyOne(coordinatesOfMovedTile, coordinatesOfEmptyTile))
        {
            SwapTileWithEmptyOne(coordinatesOfMovedTile, tile);
            return true;
        }
        return false;
    }

    public int[,] GetTilesConfiguration()
    {
        return (int[,])_tilesConfiguration.Clone();
    }

    public bool HasTile(int tile)
    {
        return _tilesCoordinates.ContainsKey(tile);
    }

    private static bool TileFitsTheBoard(int tile, int[,] tilesConfiguration)
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

    private static int GetDeviationForTile(int tile, Point coordinates, int[,] tilesConfiguration)
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

    private bool TileIsToTheLeftOfTheEmptyOne(Point tileCoordinates, Point emptyTileCoordinates) =>
        emptyTileCoordinates.I == tileCoordinates.I && emptyTileCoordinates.J == tileCoordinates.J+1;
    private bool TileIsToTheRightOfTheEmptyOne(Point tileCoordinates, Point emptyTileCoordinates) =>
        emptyTileCoordinates.I == tileCoordinates.I && emptyTileCoordinates.J == tileCoordinates.J-1;
    private bool TileIsAboveTheEmptyOne(Point tileCoordinates, Point emptyTileCoordinates) =>
        emptyTileCoordinates.I == tileCoordinates.I - 1 && emptyTileCoordinates.J == tileCoordinates.J;
    private bool TileIsBelowTheEmptyOne(Point tileCoordinates, Point emptyTileCoordinates) =>
        emptyTileCoordinates.I == tileCoordinates.I + 1 && emptyTileCoordinates.J == tileCoordinates.J;
    
    private static Dictionary<int, Point> GetTilesCoordinates(int[,] tiles)
    {
        var numberOfRows = tiles.GetLength(0);
        var numberOfColumns = tiles.GetLength(1);
        var coordinates = new Dictionary<int, Point>(numberOfRows * numberOfColumns);
        
        for (var i = 0; i < numberOfRows; i++) 
        { 
            for (var j = 0; j < numberOfColumns; j++) 
            { 
                var tile = tiles[i, j];
                coordinates[tile] = new Point(i, j);
            } 
        }
        
        return coordinates;
    }

    private static (bool, string) SetOfTilesIsValid(int[,] tiles)
    {
        var numberOfRows = tiles.GetLength(0);
        var numberOfColumns = tiles.GetLength(1);
        var set = new HashSet<int>(numberOfRows * numberOfColumns);
        
        for (var i = 0; i < numberOfRows; i++) 
        { 
            for (var j = 0; j < numberOfColumns; j++) 
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
}