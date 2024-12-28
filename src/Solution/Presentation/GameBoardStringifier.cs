using System.Text;

namespace Solution.Presentation;

public static class GameBoardStringifier
{
    public static string ToString(int[,] tilesConfiguration)
    {
        var rows = tilesConfiguration.GetLength(0);
        var cols = tilesConfiguration.GetLength(1);
        
        // Find length of the largest number, so all the columns can be padded to the same width
        var columnWidth = tilesConfiguration.Cast<int>().Max(num => num).ToString().Length;
        var result = new StringBuilder();

        for (var i = 0; i < rows; i++)
        {
            for (var j = 0; j < cols; j++)
            {
                // Format each number with padding
                var formattedNumber = tilesConfiguration[i, j].ToString().PadLeft(columnWidth);
                result.Append(formattedNumber);
                
                if (j < cols - 1)
                {
                    result.Append(' ');
                }
            }
            if (i < rows - 1)
            {
                result.AppendLine();
            }
        }

        return result.ToString();
    }
}