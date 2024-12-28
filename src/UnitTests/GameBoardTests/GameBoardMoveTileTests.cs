using Solution.Entities;

namespace UnitTests.GameBoardTests;

public class GameBoardMoveTileTests
{
    private static int EmptyTile => GameBoard.EmptyTilePlaceholder;
    
    [TestCase(2)]
    [TestCase(10)]
    [TestCase(7)]
    [TestCase(5)]
    public void GameBoardMoveTile_TileCanBeMoved_TileMoved(int tile)
    {
        var tilesConfiguration = GetValidFourByFourTilesConfiguration();
        var board = new GameBoard(tilesConfiguration);
        var initialPositionOfEmptyTile = new Point(1, 2);
        var initialPositionOfTileToBeMoved = GetPositionOfTile(tile, tilesConfiguration);
        
        // Act
        board.MoveTile(tile);
        var updatedTilesConfiguration = board.GetTilesConfiguration();
        
        // Assert
        Assert.That(GetPositionOfTile(EmptyTile, updatedTilesConfiguration), Is.EqualTo(initialPositionOfTileToBeMoved));
        Assert.That(GetPositionOfTile(tile, updatedTilesConfiguration), Is.EqualTo(initialPositionOfEmptyTile));
    }
    
    [Test]
    public void GameBoardMoveTile_TileCanBeMoved_ReturnTrue()
    {
        var initialTilesConfiguration = GetValidFourByFourTilesConfiguration();

        const int tileThatCanBeMoved = 2;
        var board = new GameBoard(initialTilesConfiguration);
        
        // Act
        var tileMoved = board.MoveTile(tileThatCanBeMoved);
        
        // Assert
        Assert.That(tileMoved, Is.EqualTo(true));
    }
    
    [Test]
    public void GameBoardMoveTile_TileIsNotPresentOnTheBoard_DoNotMoveTiles()
    {
        var initialTilesConfiguration = GetValidFourByFourTilesConfiguration();
        var board = new GameBoard(initialTilesConfiguration);
        
        // Act
        board.MoveTile(int.MaxValue);
        
        // Assert
        Assert.That(initialTilesConfiguration, Is.EqualTo(board.GetTilesConfiguration()));
    }
    
    [Test]
    public void GameBoardMoveTile_TileIsNotPresentOnTheBoard_ReturnFalse()
    {
        var initialTilesConfiguration = GetValidFourByFourTilesConfiguration();
        var board = new GameBoard(initialTilesConfiguration);
        
        // Act
        var tileMoved = board.MoveTile(int.MaxValue);
        
        // Assert
        Assert.That(tileMoved, Is.EqualTo(false));
    }
    
    [TestCase(1)]
    [TestCase(4)]
    [TestCase(9)]
    [TestCase(15)]
    public void GameBoardMoveTile_TileCannotBeMoved_DoNotMoveTiles(int tile)
    {
        var initialTilesConfiguration = GetValidFourByFourTilesConfiguration();
        var board = new GameBoard(initialTilesConfiguration);
        
        // Act
        board.MoveTile(tile);
        
        // Assert
        Assert.That(initialTilesConfiguration, Is.EqualTo(board.GetTilesConfiguration()));
    }
    
    [TestCase(1)]
    [TestCase(4)]
    [TestCase(9)]
    [TestCase(15)]
    public void GameBoardMoveTile_TileCannotBeMoved_ReturnFalse(int tile)
    {
        var initialTilesConfiguration = GetValidFourByFourTilesConfiguration();
        var board = new GameBoard(initialTilesConfiguration);
        
        // Act
        var tileMoved = board.MoveTile(tile);

        // Assert
        Assert.That(tileMoved, Is.EqualTo(false));
    }
    
    /// <summary>
    /// Returns the following board:<br />
    ///  6  1  2  3<br />
    ///  4  5  0  7<br />
    ///  8  9 10 11<br />
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