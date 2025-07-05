using NUnit.Framework;
using SnakesAndLadders;

namespace UnitTests;

public class ArrayUtilsUnitTests
{
    private static class RegularBoards
    {
        private static readonly int[] SquareBoard = [
            00, 07, 02,
            03, 04, 02,
            06, 07, 08,
        ];
        private static readonly Tile[,] SquareBoardFolded =
        {
            { new(6, 6), new(7, 7), new(8, 8) },
            { new(2, 5), new(4, 4), new(3, 3) },
            { new(0, 0), new(7, 1), new(2, 2) },
        };

        private static readonly int[] OneTileBoard = [00];
        private static readonly Tile[,] OneTileBoardFolded = { { new(0, 0) } };
        
        private static readonly int[] RectangularBoard = [
            00, 07, 02, 03,
            04, 05, 01, 07
        ];
        private static readonly Tile[,] RectangularBoardFolded =
        {
            { new(7, 7), new(1, 6), new(5, 5), new(4, 4)},
            { new(0, 0), new(7, 1), new(2, 2), new(3, 3) },
        };
        
        public static IEnumerable<TestCaseData> TestCases
        {
            get
            {
                yield return new TestCaseData(SquareBoard, 3, 3, SquareBoardFolded).SetName("Square Board");
                yield return new TestCaseData(OneTileBoard, 1, 1, OneTileBoardFolded).SetName("One-tile board");
                yield return new TestCaseData(RectangularBoard, 2, 4, RectangularBoardFolded).SetName("Rectangular Board");
            }
        }
    }
    
    [TestCaseSource(typeof(RegularBoards), nameof(RegularBoards.TestCases))]
    public void FoldTilesIntoBoard_ParametersAreValid_ReturnCorrectBoard(int[] flatBoard, int numberOfRows, int numberOfColumns, Tile[,] expectedFoldedBoard)
    {
        // Act
        var board = ArrayUtils.FoldTilesIntoBoard(flatBoard, numberOfRows, numberOfColumns);
        
        // Assert
        Assert.That(board, Is.EqualTo(expectedFoldedBoard));
    }
    
    [TestCase(new [] { 0, 1, 2, 3, 4, 5 }, 4, 2, TestName = "Too many rows")]
    [TestCase(new [] { 0, 1, 2, 3, 4, 5 }, 2, 4, TestName = "Too many columns")]
    [TestCase(new [] { 0, 1, 2, 3, 4, 5 }, -2, 4, TestName = "Number of rows is negative")]
    [TestCase(new [] { 0, 1, 2, 3, 4, 5 }, 2, -4, TestName = "Number of columns is negative")]
    [TestCase(new [] { 0, 1, 2, 3, 4, 5 }, -2, -3, TestName = "Number of both rows and columns is negative")]
    public void FoldTilesIntoBoard_ExpectedDimensionsAreInvalid_ThrowArgumentException(int[] flatBoard, int numberOfRows, int numberOfColumns)
    {
        // Act + Assert
        Assert.That(() => ArrayUtils.FoldTilesIntoBoard(flatBoard, numberOfRows, numberOfColumns), Throws.ArgumentException);
    }
}