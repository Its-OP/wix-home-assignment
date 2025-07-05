using SnakesAndLadders;

while (true)
{
    var numRows = 0;
    var numRowsIsValid = false;
    while (!numRowsIsValid)
    {
        Console.WriteLine("Please enter the number of rows of the board (must be greater than 0):");
        var numRowsStr = Console.ReadLine();
        if (!int.TryParse(numRowsStr, out numRows) || numRows < 1)
            Console.Error.WriteLine($"Value of {numRowsStr} is invalid for the number of rows.");
        else
            numRowsIsValid = true;
    }

    var numCols = 0;
    var numColsIsValid = false;
    while (!numColsIsValid)
    {
        Console.WriteLine("Please enter the number of columns of the board (must be greater than 0):");
        var numColsStr = Console.ReadLine();
        if (!int.TryParse(numColsStr, out numCols) || numCols < 1)
            Console.Error.WriteLine($"Value of {numColsStr} is invalid for the number of columns.");
        else
            numColsIsValid = true;
    }

    var snakeOrLadderProbability = 0f;
    var snakeOrLadderProbabilityIsValid = false;
    while (!snakeOrLadderProbabilityIsValid)
    {
        Console.WriteLine(
            "Please enter the probability of a tile to spawn a snake or a ladder (must be a float number between 0 and 1; recommended value is 0.2):");
        var snakeOrLadderProbabilityStr = Console.ReadLine();
        if (!float.TryParse(snakeOrLadderProbabilityStr, out snakeOrLadderProbability) || snakeOrLadderProbability < 0 ||
            snakeOrLadderProbability > 1)
        {
            Console.Error.WriteLine($"Value of {snakeOrLadderProbabilityStr} is invalid for a probability.");
        }
        else
        {
            snakeOrLadderProbabilityIsValid = true;
        }
    }
    
    var ladderProbability = 0f;
    var ladderProbabilityIsValid = false;
    while (!ladderProbabilityIsValid)
    {
        Console.WriteLine(
            "Please enter the probability of a ladder to be spawned on a non-empty tile (must be a float number between 0 and 1; recommended value is 0.5):");
        var ladderProbabilityStr = Console.ReadLine();
        if (!float.TryParse(ladderProbabilityStr, out ladderProbability) || ladderProbability < 0 || ladderProbability > 1)
            Console.Error.WriteLine($"Value of {ladderProbabilityStr} is invalid for a probability.");
        else
            ladderProbabilityIsValid = true;
    }
    

    var generator = new BoardGenerator(snakeOrLadderProbability, ladderProbability);
    var board = generator.GenerateBoard(numRows * numCols);
    var solution = Solver.SolveBoard(board);

    var rectangularBoard = ArrayUtils.FoldTilesIntoBoard(board, numRows, numCols);
    var stringBoard = Stringifier.StringifyBoard(rectangularBoard, solution);
    var stringSolution = Stringifier.StringifySolution(solution);

    Console.WriteLine(stringBoard);
    Console.WriteLine(stringSolution);
    Console.WriteLine("Press any key to continue");
    Console.ReadKey();
    Console.WriteLine();
}