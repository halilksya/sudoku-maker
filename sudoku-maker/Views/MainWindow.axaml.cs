using Avalonia.Controls;
using sudoku_maker.Models;
using sudoku_maker.Views;

namespace sudoku_maker;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private async void Create_New_Sudoku_Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var difficultySelectWindow = new DifficultySelectWindow();
        var selectedDifficulty = await difficultySelectWindow.ShowDialog<Difficulty?>(this);

        if (selectedDifficulty == null)
        {
            return;
        }

        Content = new SudokuView(selectedDifficulty.Value);
    }

    private async void Continue_Sudoku_Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
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

    private void Exit_Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        this.Close();
    }
}