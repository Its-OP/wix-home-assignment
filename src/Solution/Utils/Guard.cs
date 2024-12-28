namespace Solution.Utils;

public static class Guard
{
    public static void That(bool condition, string message)
    {
        if (!condition)
            throw new InvalidOperationException(message);
    }
}