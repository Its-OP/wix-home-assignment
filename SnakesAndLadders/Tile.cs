namespace SnakesAndLadders;

public readonly struct Tile
{
    public Tile(int value, int position)
    {
        Value = value;
        Position = position;
    }
    
    /// <summary>
    /// Where the tile moves you when you step on it
    /// </summary>
    public int Value { get; }
    
    /// <summary>
    /// The number over the tile
    /// </summary>
    public int Position { get; }

    public bool IsSnakeOrLadder => Value != Position;
}