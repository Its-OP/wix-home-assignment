using Solution.Utils;

namespace UnitTests;

public class TilesConfigurationGeneratorUnitTests
{
    [TestCase(1)]
    [TestCase(10)]
    [TestCase(25)]
    public void Generate_BoardSizeIsGreaterThanZero_ReturnSquaredTilesConfiguration(int boardSize)
    {
        // Act
        var tilesConfiguration = TilesConfigurationGenerator.Generate(boardSize);
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(tilesConfiguration.GetLength(0), Is.EqualTo(boardSize));
            Assert.That(tilesConfiguration.GetLength(1), Is.EqualTo(boardSize));
        });
    }
    
    [TestCase(1)]
    [TestCase(10)]
    [TestCase(25)]
    public void Generate_BoardSizeIsGreaterThanZero_ReturnBoardWithCorrectSetOfElements(int boardSize)
    {
        // Arrange
        var expectedSequenceOfElements = Enumerable.Range(0, (int)Math.Pow(boardSize, 2));
        
        // Act
        var tilesConfiguration = TilesConfigurationGenerator.Generate(boardSize);
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(tilesConfiguration, Is.EquivalentTo(expectedSequenceOfElements));
        });
    }
    
    [TestCase(3)]
    [TestCase(10)]
    [TestCase(25)]
    public void Generate_BoardSizeIsGreaterThanZero_ReturnBoardWithRandomlyShuffledElements(int boardSize)
    {
        // Act
        var board1 = TilesConfigurationGenerator.Generate(boardSize);
        var board2 = TilesConfigurationGenerator.Generate(boardSize);
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(board1, Is.Not.EqualTo(board2));
        });
    }
    
    [TestCase(0)]
    [TestCase(-1)]
    public void Generate_BoardSizeIsNotGreaterThanZero_ThrowException(int boardSize)
    {
        // Act + Assert
        Assert.That(() => TilesConfigurationGenerator.Generate(boardSize), Throws.Exception);
    }
}