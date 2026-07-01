using Avalonia.Controls;
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
        DataContext = viewModel;
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

    private async Task<SavePromptResult> AskSaveChangesAsync()
    {
        if (TopLevel.GetTopLevel(this) is not Window owner)
        {
            return SavePromptResult.Cancel;
        }

        var savePromptWindow = new SavePromptWindow();
        return await savePromptWindow.ShowDialog<SavePromptResult>(owner);
    }

    private async Task<Difficulty?> AskDifficultyAsync()
    {
        if (TopLevel.GetTopLevel(this) is not Window owner)
        {
            return null;
        }

        var difficultySelectWindow = new DifficultySelectWindow();
        return await difficultySelectWindow.ShowDialog<Difficulty?>(owner);
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
}