using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using Avalonia;

namespace sudoku_maker.ViewModels;

public partial class SudokuCellViewModel : ObservableObject
{
    public int Row { get; }
    public int Column { get; }
    public int SolutionValue { get; }
    public bool IsGiven { get; }
    public bool IsEditable => !IsGiven;
    public bool ReadOnly => IsGiven;

    public bool BlockRightEdge => (Column + 1) % 3 == 0 && Column != 8;
    public bool BlockBottomEdge => (Row + 1) % 3 == 0 && Row != 8;

    public string PreviousValue { get; private set; } = string.Empty;

    public ObservableCollection<NoteDigitViewModel> Notes { get; } =
        new(Enumerable.Range(1, 9).Select(d => new NoteDigitViewModel(d)));

    public bool HasNotes => Notes.Any(n => n.IsSet);
    public bool ShowNotes => string.IsNullOrEmpty(Value) && !IsGiven && HasNotes;

    [ObservableProperty]
    private string _value = string.Empty;

    [ObservableProperty]
    private bool hasError;

    [ObservableProperty]
    private bool hasConflict;

    [ObservableProperty]
    private bool isSelected;

    public SudokuCellViewModel(int row, int column, int value, int solutionValue, bool isGiven)
    {
        Row = row;
        Column = column;
        SolutionValue = solutionValue;
        IsGiven = isGiven;
        this._value = value == 0 ? string.Empty : value.ToString();
    }

    partial void OnValueChanging(string value)
    {
        PreviousValue = Value;
    }

    partial void OnValueChanged(string value)
    {
        if(IsGiven)
        {
            return;
        }

        if (string.IsNullOrEmpty(value))
        {
            HasError = false;
            OnPropertyChanged(nameof(ShowNotes));
            return;
        }

        if (value.Length > 1 || value[0] < '1' || value[0] > '9')
        {
            Value = string.Empty;
            return;
        }

        HasError = false;
        ClearNotes();
        OnPropertyChanged(nameof(ShowNotes));
    }

    public int GetNumberValue()
    {
        if (int.TryParse(Value, out int number))
        {
            return number;
        }

        return 0;
    }

    public void ClearUserValue()
    {
        if (!IsGiven)
        {
            Value = string.Empty;
            HasError = false;
        }   
    }

    public void ShowSolution()
    {
        Value = SolutionValue.ToString();
        HasError = false;
    }

    public bool IsCorrect()
    {
        if(IsGiven)
        {
            return true;
        }

        int currentValue = GetNumberValue();

        return currentValue == 0 || currentValue == SolutionValue;
    }

    public void ToggleNote(int digit)
    {
        if (IsGiven || !string.IsNullOrEmpty(Value))
        {
            return;
        }

        var note = Notes.FirstOrDefault(n => n.Digit == digit);

        if (note != null)
        {
            note.IsSet = !note.IsSet;
            OnPropertyChanged(nameof(HasNotes));
            OnPropertyChanged(nameof(ShowNotes));
        }
    }

    public void ClearNotes()
    {
        foreach (var note in Notes)
        {
            note.IsSet = false;
        }

        OnPropertyChanged(nameof(HasNotes));
        OnPropertyChanged(nameof(ShowNotes));
    }
}