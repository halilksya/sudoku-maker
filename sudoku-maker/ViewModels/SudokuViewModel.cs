using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using sudoku_maker.Models;
using sudoku_maker.Services;

namespace sudoku_maker.ViewModels;

public partial class SudokuViewModel : ObservableObject
{
    private readonly SudokuGenerator _generator = new();

    [ObservableProperty]
    private ObservableCollection<SudokuCellViewModel> _cells = new();

    [ObservableProperty]
    private Difficulty selectedDifficulty = Difficulty.Medium;

    public SudokuViewModel()
    {
        NewGame();
    }

    [RelayCommand]
    private void NewGame()
    {
        var sudoku = _generator.GeneratePuzzle(SelectedDifficulty);
        
        Cells.Clear();

        for (int row = 0; row < SudokuBoard.Size; row++)
        {
            for (int column = 0; column < SudokuBoard.Size; column++)
            {
                int value = sudoku.Puzzle.GetValue(row, column);
                int solutionValue = sudoku.Solution.GetValue(row, column);
                Cells.Add(new SudokuCellViewModel(row, column, value, solutionValue));
            }
        }
    }

    [RelayCommand]
    private void Check()
    {
        foreach (var cell in Cells)
        {
            if(cell.IsGiven)
            {
                continue;
            }

            cell.HasError = !cell.IsCorrect();
        }
    }

    [RelayCommand]
    private void Reset()
    {
        foreach (var cell in Cells)
        {
            cell.ClearUserValue();
        }
    }

    [RelayCommand]
    private void Solve()
    {
        foreach (var cell in Cells)
        {
            cell.ShowSolution();
        }
    }

    [RelayCommand]
    private void Hint()
    {
        var cell = Cells.FirstOrDefault(c => !c.IsGiven && c.GetNumberValue() != c.SolutionValue);

        if(cell == null)
        {
            return;
        }

        cell.ShowSolution();
    }
}