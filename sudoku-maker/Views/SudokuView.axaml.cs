using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using Avalonia.Interactivity;
using System.Threading.Tasks;
using sudoku_maker.ViewModels;
using sudoku_maker.Models;

namespace sudoku_maker.Views;

public partial class SudokuView : UserControl
{
    public SudokuView()
        : this(null)
    {
    }

    public SudokuView(Difficulty? initialDifficulty = null)
    {
        InitializeComponent();
        
        var viewModel = new SudokuViewModel(initialDifficulty);
        viewModel.OpenSavedGamesRequested = OpenSavedGames;
        viewModel.AskToSaveChanges = AskSaveChangesAsync;
        viewModel.AskForDifficulty = AskDifficultyAsync;
        viewModel.ShowCompletionAndAskNewGame = ShowCompletionAndAskNewGameAsync;
        DataContext = viewModel;

        Loaded += (sender, args) => Focus();
    }

    public void LoadSaveGame(SaveGame saveGame)
    {
        if (DataContext is SudokuViewModel sudokuViewModel)
        {
            sudokuViewModel.LoadSaveGame(saveGame);
        }
    }

    public static Task<SaveGame?> PickSavedGameAsync(Window owner)
    {
        var savedGamesViewModel = new SavedGameViewModel();
        Window? window = null;

        savedGamesViewModel.SaveGameSelected = saveGame =>
        {
            window?.Close(saveGame);
        };

        savedGamesViewModel.CancelRequested = () =>
        {
            window?.Close((SaveGame?)null);
        };

        window = new Window
        {
            Title = "Saved Games",
            Width = 480,
            Height = 540,
            MinWidth = 440,
            MinHeight = 500,
            CanResize = false,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            Content = new SavedGamesView
            {
                DataContext = savedGamesViewModel
            }
        };

        return window.ShowDialog<SaveGame?>(owner);
    }
    
    private void OpenSavedGames()
    {
        if (TopLevel.GetTopLevel(this) is not Window owner)
        {
            return;
        }

        _ = OpenSavedGamesAsync(owner);
    }

    private async Task OpenSavedGamesAsync(Window owner)
    {
        var saveGame = await PickSavedGameAsync(owner);

        if (saveGame == null)
        {
            return;
        }

        LoadSaveGame(saveGame);
    }

    private Window? GetOwnerWindow()
    {
        if (TopLevel.GetTopLevel(this) is Window topLevelOwner)
        {
            return topLevelOwner;
        }

        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            return desktop.MainWindow;
        }

        return null;
    }

    private async Task<SavePromptResult> AskSaveChangesAsync()
    {
        var owner = GetOwnerWindow();

        if (owner == null)
        {
            return SavePromptResult.Cancel;
        }

        var savePromptWindow = new SavePromptWindow();
        return await savePromptWindow.ShowDialog<SavePromptResult>(owner);
    }

    private async Task<Difficulty?> AskDifficultyAsync()
    {
        var owner = GetOwnerWindow();

        if (owner == null)
        {
            return null;
        }

        var difficultySelectWindow = new DifficultySelectWindow();
        return await difficultySelectWindow.ShowDialog<Difficulty?>(owner);
    }

    private async Task<bool> ShowCompletionAndAskNewGameAsync(int score)
    {
        var completionWindow = new CompletionWindow(score);

        try
        {
            var owner = GetOwnerWindow();
            if (owner != null)
            {
                return await completionWindow.ShowDialog<bool>(owner);
            }

            completionWindow.Show();
            return false;
        }
        catch
        {
            try
            {
                completionWindow.Show();
            }
            catch
            {
            }

            return false;
        }
    }

    private void Back_Button_Click(object? sender, RoutedEventArgs e)
    {
        if (TopLevel.GetTopLevel(this) is Window currentWindow)
        {
            var mainWindow = new MainWindow();

            mainWindow.Show();

            currentWindow.Close();
        }
    }

    private void Cell_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (sender is Border border &&
            border.DataContext is SudokuCellViewModel cell &&
            DataContext is SudokuViewModel viewModel)
        {
            viewModel.SelectCell(cell);
            Focus();
        }
    }

    private void SudokuView_KeyDown(object? sender, KeyEventArgs e)
    {
        if (DataContext is not SudokuViewModel viewModel)
        {
            return;
        }

        switch (e.Key)
        {
            case Key.D1: case Key.NumPad1: viewModel.EnterDigit(1); break;
            case Key.D2: case Key.NumPad2: viewModel.EnterDigit(2); break;
            case Key.D3: case Key.NumPad3: viewModel.EnterDigit(3); break;
            case Key.D4: case Key.NumPad4: viewModel.EnterDigit(4); break;
            case Key.D5: case Key.NumPad5: viewModel.EnterDigit(5); break;
            case Key.D6: case Key.NumPad6: viewModel.EnterDigit(6); break;
            case Key.D7: case Key.NumPad7: viewModel.EnterDigit(7); break;
            case Key.D8: case Key.NumPad8: viewModel.EnterDigit(8); break;
            case Key.D9: case Key.NumPad9: viewModel.EnterDigit(9); break;
            case Key.Delete:
            case Key.Back:
                viewModel.ClearSelectedCell();
                break;
            case Key.Up: viewModel.MoveSelection(-1, 0); break;
            case Key.Down: viewModel.MoveSelection(1, 0); break;
            case Key.Left: viewModel.MoveSelection(0, -1); break;
            case Key.Right: viewModel.MoveSelection(0, 1); break;
            case Key.N:
                viewModel.ToggleNoteModeCommand.Execute(null);
                break;
            case Key.Z when e.KeyModifiers.HasFlag(KeyModifiers.Control) && e.KeyModifiers.HasFlag(KeyModifiers.Shift):
                viewModel.RedoCommand.Execute(null);
                break;
            case Key.Z when e.KeyModifiers.HasFlag(KeyModifiers.Control):
                viewModel.UndoCommand.Execute(null);
                break;
            case Key.Y when e.KeyModifiers.HasFlag(KeyModifiers.Control):
                viewModel.RedoCommand.Execute(null);
                break;
            default:
                return;
        }

        e.Handled = true;
    }
}