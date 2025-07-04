using NUnit.Framework;
using SnakesAndLadders;

namespace UnitTests;

public class SolverUnitTests
{
    private static class RegularBoards
    {
        // 6x6 board from LeetCode: https://leetcode.com/problems/snakes-and-ladders/description/
        private static readonly int[] LeetCodeBoard = [
            00, 13, 02, 03, 04, 05,
            06, 07, 08, 09, 10, 11,
            12, 34, 14, 15, 12, 17,
            18, 19, 20, 21, 22, 23,
            24, 25, 26, 27, 28, 29,
            30, 31, 32, 33, 34, 35
        ];

        private static readonly int[] LeetCodeBoardSolution = [6, 6, 1, 6];

        private static readonly int[] OneTileBoard = [00];
        private static readonly int[] OneTileBoardSolution = [6];
        
        // Board with 23 tiles. Mathematically, it cannot be reshaped into a rectangle
        private static readonly int[] NonRectangularBoard =
        [
            00, 17, 02, 03, 04,
            05, 06, 07, 20, 09,
            10, 11, 12, 13, 14,
            15, 16, 17, 18, 19,
            20, 03, 22
        ];

        private static readonly int[] NonRectangularBoardSolution = [1, 6];
        
        // Every tile, besides the first and the last ones, has a snake to the first tile
        private static readonly int[] UnsolvableBoard =
        [
            00, 00, 00,
            00, 00, 00,
            00, 00, 08
        ];

        private static readonly int[] UnsolvableBoardSolution = [];
        
        public static IEnumerable<TestCaseData> TestCases
        {
            get
            {
                yield return new TestCaseData(LeetCodeBoard, LeetCodeBoardSolution).SetName("LeetCode 6x6 Board");
                yield return new TestCaseData(OneTileBoard, OneTileBoardSolution).SetName("One-tile board");
                yield return new TestCaseData(NonRectangularBoard, NonRectangularBoardSolution).SetName("Non-rectangular board");
                yield return new TestCaseData(UnsolvableBoard, UnsolvableBoardSolution).SetName("Unsolvable board");
            }
        }
    }
    
    private static class InvalidBoards
    {
        
        // There is no tile with index 10 on the board
        private static readonly int[] BoardWithLargeTile =
        [
            00, 10, 02,
            03, 04, 05,
            06, 07, 08
        ];

        // Tiles must never be negative
        private static readonly int[] BoardWithNegativeTile =
        [
            00, -1, 02,
            03, 04, 05,
            06, 07, 08
        ];
        
        private static readonly int[] EmptyBoard = [];
        
        public static IEnumerable<TestCaseData> TestCases
        {
            get
            {
                yield return new TestCaseData(BoardWithLargeTile).SetName("Board with a large tile");
                yield return new TestCaseData(BoardWithNegativeTile).SetName("Board with a negative tile");
                yield return new TestCaseData(EmptyBoard).SetName("Empty board");
            }
        }
    }
    
    [TestCaseSource(typeof(RegularBoards), nameof(RegularBoards.TestCases))]
    public void SolveBoard_BoardIsRegular_ReturnCorrectDiceSequence(int[] boardTiles, int[] expectedDiceSequence)
    {
        // Arrange
        var board = boardTiles.ToList();
        
        // Act
        var diceSequence = Solver.SolveBoard(board);
        
        // Assert
        Assert.That(diceSequence, Is.EqualTo(expectedDiceSequence));
    }
    
    [TestCaseSource(typeof(InvalidBoards), nameof(InvalidBoards.TestCases))]
    public void SolveBoard_BoardIsInvalid_ThrowArgumentException(int[] boardTiles)
    {
        // Arrange
        var board = boardTiles.ToList();
        
        // Act + Assert
        Assert.That(() => Solver.SolveBoard(board), Throws.ArgumentException);
    }
}