using sudoku_maker.Models;

namespace sudoku_maker.Services;

public class SudokuValidator
{
    public bool IsValidMove(SudokuBoard board, int row, int column, int number)
    {
        if (number < 1 || number > SudokuBoard.Size)
        {
            return false;
        }
        return IsRowValid(board, row, column, number) && IsColumnValid(board, row, column, number) && IsBoxValid(board, row, column, number);
    }

    public bool IsBoardValid(SudokuBoard board)
    {
        for(int row = 0; row < SudokuBoard.Size; row++)
        {
            for(int column = 0; column < SudokuBoard.Size; column++)
            {
                int value = board.GetValue(row, column);

                if(value == SudokuBoard.Empty)
                {
                    continue;
                }

                if(!IsValidMove(board, row, column, value))
                {
                    return false;
                }
            }
        }
        return true;
    }

    public bool HasConflict(SudokuBoard board, int row, int column, int number)
    {
        int value = board.GetValue(row, column);

        if(value == SudokuBoard.Empty)
        {
            return false;
        }
        return !IsValidMove(board, row, column, value);
    }

    private bool IsRowValid(SudokuBoard board, int row, int currentColumn, int number)
    {
        for(int column = 0; column < SudokuBoard.Size; column++)
        {
            if(column == currentColumn)
            {
                continue;
            }

            if(board.GetValue(row, column) == number)
            {
                return false;
            }
        }
        return true;
    }

    private bool IsColumnValid(SudokuBoard board, int currentRow, int column, int number)
    {
        for(int row = 0; row < SudokuBoard.Size; row++)
        {
            if(row == currentRow)
            {
                continue;
            }

            if(board.GetValue(row, column) == number)
            {
                return false;
            }
        }
        return true;
    }

    private bool IsBoxValid(SudokuBoard board, int currentRow, int currentColumn, int number)
    {
        int boxStartRow = (currentRow / 3) * 3;
        int boxStartColumn = (currentColumn / 3) * 3;

        for(int row = boxStartRow; row < boxStartRow + 3; row++)
        {
            for(int column = boxStartColumn; column < boxStartColumn + 3; column++)
            {
                if(row == currentRow && column == currentColumn)
                {
                    continue;
                }

                if(board.GetValue(row, column) == number)
                {
                    return false;
                }
            }
        }
        return true;
    }
    
}