namespace SnakesAndLadders;

public static class Solver
{
    // Although we should never get stuck in a while loop (even for invalid boards), this constant acts as a fail-safe mechanism
    private const int MaxIterations = 1000;
    
    /// <summary>
    /// Expects a list where:
    /// <list type="number">
    ///   <item>
    ///     <description>The index of each element represents the tile number minus 1.</description>
    ///   </item>
    ///   <item>
    ///     <description>The value of each element is the square the player can jump to: equal to the index for a regular tile; greater for a ladder; and smaller for a snake.</description>
    ///   </item>
    /// </list>
    /// </summary>
    public static IList<int> SolveBoard(IReadOnlyList<int> board, int maxIterations = MaxIterations)
    {
        if (board.Count == 0)
            throw new ArgumentException("Board must not be empty");
        
        // BFS solver for Snake and Ladders
        var queue = new Queue<Tile>();
        queue.Enqueue(Tile.CreateFirst(board[0]));
        
        // We can avoid creating a set of visited tiles by modifying the board in-place.
        // However, I don't like such an approach, since the caller typically expects parameters to be immutable
        var visitedTiles = new HashSet<Tile>();
        Tile? finalTile = null;
        var iteration = 0;
        
        while (queue.Count > 0 && finalTile is null && iteration < maxIterations)
        {
            iteration += 1;
            var currentQueueLength = queue.Count;
            for (var i = 0; i < currentQueueLength && finalTile is null; i++)
            {
                // Get tile number
                var tile = queue.Dequeue();
                // A small optimization trick, so if the last tile can be reached directly from the current one, it is reached immediately
                for (var diceRoll = 6; diceRoll >= 1; diceRoll--)
                {
                    // Position of the next tile, capped by the final tile of the board
                    var nextTilePosition = Math.Min(tile.Value + diceRoll, board.Count - 1);
                    // Get tile, where you can move from the current one immediately. It points to itself for a regular tile; in-front for a ladder; and behind for a snake
                    var nextTileValue = board[nextTilePosition];
                    
                    if (nextTileValue < 0 || nextTileValue > board.Count - 1)
                        throw new ArgumentException($"Tile value of {tile.Value} is out of range");
                    
                    // The final tile can be reached right from the current one
                    if (nextTileValue == board.Count - 1)
                    {
                        finalTile = Tile.Create(board.Count - 1, diceRoll, tile);
                        return BuildDiceRollSequence(finalTile);
                    }
                    
                    var nextTile = Tile.Create(nextTileValue, diceRoll, tile);
                    if (!visitedTiles.Add(nextTile))
                        continue;

                    queue.Enqueue(nextTile);
                }
            }
        }

        if (iteration >= MaxIterations)
        {
            throw new ArgumentException("Limit of iterations reached. The board is likely to be invalid.");
        }
        
        // Solution does not exist - we can't reach any new nodes from the current BFS layer, and the final tile has not been reached
        return [];
    }
    
    // Backtrack the optimal path
    private static IList<int> BuildDiceRollSequence(Tile finalTile)
    {
        var diceSequence = new List<int>();
        for (var curr = finalTile; curr.DiceRoll is not null; curr = curr.Previous)
        {
            diceSequence.Add(curr.DiceRoll.Value);
        }

        diceSequence.Reverse();
        
        return diceSequence;
    }
    
    private class Tile : IEquatable<Tile>
    {
        public int Value { get; private set; }
        public int? DiceRoll { get; private set; }
        public Tile? Previous { get; private set; }

        private Tile(int value, int? diceRoll, Tile? previous)
        {
            if (diceRoll is < 1 or > 6)
                throw new ArgumentOutOfRangeException(nameof(diceRoll));
            
            Value = value;
            DiceRoll = diceRoll;
            Previous = previous;
        }

        public static Tile CreateFirst(int value)
        {
            return new Tile(value, null, null);
        }

        public static Tile Create(int value, int diceRoll, Tile previous)
        {
            return new Tile(value, diceRoll, previous);
        }

        public bool Equals(Tile? other)
        {
            if (other is null) return false;
            return Value == other.Value;
        }

        public override int GetHashCode()
        {
            return Value;
        }
    }
}
