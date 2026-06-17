using CommunityToolkit.Mvvm.ComponentModel;
using Avalonia;

namespace sudoku_maker.ViewModels;

public partial class SudokuCellViewModel : ObservableObject
{
    [ObservableProperty]
    private int row;
    [ObservableProperty]
    private int column;
    [ObservableProperty]
    private string value = "";

    [ObservableProperty]
    private bool isEditable;

    public Thickness BorderThickness => new(
        right: (Column == 2 || Column == 5) ? 4 : 1,
        bottom: (Row == 2 || Row == 5) ? 6 : 1,
        left: 1,
        top: (Row == 0 || Row == 3 || Row == 6) ? 2 : 1);
}
