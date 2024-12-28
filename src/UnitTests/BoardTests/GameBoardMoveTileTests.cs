using Solution.Entities;

namespace UnitTests.BoardTests;

public class GameBoardMoveTileTests
{
    private const int EmptyTile = 0;
    
    [TestCase(2, Direction.Down)]
    [TestCase(10, Direction.Up)]
    [TestCase(7, Direction.Left)]
    [TestCase(5, Direction.Right)]
    public void GameBoardMoveTile_TileCanBeMovedInTheProvidedDirection_TileMoved(int tile, Direction direction)
    {
        var tilesConfiguration = GetValidFourByFourTilesConfiguration();
        var board = new GameBoard(tilesConfiguration);
        var initialPositionOfEmptyTile = new Point(1, 2);
        var initialPositionOfTileToBeMoved = GetPositionOfTile(tile, tilesConfiguration);
        
        // Act
        board.MoveTile(tile, direction);
        var updatedTilesConfiguration = board.GetTilesConfiguration();
        
        // Assert
        Assert.That(GetPositionOfTile(EmptyTile, updatedTilesConfiguration), Is.EqualTo(initialPositionOfTileToBeMoved));
        Assert.That(GetPositionOfTile(tile, updatedTilesConfiguration), Is.EqualTo(initialPositionOfEmptyTile));
    }
    
    [Test]
    public void GameBoardMoveTile_TileCanBeMovedInTheProvidedDirection_TileMoved()
    {
        var initialTilesConfiguration = GetValidFourByFourTilesConfiguration();

        const int tileThatCanBeMoved = 2;
        const Direction directionWhereTileCanBeMoved = Direction.Down;
        var board = new GameBoard(initialTilesConfiguration);
        
        // Act
        var tileMoved = board.MoveTile(tileThatCanBeMoved, directionWhereTileCanBeMoved);
        
        // Assert
        Assert.That(tileMoved, Is.EqualTo(true));
    }
    
    [Test]
    public void GameBoardMoveTile_TileIsNotPresentOnTheBoard_DoNotMoveTiles()
    {
        var initialTilesConfiguration = GetValidFourByFourTilesConfiguration();
        var board = new GameBoard(initialTilesConfiguration);
        
        // Act
        board.MoveTile(int.MaxValue, Direction.Down);
        
        // Assert
        Assert.That(initialTilesConfiguration, Is.EqualTo(board.GetTilesConfiguration()));
    }
    
    [Test]
    public void GameBoardMoveTile_TileIsNotPresentOnTheBoard_ReturnFalse()
    {
        var initialTilesConfiguration = GetValidFourByFourTilesConfiguration();
        var board = new GameBoard(initialTilesConfiguration);
        
        // Act
        var tileMoved = board.MoveTile(int.MaxValue, Direction.Down);
        
        // Assert
        Assert.That(tileMoved, Is.EqualTo(false));
    }
    
    [TestCase(1, Direction.Down)]
    [TestCase(5, Direction.Left)]
    [TestCase(10, Direction.Right)]
    [TestCase(6, Direction.Up)]
    public void GameBoardMoveTile_TryMoveTileOverNonEmptyOne_DoNotMoveTiles(int tile, Direction direction)
    {
        var initialTilesConfiguration = GetValidFourByFourTilesConfiguration();
        var board = new GameBoard(initialTilesConfiguration);
        
        // Act
        board.MoveTile(tile, direction);
        
        // Assert
        Assert.That(initialTilesConfiguration, Is.EqualTo(board.GetTilesConfiguration()));
    }
    
    [TestCase(1, Direction.Down)]
    [TestCase(5, Direction.Left)]
    [TestCase(10, Direction.Right)]
    [TestCase(6, Direction.Up)]
    public void GameBoardMoveTile_TryMoveTileOverNonEmptyOne_ReturnFalse(int tile, Direction direction)
    {
        var initialTilesConfiguration = GetValidFourByFourTilesConfiguration();
        var board = new GameBoard(initialTilesConfiguration);
        
        // Act
        var tileMoved = board.MoveTile(tile, direction);

        // Assert
        Assert.That(tileMoved, Is.EqualTo(false));
    }
    
    /// <summary>
    /// Returns the following board:<br />
    /// 6  1  2  3<br />
    /// 4  5  0  7<br />
    /// 8  9 10 11<br />
    /// 12 13 14 15
    /// </summary>
    /// <returns></returns>
    private int[,] GetValidFourByFourTilesConfiguration()
    {
        return new[,] { { 6, 1, 2, 3 }, { 4, 5, 0, 7 }, { 8, 9, 10, 11 }, { 12, 13, 14, 15 } };
    }

    private Point? GetPositionOfTile(int tile, int[,] tilesConfiguration)
    {
        for (var i = 0; i < tilesConfiguration.GetLength(0); i++)
        {
            for (var j = 0; j < tilesConfiguration.GetLength(1); j++)
            {
                if (tilesConfiguration[i, j] == tile)
                {
                    return new Point(i, j);
                }
            }
        }
        
        return null;
    }
}