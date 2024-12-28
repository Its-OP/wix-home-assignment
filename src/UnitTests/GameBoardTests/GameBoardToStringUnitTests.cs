﻿using Solution.Entities;

namespace UnitTests.GameBoardTests;

public class GameBoardToStringUnitTests
{
    [Test]
    public void ToString_ReturnCorrectStringRepresentation()
    {
        // Arrange
        var gameBoard = new GameBoard(new [,] { { 1, 2, 3, 4 }, { 5, 6, 7, 8 }, { 9, 10, 11, 12 }, { 13, 14, 15, 0 } });
        const string expectedStringifiedBoard = """
                                                 1  2  3  4
                                                 5  6  7  8
                                                 9 10 11 12
                                                13 14 15  0
                                                """;
        
        // Act
        var stringifiedGameBoard = gameBoard.ToString();
        
        // Assert
        Assert.That(stringifiedGameBoard, Is.EqualTo(expectedStringifiedBoard));
    }
}