namespace SnakesAndLadders;

public static class ArrayUtils
{
    // Use Tiles instead of ints, because finding the position of an int tile on the 2D board is non-trivial
    public static Tile[,] FoldTilesIntoBoard(IReadOnlyList<int> flatBoard, int numRows, int numCols)
    {
        if (numRows <= 0 || numCols <= 0)
            throw new ArgumentException("Row/column counts must be positive.");
        if (flatBoard.Count != numRows * numCols)
            throw new ArgumentException("Board size does not match rows × cols.");
        
        var board = new Tile[numRows, numCols];
        var position = 0;
        var leftToRight = true;
        for (var row = numRows - 1; row >= 0; row--)
        {
            var colNumber = 0;
            while (colNumber < numCols)
            {
                var colIdx = leftToRight ? colNumber : numCols - 1 - colNumber;
            
                var value = flatBoard[position];
                board[row, colIdx] = new Tile(value, position);
                position++;
                colNumber++;
            }
            leftToRight = !leftToRight;
        }
        
        return board;
    }
}