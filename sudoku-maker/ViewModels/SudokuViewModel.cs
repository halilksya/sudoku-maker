using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Avalonia.Threading;
using sudoku_maker.Models;
using sudoku_maker.Services;

namespace sudoku_maker.ViewModels;

public partial class SudokuViewModel : ObservableObject
{
    private readonly SudokuGenerator _generator = new();

    private readonly SaveGameService _saveGameService = new();
    private readonly DispatcherTimer _timer;
    private readonly Stopwatch _stopwatch = new();
    private int _timeElapsed;

    [ObservableProperty]
    private ObservableCollection<SudokuCellViewModel> _cells = new();

    [ObservableProperty]
    private Difficulty selectedDifficulty = Difficulty.Medium;

    [ObservableProperty]
    private string? currentSaveId;

    [ObservableProperty]
    private bool hasUnsavedChanges;

    public Action? OpenSavedGamesRequested { get; set; }
    public Func<Task<SavePromptResult>>? AskToSaveChanges { get; set; }

    public Func<Task<Difficulty?>>? AskForDifficulty { get; set; }

    public SudokuViewModel(Difficulty? initialDifficulty = null)
    {
        _timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(1)
        };

        _timer.Tick += (sender, args) =>
        {
            OnPropertyChanged(nameof(FormattedElaspedTime));
        };

        if (initialDifficulty.HasValue)
        {
            SelectedDifficulty = initialDifficulty.Value;
        }

        CreateNewGame();
    }

    [RelayCommand]
    private async Task NewGame()
    {
        if (!await CheckForUnsavedChanges())
        {
            return;
        }

        var difficulty = AskForDifficulty == null
            ? SelectedDifficulty
            : await AskForDifficulty();

        if (difficulty == null)
        {
            return;
        }

        SelectedDifficulty = difficulty.Value;
        CreateNewGame();
    }

    private void CreateNewGame()
    {
        var sudoku = _generator.GeneratePuzzle(SelectedDifficulty);

        Cells.Clear();

        for (int row = 0; row < SudokuBoard.Size; row++)
        {
            for (int column = 0; column < SudokuBoard.Size; column++)
            {
                int value = sudoku.Puzzle.GetValue(row, column);
                int solutionValue = sudoku.Solution.GetValue(row, column);

                var cell = new SudokuCellViewModel(row, column, value, solutionValue);

                cell.PropertyChanged += (sender, args) =>
                {
                    if (args.PropertyName == nameof(SudokuCellViewModel.Value))
                    {
                        HasUnsavedChanges = true;
                    }
                };

                Cells.Add(cell);
            }
        }

        CurrentSaveId = null;
        HasUnsavedChanges = true;
        StartTimer(0);
    }

    private SaveGame CreateSaveGameFromCurrentState()
    {
        var saveGame = new SaveGame
        {
            Difficulty = SelectedDifficulty,
            IsCompleted = false,
            TimeElapsed = _timeElapsed + (int)_stopwatch.Elapsed.TotalSeconds
        };

        if(!string.IsNullOrEmpty(CurrentSaveId))
        {
            saveGame.Id = CurrentSaveId;
        }

        foreach(var cell in Cells)
        {
            saveGame.Board[cell.Row][cell.Column] = cell.IsGiven ? cell.SolutionValue : 0;
            saveGame.CurrentBoard[cell.Row][cell.Column] = cell.GetNumberValue();
            saveGame.Solution[cell.Row][cell.Column] = cell.SolutionValue;
        }

        return saveGame;
    }

    [RelayCommand]
    private void SaveGame()
    {
        try
        {
            var saveGame = CreateSaveGameFromCurrentState();
            var savedGame = _saveGameService.CreateOrUpdate(saveGame);
            CurrentSaveId = savedGame.Id;
            HasUnsavedChanges = false;
        }
        catch
        {
            HasUnsavedChanges = true;
        }
    }

    private async Task<bool> CheckForUnsavedChanges()
    {
        if (HasUnsavedChanges)
        {
            var result = AskToSaveChanges == null
                ? SavePromptResult.Cancel
                : await AskToSaveChanges();

            if (result == SavePromptResult.Save)
            {
                SaveGame();
                return true;
            }
            else if (result == SavePromptResult.DontSave)
            {
                return true;
            }
            else if (result == SavePromptResult.Cancel)
            {
                return false;
            }
        }
        return true;
    }

    [RelayCommand]
    private async Task OpenSavedGames()
    {
        if (await CheckForUnsavedChanges())
        {
            OpenSavedGamesRequested?.Invoke();
        }
    }

    public void LoadSaveGame(SaveGame saveGame)
    {
        Cells.Clear();

        SelectedDifficulty = saveGame.Difficulty;
        CurrentSaveId = saveGame.Id;

        for(int row = 0; row < SudokuBoard.Size; row++)
        {
            for(int column = 0; column < SudokuBoard.Size; column++)
            {
                int value = saveGame.Board[row][column];
                int currentValue = saveGame.CurrentBoard[row][column];
                int solutionValue = saveGame.Solution[row][column];

                int cellValue = currentValue != 0 ? currentValue : value;

                var cell = new SudokuCellViewModel(row, column, cellValue, solutionValue);

                cell.PropertyChanged += (sender, args) =>
                {
                    if (args.PropertyName == nameof(SudokuCellViewModel.Value))
                    {
                        HasUnsavedChanges = true;
                    }
                };

                Cells.Add(cell);
            }
        }
        HasUnsavedChanges = false;
        StartTimer(saveGame.TimeElapsed);
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

        HasUnsavedChanges = true;
    }

    [RelayCommand]
    private void Solve()
    {
        foreach (var cell in Cells)
        {
            cell.ShowSolution();
        }

        HasUnsavedChanges = true;
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

        HasUnsavedChanges = true;
    }

    public string FormattedElaspedTime
    {
        get
        {
            var totalSecond = _timeElapsed + (int)_stopwatch.Elapsed.TotalSeconds;
            var timeSpan = TimeSpan.FromSeconds(totalSecond);

            if(timeSpan.TotalHours >= 1)
            {
                return timeSpan.ToString(@"hh\:mm\:ss");
            }
            else
            {
                return timeSpan.ToString(@"mm\:ss");
            }
        }
    }

    private void StartTimer(int elapsedSeconds = 0)
    {
        _timeElapsed = elapsedSeconds;
        _stopwatch.Restart();

        if (!_timer.IsEnabled)
        {
            _timer.Start();
        }
        OnPropertyChanged(nameof(FormattedElaspedTime));
    }
}