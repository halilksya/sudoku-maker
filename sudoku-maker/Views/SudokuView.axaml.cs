using Avalonia.Controls;
using sudoku_maker.ViewModels;

namespace sudoku_maker.Views;

public partial class SudokuView : UserControl
{
    public SudokuView()
    {
        InitializeComponent();
        DataContext = new SudokuViewModel();
    }
}