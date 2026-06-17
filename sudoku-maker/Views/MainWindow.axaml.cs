using Avalonia.Controls;
using sudoku_maker.Views;

namespace sudoku_maker;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Button_SizeChanged(object? sender, SizeChangedEventArgs e)
    {
    }

    private void Create_New_Sudoku_Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Content = new SudokuView();
    }

    private void Continue_Sudoku_Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        // Implement functionality to continue a saved Sudoku game
    }

    private void Exit_Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        this.Close();
    }
}