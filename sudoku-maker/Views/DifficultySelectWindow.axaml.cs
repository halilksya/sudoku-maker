using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using sudoku_maker.Models;

namespace sudoku_maker.Views;

public partial class DifficultySelectWindow : Window
{
    public DifficultySelectWindow()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void EasyButton_Click(object? sender, RoutedEventArgs e) => Close(Difficulty.Easy);
    private void MediumButton_Click(object? sender, RoutedEventArgs e) => Close(Difficulty.Medium);
    private void HardButton_Click(object? sender, RoutedEventArgs e) => Close(Difficulty.Hard);
    private void CancelButton_Click(object? sender, RoutedEventArgs e) => Close(null);
}