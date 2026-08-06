using Avalonia.Controls;
using Avalonia.Interactivity;
using sudoku_maker.ViewModels;

namespace sudoku_maker.Views;

public partial class LeaderboardView : UserControl
{
    public LeaderboardView()
    {
        InitializeComponent();
        DataContext = new LeaderboardViewModel();
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