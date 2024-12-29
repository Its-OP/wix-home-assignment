using Solution.Entities;

namespace UnitTests.GameBoardTests;

public class GameBoardGetTilesConfigurationUnitTests
{
    private readonly int[,] _defaultTilesConfiguration = new [,] { { 1, 2, 3, 4 }, { 5, 6, 7, 8 }, { 9, 10, 11, 12 }, { 13, 14, 15, 0 } };

    [Test]
    public void GetTilesConfiguration_ReturnCorrectConfiguration()
    {
        // Arrange
        var board = new GameBoard(_defaultTilesConfiguration);
        
        // Act
        var boardTilesConfiguration = board.GetTilesConfiguration();
        
        // Assert
        Assert.That(boardTilesConfiguration, Is.EqualTo(_defaultTilesConfiguration));
    }
    
    [Test]
    public void GetTilesConfiguration_BoardRemainsUnchanged_ReturnEqualConfigurations()
    {
        // Arrange
        var board = new GameBoard(_defaultTilesConfiguration);
        
        // Act
        var tilesConfiguration1 = board.GetTilesConfiguration();
        var tilesConfiguration2 = board.GetTilesConfiguration();
        
        // Assert
        Assert.That(tilesConfiguration1, Is.EqualTo(tilesConfiguration2));
    }
    
    [Test]
    public void GetTilesConfiguration_BoardRemainsUnchanged_ReturnRefDifferentConfigurations()
    {
        // Arrange
        var board = new GameBoard(_defaultTilesConfiguration);
        
        // Act
        var tilesConfiguration1 = board.GetTilesConfiguration();
        var tilesConfiguration2 = board.GetTilesConfiguration();
        
        // Assert
        Assert.That(tilesConfiguration1, Is.Not.SameAs(tilesConfiguration2));
    }
}