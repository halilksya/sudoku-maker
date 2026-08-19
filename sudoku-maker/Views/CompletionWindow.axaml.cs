using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace sudoku_maker.Views;

public partial class CompletionWindow : Window
{
    public bool Result { get; private set; }

    public CompletionWindow() : this(0)
    {
    }

    public CompletionWindow(int score)
    {
        InitializeComponent();
        ScoreTextBlock.Text = $"Score: {score}";
        WindowStartupLocation = WindowStartupLocation.CenterScreen;
        CanResize = false;
        ShowInTaskbar = true;
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void YesButton_Click(object? sender, RoutedEventArgs e)
    {
        Result = true;
        Close();
    }

    private void NoButton_Click(object? sender, RoutedEventArgs e)
    {
        Result = false;
        Close();
    }
}