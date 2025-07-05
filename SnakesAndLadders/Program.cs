using SnakesAndLadders;

var generator = new BoardGenerator();
var board = generator.GenerateBoard(64);
var solution = Solver.SolveBoard(board);

var rectangularBoard = ArrayUtils.FoldTilesIntoBoard(board, 8, 8);
var stringBoard = Stringifier.StringifyBoardSolution(rectangularBoard, solution);

Console.WriteLine(stringBoard);
Console.ReadKey();