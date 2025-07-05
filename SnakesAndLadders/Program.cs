using System.Text;
using SnakesAndLadders;

var generator = new BoardGenerator();
var board = generator.GenerateBoard(64);
var solution = Solver.SolveBoard(board);
var stringBoard = StringifyBoard(board, 8, 8, solution);
Console.WriteLine(stringBoard);
Console.ReadKey();
return;

static string StringifyBoard(IReadOnlyList<int> flatBoard, int numRows, int numCols, IList<int> solution)
{
    const string Red = "\u001b[31m";
    const string Green  = "\u001b[32m";
    const string Reset = "\u001b[0m";
    
    if (numRows <= 0 || numCols <= 0)
        throw new ArgumentException("Row/column counts must be positive.");
    if (flatBoard.Count != numRows * numCols)
        throw new ArgumentException("Board size does not match rows × cols.");

    var redBoardTiles = new HashSet<int> { 0, flatBoard.Count - 1 };
    var greenBoardTiles = new HashSet<int>();
    var currTile = 0;
    foreach (var diceRoll in solution)
    {
        currTile += diceRoll;
        redBoardTiles.Add(currTile);
        if (currTile >= flatBoard.Count - 1)
            break;

        if (flatBoard[currTile] != currTile)
        {
            currTile = flatBoard[currTile];
            greenBoardTiles.Add(currTile);
        }
    }
    
    // Reshape the 1D board into a rectangle
    var redBoardCoords = new HashSet<(int, int)>();
    var greenBoardCoords = new HashSet<(int, int)>();
    var board = new int[numRows, numCols];
    var tile = 0;
    var leftToRight = true;
    for (var row = numRows - 1; row >= 0; row--)
    {
        var colNumber = 0;
        while (colNumber < numCols)
        {
            var colIdx = leftToRight ? colNumber : numCols - 1 - colNumber;
            if (redBoardTiles.Contains(tile))
            {
                redBoardCoords.Add((row, colIdx));
            }
            else if (greenBoardTiles.Contains(tile))
            {
                greenBoardCoords.Add((row, colIdx));
            }
            
            var flatBoardTileValue = flatBoard[tile];
            // Set to -1 for regular tiles, and to the tile value for snakes/ladders
            board[row, colIdx] = flatBoardTileValue == tile ? -1 : flatBoardTileValue + 1;
            tile++;
            colNumber++;
        }
        leftToRight = !leftToRight;
    }

    // Use cells of the same width
    var maxLen = flatBoard.Max(v => v.ToString().Length);
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
            var rawCellContents = board[row, col].ToString();
            var cellContents = rawCellContents;   // visible text without colour codes
            if (redBoardCoords.Contains((row, col)))
            {
                cellContents = $"{Red}{cellContents}{Reset}";
            }
            else if (greenBoardCoords.Contains((row, col)))
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