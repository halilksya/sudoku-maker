using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace sudoku_maker.ViewModels;

public partial class SudokuViewModel : ObservableObject
{
    public ObservableCollection<SudokuCellViewModel> Cells { get; } = new();

    public SudokuViewModel()
    {
        CreateBoard();
    }

    private void CreateBoard()
    {
        for (int row = 0; row < 9; row++)
        {
            for (int column = 0; column < 9; column++)
            {
                Cells.Add(new SudokuCellViewModel
                {
                    Row = row,
                    Column = column,
                    Value = string.Empty,
                    IsEditable = true
                });
            }
        }
    }
}