using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using sudoku_maker.Models;

namespace sudoku_maker.Views;

public partial class SavePromptWindow : Window
{
    public SavePromptWindow()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void SaveButton_Click(object? sender, RoutedEventArgs e) => Close(SavePromptResult.Save);
    private void DontSaveButton_Click(object? sender, RoutedEventArgs e) => Close(SavePromptResult.DontSave);
    private void CancelButton_Click(object? sender, RoutedEventArgs e) => Close(SavePromptResult.Cancel);
}