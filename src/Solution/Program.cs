using Solution.Entities;
using Solution.Presentation;
using Solution.Utils;

const int defaultBoardSize = 4;

var gameBoard = new GameBoard(TilesConfigurationGenerator.Generate(defaultBoardSize));
Console.WriteLine("Welcome to 15 Puzzle! Press any key to continue...");
Console.ReadKey();

while (!gameBoard.IsInWinningConfiguration)
{
    PrintGameBoard(gameBoard);
    Console.WriteLine("Type tile that you would like to move");
    var tileString = Console.ReadLine();
    if (!int.TryParse(tileString, out var tile))
    {
        Console.Error.WriteLine("Tile must be a natural number or 0!");
        continue;
    }
    
    if (!gameBoard.HasTile(tile))
    {
        Console.Error.WriteLine($"Tile {tile} is not on the game board!");
        continue;
    }
    
    if (!gameBoard.MoveTile(tile))
    {
        Console.Error.WriteLine($"Tile {tile} cannot be moved!");
        continue;
    }
}

Console.WriteLine("Congratulations! You've solved the puzzle! Press any key to exit.");
Console.ReadKey();
return;

static void PrintGameBoard(GameBoard gameBoard)
{
    var rowLength = GameBoardStringifier.GetStringRowLength(gameBoard);
    var lineDivider = new string('=', rowLength);
    Console.WriteLine(lineDivider);
    Console.WriteLine(GameBoardStringifier.ToString(gameBoard));
    Console.WriteLine(lineDivider);
}