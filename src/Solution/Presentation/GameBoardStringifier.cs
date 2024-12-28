﻿using System.Text;
using Solution.Entities;

namespace Solution.Presentation;

public static class GameBoardStringifier
{
    public static string ToString(GameBoard gameBoard)
    {
        var tilesConfiguration = gameBoard.GetTilesConfiguration();
        
        // Find length of the largest number, so all the columns can be padded to the same width
        var columnWidth = GetColumnWidth(gameBoard);
        var result = new StringBuilder();

        for (var i = 0; i < gameBoard.NumberOfRows; i++)
        {
            for (var j = 0; j < gameBoard.NumberOfColumns; j++)
            {
                // Format each number with padding
                var formattedNumber = tilesConfiguration[i, j].ToString().PadLeft(columnWidth);
                result.Append(formattedNumber);
                
                if (j < gameBoard.NumberOfColumns - 1)
                {
                    result.Append(' ');
                }
            }
            if (i < gameBoard.NumberOfRows - 1)
            {
                result.AppendLine();
            }
        }

        return result.ToString();
    }

    public static int GetColumnWidth(GameBoard gameBoard)
    {
        return gameBoard.MaxTile.ToString().Length;
    }

    public static int GetStringRowLength(GameBoard gameBoard)
    {
        var columnWidth = GetColumnWidth(gameBoard);
        var numberOfColumns = gameBoard.NumberOfColumns;
        
        return numberOfColumns * columnWidth + numberOfColumns - 1;
    }
}