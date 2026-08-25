using Avalonia.Controls;
using Avalonia.Interactivity;
using sudoku_maker.Models;
using sudoku_maker.Views;

namespace sudoku_maker;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private async void Create_New_Sudoku_Button_Click(object? sender, RoutedEventArgs e)
    {
        var difficultySelectWindow = new DifficultySelectWindow();
        var selectedDifficulty = await difficultySelectWindow.ShowDialog<Difficulty?>(this);

        if (selectedDifficulty == null)
        {
            return;
        }

        Content = new SudokuView(selectedDifficulty.Value);
    }

    private async void Continue_Sudoku_Button_Click(object? sender, RoutedEventArgs e)
    {
        var saveGame = await SudokuView.PickSavedGameAsync(this);

        if (saveGame == null)
        {
            return;
        }

        var sudokuView = new SudokuView(saveGame.Difficulty);
        sudokuView.LoadSaveGame(saveGame);
        Content = sudokuView;
    }

    private void Leaderboard_Button_Click(object? sender, RoutedEventArgs e)
    {
        Content = new LeaderboardView();
    }

    private void LanguageButton_Click(object? sender, RoutedEventArgs e)
    {
        var service = sudoku_maker.Services.LocalizationService.Instance;

        service.CurrentLanguage = service.CurrentLanguage == sudoku_maker.Services.AppLanguage.English
            ? sudoku_maker.Services.AppLanguage.Turkish
            : sudoku_maker.Services.AppLanguage.English;
    }

    private void Exit_Button_Click(object? sender, RoutedEventArgs e)
    {
        this.Close();
    }
}