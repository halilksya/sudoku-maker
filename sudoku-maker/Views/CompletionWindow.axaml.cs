using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using sudoku_maker.ViewModels;

namespace sudoku_maker.Views;

public partial class CompletionWindow : Window
{
    public CompletionWindow() : this(0)
    {
    }

    public CompletionWindow(int score)
    {
        InitializeComponent();
        DataContext = new CompletionWindowViewModel(score);
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void YesButton_Click(object? sender, RoutedEventArgs e) => Close(true);
    private void NoButton_Click(object? sender, RoutedEventArgs e) => Close(false);
}