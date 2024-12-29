using Solution.Entities;

namespace UnitTests.GameBoardTests;

public class GameBoardIsInWinningConfigurationTests
{
    [Test]
    public void IsInWinningConfiguration_GameIsStartedWithWinningConfiguration_ReturnTrue()
    {
        // Arrange
        var initialTilesConfiguration = GetTilesConfiguration(isWinning: true);
        var board = new GameBoard(initialTilesConfiguration);

        // Assert
        Assert.That(board.IsInWinningConfiguration, Is.EqualTo(true));
    }
    
    [Test]
    public void IsInWinningConfiguration_GameIsStartedInNonWinningConfiguration_ReturnFalse()
    {
        // Arrange
        var initialTilesConfiguration = GetTilesConfiguration(isWinning: false);
        var board = new GameBoard(initialTilesConfiguration);

        // Assert
        Assert.That(board.IsInWinningConfiguration, Is.EqualTo(false));
    }
    
    [Test]
    public void IsInWinningConfiguration_ConfigurationBecameWinningAfterTileWasMoved_ReturnTrue()
    {
        // Arrange
        var initialTilesConfiguration = GetTilesConfiguration(isWinning: false);
        var board = new GameBoard(initialTilesConfiguration);
        
        // Act
        board.MoveTile(15);

        // Assert
        Assert.That(board.IsInWinningConfiguration, Is.EqualTo(true));
    }
    
    [Test]
    public void IsInWinningConfiguration_ConfigurationBecameNonWinningAfterTileWasMoved_ReturnFalse()
    {
        // Arrange
        var initialTilesConfiguration = GetTilesConfiguration(isWinning: true);
        var board = new GameBoard(initialTilesConfiguration);
        
        // Act
        board.MoveTile(15);

        // Assert
        Assert.That(board.IsInWinningConfiguration, Is.EqualTo(false));
    }
    
    /// <summary>
    /// Returns board for the winning configuration:<br />
    ///  1  2  3  4<br />
    ///  5  6  7  8<br />
    ///  9 10 11 12<br />
    /// 13 14 15  0<br />
    /// Or for the almost-winning one:<br />
    ///  1  2  3  4<br />
    ///  5  6  7  8<br />
    ///  9 10 11 12<br />
    /// 13 14  0 15<br />
    /// </summary>
    /// <returns></returns>
    private int[,] GetTilesConfiguration(bool isWinning)
    {
        if (isWinning)
            return new [,] { { 1, 2, 3, 4 }, { 5, 6, 7, 8 }, { 9, 10, 11, 12 }, { 13, 14, 15, 0 } };
        
        return new [,] { { 1, 2, 3, 4 }, { 5, 6, 7, 8 }, { 9, 10, 11, 12 }, { 13, 14, 0, 15 } };
    }
}