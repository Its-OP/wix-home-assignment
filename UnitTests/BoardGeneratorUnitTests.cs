using NUnit.Framework;
using SnakesAndLadders;

namespace UnitTests;

public class BoardGeneratorUnitTests
{
    private const int NumberOfTiles = 100;
    
    [Test]
    public void GenerateBoard_ZeroProbabilityForSnakeOrLadder_EveryTilePointsToItself()
    {
        // Arrange
        var generator = new BoardGenerator(0, 0);
        
        // Act
        var board = generator.GenerateBoard(NumberOfTiles);
        var expectedBoardIndices = Enumerable.Range(0, NumberOfTiles).ToList();
        
        // Assert
        Assert.That(board, Is.EqualTo(expectedBoardIndices));
    }
    
    [Test]
    public void GenerateBoard_MaxProbabilityForSnakeOrLadder_FirstAndLastTilesAreClearOfSnakesOrLadders()
    {
        // Arrange
        var generator = new BoardGenerator(snakeOrLadderGenerationProbability: 1);
        
        // Act
        var board = generator.GenerateBoard(NumberOfTiles);
        var expectedBoardIndices = Enumerable.Range(0, NumberOfTiles).ToList();
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(board[0], Is.EqualTo(0));
            Assert.That(board[^1], Is.EqualTo(NumberOfTiles - 1));
        });
    }

    [Test]
    public void GenerateBoard_MaxProbabilityForSnakes_BoardRemainsSolvable()
    {
        // Arrange
        var generator = new BoardGenerator(1, 0);
        
        // Act
        var board = generator.GenerateBoard(NumberOfTiles);
        var solution = Solver.SolveBoard(board);
        
        // Assert
        Assert.That(solution, Is.Not.Empty);
    }
    
    [Test]
    // A ladder is spawned after every 5 snakes
    public void GenerateBoard_MaxProbabilityForSnakes_SpawnSixteenLadders()
    {
        // Arrange
        var generator = new BoardGenerator(1, 0);
        
        // Act
        var board = generator.GenerateBoard(NumberOfTiles);
        var mapOfSnakes = new List<bool>();
        for (var i = 0; i < NumberOfTiles; i++)
        {
            // Add False if ladder was spawned, True otherwise
            mapOfSnakes.Add(board[i] <= i);
        }
        var numLadders = mapOfSnakes.Count(x => x == false);
        
        // Assert
        Assert.That(numLadders, Is.EqualTo(16));
    }
    
    [Test]
    public void GenerateBoard_MaxProbabilityForLadders_SpawnOnlyLadders()
    {
        // Arrange
        var generator = new BoardGenerator(1, 1);
        
        // Act
        var board = generator.GenerateBoard(NumberOfTiles);
        var mapOfLadders = new List<bool>();
        // Skip first and last tiles
        for (var i = 1; i < NumberOfTiles-1; i++)
        {
            // Add False if ladder was spawned, True otherwise
            mapOfLadders.Add(board[i] >= i);
        }
        
        // Assert
        Assert.That(mapOfLadders, Is.All.True);
    }
}