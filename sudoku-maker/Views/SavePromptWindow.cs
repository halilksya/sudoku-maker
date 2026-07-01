using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Styling;
using sudoku_maker.Models;

namespace sudoku_maker.Views;

public class SavePromptWindow : Window
{
    public SavePromptWindow()
    {
        Title = "Save Changes";
        Width = 560;
        Height = 260;
        CanResize = false;
        WindowStartupLocation = WindowStartupLocation.CenterOwner;

        Styles.Add(new Style(x => x.OfType<Button>())
        {
            Setters =
            {
                new Setter(Button.BackgroundProperty, Brushes.LightBlue),
                new Setter(Button.ForegroundProperty, Brushes.Black),
                new Setter(Button.FontSizeProperty, 14d),
                new Setter(Button.BorderThicknessProperty, new Thickness(1)),
                new Setter(Button.BorderBrushProperty, Brushes.Gray),
                new Setter(Button.CornerRadiusProperty, new CornerRadius(5)),
                new Setter(Button.PaddingProperty, new Thickness(6, 4))
            }
        });

        var text = new TextBlock
        {
            Text = "You have unsaved changes. Do you want to save them?",
            FontSize = 15,
            TextAlignment = TextAlignment.Center,
            TextWrapping = TextWrapping.Wrap,
            HorizontalAlignment = HorizontalAlignment.Center,
            Margin = new Thickness(0, 0, 0, 16)
        };

        var saveButton = new Button
        {
            Content = "Save",
            Width = 120,
        };

        var dontSaveButton = new Button
        {
            Content = "Don't Save",
            Width = 120,
        };

        var cancelButton = new Button
        {
            Content = "Cancel",
            Width = 120,
        };

        saveButton.Click += (sender, e) =>
        {
            Close(SavePromptResult.Save);
        };

        dontSaveButton.Click += (sender, e) =>
        {
            Close(SavePromptResult.DontSave);
        };

        cancelButton.Click += (sender, e) =>
        {
            Close(SavePromptResult.Cancel);
        };

        var buttons = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            HorizontalAlignment = HorizontalAlignment.Center,
            Spacing = 12,
            Children =
            {
                saveButton,
                dontSaveButton,
                cancelButton
            }
        };

        Content = new Grid
        {
            Background = Brushes.LightGray,
            Children =
            {
                new Border
                {
                    Background = Brushes.White,
                    Width = 500,
                    Padding = new Thickness(20),
                    CornerRadius = new CornerRadius(10),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Child = new StackPanel
                    {
                        Spacing = 16,
                        Children =
                        {
                            text,
                            buttons
                        }
                    }
                }
            }
        };
    }
}