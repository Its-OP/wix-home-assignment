using System.Text;

namespace SnakesAndLadders;

public static class Stringifier
{
    private const string Red = "\u001b[31m";
    private const string Green  = "\u001b[32m";
    private const string Reset = "\u001b[0m";
    
    public static string StringifyBoardSolution(Tile[,] board, IReadOnlyList<int> solution)
    {
        var numRows = board.GetLength(0);
        var numCols = board.GetLength(1);
        var maxPosition = numCols * numRows - 1;
        
        var currentPosition = 0;
        var redCells = new HashSet<(int, int)> { PositionToCoordinates(currentPosition, numRows, numCols) };
        var greenCells = new HashSet<(int, int)>();

        foreach (var diceRoll in solution)
        {
            currentPosition = Math.Min(currentPosition + diceRoll, maxPosition);
            var (row, col) = PositionToCoordinates(currentPosition, numRows, numCols);
            redCells.Add((row, col));
            if (board[row, col].IsSnakeOrLadder)
            {
                currentPosition = board[row, col].Value;
                greenCells.Add(PositionToCoordinates(currentPosition, numRows, numCols));
            }
        }
        
        // Use cells of the same width
        var maxLen = board.Cast<Tile>().Max(x => x.Position.ToString().Length);
        // one space left + one space right -> +2
        var innerWidth = maxLen + 2;

        var horizontalBorder = "+" + string.Join("+", Enumerable.Repeat(new string('-', innerWidth), numCols)) + "+";
        var sb = new StringBuilder();
        for (var row = 0; row < numRows; row++)
        {
            // Top border of the current row
            sb.AppendLine(horizontalBorder);
            var rowSb = new StringBuilder();
            for (var col = 0; col < numCols; col++)
            {
                var tile = board[row, col];
                var rawCellContents = tile.IsSnakeOrLadder ? (tile.Value + 1).ToString() : "-1";
                var cellContents = rawCellContents;   // visible text without colour codes
                if (redCells.Contains((row, col)))
                {
                    cellContents = $"{Red}{cellContents}{Reset}";
                }
                else if (greenCells.Contains((row, col)))
                {
                    cellContents = $"{Green}{cellContents}{Reset}";
                }
                rowSb.Append('|');
                // 1-space left padding
                rowSb.Append(' ');
                rowSb.Append(cellContents);
                // Fill the remaining width (gives ≥1 space on the right)
                rowSb.Append(new string(' ', innerWidth - 1 - rawCellContents.Length));
            }

            rowSb.Append('|');
            sb.AppendLine(rowSb.ToString());
        }

        // bottom border
        sb.AppendLine(horizontalBorder);
        sb.AppendLine($"The optimal combination of dice rolls is: [ {string.Join(", ", solution)} ]");
        return sb.ToString();
    }

    private static (int, int) PositionToCoordinates(int position, int numRows, int numCols)
    {
        var rowFromBottom = position / numCols;
        var row = numRows - 1 - rowFromBottom;
        var col = position % numCols;
        if (rowFromBottom % 2 == 1)
            col = numCols - 1 - col;

        return (row, col);
    }
}