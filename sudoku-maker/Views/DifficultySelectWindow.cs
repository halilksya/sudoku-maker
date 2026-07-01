using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Styling;
using sudoku_maker.Models;

namespace sudoku_maker.Views;

public class DifficultySelectWindow : Window
{
    public DifficultySelectWindow()
    {
        Title = "Select Difficulty";
        Width = 420;
        Height = 420;
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
            Text = "Please select a difficulty level:",
            FontSize = 18,
            FontWeight = FontWeight.Bold,
            TextAlignment = TextAlignment.Center,
            TextWrapping = TextWrapping.Wrap,
            HorizontalAlignment = HorizontalAlignment.Center,
            Margin = new Thickness(0, 0, 0, 16)
        };

        var easyButton = new Button
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            Content = "Easy",
            Width = 140,
        };

        var mediumButton = new Button
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            Content = "Medium",
            Width = 140,
        };

        var hardButton = new Button
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            Content = "Hard",
            Width = 140,
        };

        var cancelButton = new Button
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            Content = "Cancel",
            Width = 140,
        };

        easyButton.Click += (sender, e) =>
        {
            Close(Difficulty.Easy);
        };

        mediumButton.Click += (sender, e) =>
        {
            Close(Difficulty.Medium);
        };

        hardButton.Click += (sender, e) =>
        {
            Close(Difficulty.Hard);
        };

        cancelButton.Click += (sender, e) =>
        {
            Close(null);
        };

        Content = new Grid
        {
            Background = Brushes.LightGray,
            Children =
            {
                new Border
                {
                    Background = Brushes.White,
                    Width = 340,
                    Padding = new Thickness(20),
                    CornerRadius = new CornerRadius(10),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Child = new StackPanel
                    {
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        Spacing = 12,
                        Children =
                        {
                            text,
                            easyButton,
                            mediumButton,
                            hardButton,
                            cancelButton
                        }
                    }
                }
            }
        };
    }
}