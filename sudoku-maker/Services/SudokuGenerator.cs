using System;
using System.Collections.Generic;
using System.Linq;
using sudoku_maker.Models;

namespace sudoku_maker.Services;

public class SudokuGenerator
{
    private readonly Random _random = new();
    private readonly SudokuSolver _solver = new();

    public (SudokuBoard Puzzle, SudokuBoard Solution) GeneratePuzzle(Difficulty difficulty)
    {
        SudokuBoard solution = GenerateSolvedBoard();
        SudokuBoard puzzle = solution.Clone();

        int cellsToRemove = GetCellsToRemove(difficulty);
        RemoveCells(puzzle, cellsToRemove);

        return (puzzle, solution);
    }

    private SudokuBoard GenerateSolvedBoard()
    {
        SudokuBoard board = new();
        FillBoard(board);

        return board;
    }

    private bool FillBoard(SudokuBoard board)
    {
        var emptyCell = _solver.FindEmptyCell(board);

        if(emptyCell == null)
        {
            return true;
        }

        var (row, column) = emptyCell.Value;

        List<int> numbers = GetShuffledNumbers();

        foreach(int number in numbers)
        {
            SudokuValidator validator = new();

            if(validator.IsValidMove(board, row, column, number))
            {
                board.SetValue(row, column, number);

                if(FillBoard(board))
                {
                    return true;
                }

                board.SetValue(row, column, SudokuBoard.Empty);
            }
        }
        return false;
    }

    private void RemoveCells(SudokuBoard puzzle, int cellsToRemove)
    {
        List<(int row, int column)> positions = GetShuffledPositions();

        int removed = 0;

        foreach(var position in positions)
        {
            if(removed >= cellsToRemove)
            {
                break;
            }

            int oldValue = puzzle.GetValue(position.row, position.column);
            puzzle.SetValue(position.row, position.column, SudokuBoard.Empty);

            int solutionCount = _solver.CountSolutions(puzzle);

            if(solutionCount == 1)
            {
                removed++;
            }
            else
            {
                puzzle.SetValue(position.row, position.column, oldValue);
            }
        }
    }

    private int GetCellsToRemove(Difficulty difficulty)
    {
        return difficulty switch
        {
            Difficulty.Easy => 35,
            Difficulty.Medium => 45,
            Difficulty.Hard => 55,
            _ => 40
        };
    }

    private List<int> GetShuffledNumbers()
    {
        List<int> numbers = new();

        for(int number = 1; number <= SudokuBoard.Size; number++)
        {
            numbers.Add(number);
        }

        return numbers.OrderBy(_ => _random.Next()).ToList();
    }

    private List<(int Row, int Column)> GetShuffledPositions()
    {
        List<(int Row, int Column)> positions = new();

        for(int row = 0; row < SudokuBoard.Size; row++)
        {
            for(int column = 0; column < SudokuBoard.Size; column++)
            {
                positions.Add((row, column));
            }
        }

        return positions.OrderBy(_ => _random.Next()).ToList();
    }
}