using sudoku_maker.Models;

namespace sudoku_maker.Services;

public class SudokuSolver
{
    private readonly SudokuValidator _validator = new();

    public bool Solve(SudokuBoard board)
    {
        var emptyCell = FindEmptyCell(board);

        if(emptyCell == null)
        {
            return true;
        }

        int row = emptyCell.Value.row;
        int column = emptyCell.Value.column;

        for(int number = 1; number <= SudokuBoard.Size; number++)
        {
            if(_validator.IsValidMove(board, row, column, number))
            {
                board.SetValue(row, column, number);

                if(Solve(board))
                {
                    return true;
                }

                board.SetValue(row, column, SudokuBoard.Empty);
            }
        }

        return false;
    }

    public int CountSolutions(SudokuBoard board)
    {
        var copy = board.Clone();
        int count = 0;

        CountSolutionsRecursive(copy, ref count, 2);

        return count;
    }

    private void CountSolutionsRecursive(SudokuBoard board, ref int count, int maxCount)
    {
        if(count >= maxCount)
        {
            return;
        }

        var emptyCell = FindEmptyCell(board);

        if(emptyCell == null)
        {
            count++;
            return;
        }

        int row = emptyCell.Value.row;
        int column = emptyCell.Value.column;

        for(int number = 1; number <= SudokuBoard.Size; number++)
        {
            if(_validator.IsValidMove(board, row, column, number))
            {
                board.SetValue(row, column, number);
                CountSolutionsRecursive(board, ref count, maxCount);
                board.SetValue(row, column, SudokuBoard.Empty);
            }
        }
    }

    public (int row, int column)? FindEmptyCell(SudokuBoard board)
    {
        for(int row = 0; row < SudokuBoard.Size; row++)
        {
            for(int column = 0; column < SudokuBoard.Size; column++)
            {
                if(board.IsEmpty(row, column))
                {
                    return (row, column);
                }
            }
        }
        return null;
    }
}