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

    private void Continue_Sudoku_Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Content = new SudokuView();
    }

    private void Exit_Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        this.Close();
    }
}