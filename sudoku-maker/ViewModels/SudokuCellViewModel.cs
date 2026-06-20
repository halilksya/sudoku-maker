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

    [ObservableProperty]
    private string _value = string.Empty;

    [ObservableProperty]
    private bool hasError;

    public SudokuCellViewModel(int row, int column, int value, int solutionValue)
    {
        Row = row;
        Column = column;
        SolutionValue = solutionValue;
        IsGiven = value != 0;
        this._value = value == 0 ? string.Empty : value.ToString();
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
            return;
        }

        if (value.Length > 1 || value[0] < '1' || value[0] > '9')
        {
            Value = string.Empty;
            return;
        }

        HasError = false;
        
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
}
