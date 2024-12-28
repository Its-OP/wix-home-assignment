using Solution.Presentation;

namespace UnitTests;

public class GameBoardStringifierUnitTests
{
    [Test]
    public void ToString_ReturnCorrectStringRepresentation()
    {
        // Arrange
        var tilesConfiguration = new [,] { { 1, 2, 3, 4 }, { 5, 6, 7, 8 }, { 9, 10, 11, 12 }, { 13, 14, 15, 0 } };
        const string expectedStringifiedTilesConfiguration = """
                                                 1  2  3  4
                                                 5  6  7  8
                                                 9 10 11 12
                                                13 14 15  0
                                                """;
        
        // Act
        var stringifiedTilesConfiguration = GameBoardStringifier.ToString(tilesConfiguration);
        
        // Assert
        Assert.That(stringifiedTilesConfiguration, Is.EqualTo(expectedStringifiedTilesConfiguration));
    }
}