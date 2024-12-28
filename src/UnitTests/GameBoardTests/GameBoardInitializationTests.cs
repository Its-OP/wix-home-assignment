using Solution.Entities;

namespace UnitTests.GameBoardTests;

public class BoardInitializationTests
{
    [Test]
    public void InitializeBoard_TilesArePositionedCorrectly_UseTilesAsInitialBoard()
    {
        // Arrange
        int[,] tiles = { { 0, 1, 2, 3 }, { 4, 5, 6, 7 }, { 8, 9, 10, 11 }, { 12, 13, 14, 15 } };
        
        // Act
        var board = new GameBoard(tiles);
        
        // Assert
        Assert.That(board.GetTilesConfiguration(), Is.EqualTo(tiles));
    }
    
    [Test]
    public void InitializeBoard_TilesAreNotPositionedAsSquare_ThrowException()
    {
        // Arrange
        int[,] nonSquaredTiles = { { 0, 1, 2, 3 }, { 4, 5, 6, 7 }, { 8, 9, 10, 11 } };
        
        // Act + Assert
        Assert.That(() => new GameBoard(nonSquaredTiles), Throws.Exception);
    }
    
    [Test]
    public void InitializeBoard_TilesContainInvalidNumber_ThrowException()
    {
        // Arrange
        int[,] tilesWithInvalidNumber = { { 0, 1, 2, 3 }, { 4, 5, 6, 7 }, { 8, 9, 10, 11 }, { 12, 13, 14, 100 } };
        
        // Act + Assert
        Assert.That(() => new GameBoard(tilesWithInvalidNumber), Throws.Exception);
    }
}