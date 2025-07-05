namespace SnakesAndLadders;

public static class Solver
{
    // Although we should never get stuck in a while loop (even for invalid boards), this constant acts as a fail-safe mechanism
    private const int MaxIterations = 1000;
    
    // Expects a list where:
    // 1. The index of each element represents the position of the tile minus 1.
    // 2. The value of each element represents the value of the tile.
    public static IReadOnlyList<int> SolveBoard(IReadOnlyList<int> board, int maxIterations = MaxIterations)
    {
        if (board.Count == 0)
            throw new ArgumentException("Board must not be empty");
        
        // BFS solver for Snake and Ladders
        var queue = new Queue<TileNode>();
        queue.Enqueue(TileNode.CreateFirst(board[0]));
        
        // We can avoid creating a set of visited tiles by modifying the board in-place.
        // However, I don't like such an approach, since the caller typically expects parameters to be immutable
        var visitedTiles = new HashSet<TileNode>();
        TileNode? finalTile = null;
        var iteration = 0;
        
        while (queue.Count > 0 && finalTile is null && iteration < maxIterations)
        {
            iteration += 1;
            var currentQueueLength = queue.Count;
            for (var i = 0; i < currentQueueLength && finalTile is null; i++)
            {
                var tile = queue.Dequeue();
                // A small optimization trick, so if the last tile can be reached directly from the current one, it is reached immediately
                for (var diceRoll = 6; diceRoll >= 1; diceRoll--)
                {
                    // Position of the next tile, capped by the final tile of the board
                    var nextTilePosition = Math.Min(tile.Position + diceRoll, board.Count - 1);
                    var nextTileValue = board[nextTilePosition];
                    
                    if (nextTileValue < 0 || nextTileValue > board.Count - 1)
                        throw new ArgumentException($"Tile value of {tile.Position} is out of range");
                    
                    // The final tile can be reached right from the current one
                    if (nextTileValue == board.Count - 1)
                    {
                        finalTile = TileNode.Create(board.Count - 1, diceRoll, tile);
                        return BuildDiceRollSequence(finalTile);
                    }
                    
                    var nextTile = TileNode.Create(nextTileValue, diceRoll, tile);
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
    private static IReadOnlyList<int> BuildDiceRollSequence(TileNode finalTile)
    {
        var diceSequence = new List<int>();
        for (var curr = finalTile; curr.DiceRoll is not null; curr = curr.Previous)
        {
            diceSequence.Add(curr.DiceRoll.Value);
        }

        diceSequence.Reverse();
        
        return diceSequence;
    }
    
    private class TileNode : IEquatable<TileNode>
    {
        public int Position { get; private set; }
        public int? DiceRoll { get; private set; }
        public TileNode? Previous { get; private set; }

        private TileNode(int position, int? diceRoll, TileNode? previous)
        {
            if (diceRoll is < 1 or > 6)
                throw new ArgumentOutOfRangeException(nameof(diceRoll));
            
            Position = position;
            DiceRoll = diceRoll;
            Previous = previous;
        }

        public static TileNode CreateFirst(int value)
        {
            return new TileNode(value, null, null);
        }

        public static TileNode Create(int value, int diceRoll, TileNode previous)
        {
            return new TileNode(value, diceRoll, previous);
        }

        public bool Equals(TileNode? other)
        {
            if (other is null) return false;
            return Position == other.Position;
        }

        public override int GetHashCode()
        {
            return Position;
        }
    }
}
