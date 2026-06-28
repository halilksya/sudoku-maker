using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Threading.Tasks;
using sudoku_maker.ViewModels;
using sudoku_maker.Models;

namespace sudoku_maker.Views;

public partial class SudokuView : UserControl
{
    public SudokuView(Difficulty? initialDifficulty = null)
    {
        InitializeComponent();
        
        var viewModel = new SudokuViewModel(initialDifficulty);
        viewModel.OpenSavedGamesRequested = OpenSavedGames;
        viewModel.AskToSaveChanges = AskSaveChangesAsync;
        viewModel.AskForDifficulty = AskDifficultyAsync;
        DataContext = viewModel;
    }
    
    private void OpenSavedGames()
    {
        var savedGamesViewModel = new SavedGameViewModel();

        Window? window = null;

        savedGamesViewModel.SaveGameSelected = (saveGame) =>
        {
            if(DataContext is SudokuViewModel sudokuViewModel)
            {
                sudokuViewModel.LoadSaveGame(saveGame);
            }

            window?.Close();
        };

        savedGamesViewModel.CancelRequested = () =>
        {
            window?.Close();
        };

        window = new Window
        {
            Title = "Saved Games",
            Width = 400,
            Height = 500,
            Content = new SavedGamesView
            {
                DataContext = savedGamesViewModel
            }
        };

        window.Show();
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