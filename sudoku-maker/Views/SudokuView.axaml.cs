using Avalonia.Controls;
using Avalonia.VisualTree;
using sudoku_maker.Views;
using sudoku_maker.ViewModels;

namespace sudoku_maker.Views;

public partial class SudokuView : UserControl
{
    public SudokuView()
    {
        InitializeComponent();
        DataContext = new SudokuViewModel();
    }

    private void Back_Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (TopLevel.GetTopLevel(this) is Window currentWindow)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            currentWindow.Close();
        }
    }

    private void New_Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        DataContext = new SudokuViewModel();
    }

    private void Reset_Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        // Üşendim :o
    }
}
