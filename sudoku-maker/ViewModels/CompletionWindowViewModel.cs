namespace sudoku_maker.ViewModels;

public class CompletionWindowViewModel
{
    public string ScoreText { get; }

    public CompletionWindowViewModel(int score)
    {
        ScoreText = $"Score: {score}";
    }
}