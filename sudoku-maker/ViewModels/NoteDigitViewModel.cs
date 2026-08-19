using CommunityToolkit.Mvvm.ComponentModel;

namespace sudoku_maker.ViewModels;

public partial class NoteDigitViewModel : ObservableObject
{
    public int Digit { get; }

    [ObservableProperty]
    private bool isSet;

    public string DisplayText => IsSet ? Digit.ToString() : string.Empty;

    public NoteDigitViewModel(int digit)
    {
        Digit = digit;
    }

    partial void OnIsSetChanged(bool value)
    {
        OnPropertyChanged(nameof(DisplayText));
    }
}