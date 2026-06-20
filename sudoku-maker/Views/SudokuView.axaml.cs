using Avalonia.Controls;
using Avalonia.Interactivity;
using sudoku_maker.ViewModels;

namespace sudoku_maker.Views;

public partial class SudokuView : UserControl
{
    public SudokuView()
    {
        InitializeComponent();
        DataContext = new SudokuViewModel();
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