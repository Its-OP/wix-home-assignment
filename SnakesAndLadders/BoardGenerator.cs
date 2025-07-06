namespace SnakesAndLadders;

public class BoardGenerator
{
    // If we get 6 or more snakes in a row, the board becomes unsolvable
    private const int MaxNumberOfConcurrentSnakes = 5;
    
    // How likely a snake or a ladder is to spawn on a tile
    private readonly float _snakeOrLadderGenerationProbability;
    
    // When either a snake or a ladder has spawned, how likely it is to be a ladder
    private readonly float _ladderProbability;
    
    public BoardGenerator(float snakeOrLadderGenerationProbability = 0.1f, float ladderProbability = 0.5f)
    {
        if (snakeOrLadderGenerationProbability is < 0 or > 1)
            throw new ArgumentOutOfRangeException($"Snake or Ladder probability of {snakeOrLadderGenerationProbability} is out of the [0, 1] range");
        
        if (ladderProbability is < 0 or > 1)
            throw new ArgumentOutOfRangeException($"Ladder probability of {ladderProbability} is out of the [0, 1] range");
        
        _snakeOrLadderGenerationProbability = snakeOrLadderGenerationProbability;
        _ladderProbability = ladderProbability;
    }

    public IReadOnlyList<int> GenerateBoard(int numberOfTiles)
    {
        if (numberOfTiles < 1)
            throw new ArgumentException("Board must have at least one tile");
        
        var board = new int[numberOfTiles];
        var random = new Random();
        var numberOfConcurrentSnakes = 0;
        
        // First and last tiles must not have either snakes or ladders
        for (var i = 1; i < numberOfTiles - 1; i++)
        {
            var currentTileValue = i;
            var snakeOrLadderHasSpawned = random.NextSingle() < _snakeOrLadderGenerationProbability;
            if (snakeOrLadderHasSpawned)
            {
                var ladderHasSpawned = random.NextSingle() < _ladderProbability;
                if (ladderHasSpawned || numberOfConcurrentSnakes >= MaxNumberOfConcurrentSnakes)
                {
                    // Ladder goes to at least next tile
                    currentTileValue = random.Next(i + 1, numberOfTiles - 1);
                    numberOfConcurrentSnakes = 0;
                }
                else
                {
                    // Snake goes to at most previous tile
                    currentTileValue = random.Next(0, i - 1);
                    numberOfConcurrentSnakes += 1;
                }
            }
            else
            {
                numberOfConcurrentSnakes = 0;
            }
            
            board[i] = currentTileValue;
        }

        board[^1] = numberOfTiles - 1;
        return board;
    }
}