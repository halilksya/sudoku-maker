using System.Reflection;
using sudoku_maker.Models;
using sudoku_maker.ViewModels;

var vm = new SudokuViewModel(Difficulty.Easy);
var cells = new System.Collections.ObjectModel.ObservableCollection<SudokuCellViewModel>();
for (int r = 0; r < 9; r++)
for (int c = 0; c < 9; c++)
{
    var cell = new SudokuCellViewModel(r, c, 1, 1, false);
    cells.Add(cell);
}
vm.Cells = cells;
vm.ShowCompletionAndAskNewGame = _ => Task.FromResult(true);

var method = typeof(SudokuViewModel).GetMethod("CheckForCompletion", BindingFlags.NonPublic | BindingFlags.Instance)\!;
method.Invoke(vm, null);

Console.WriteLine($"All solved? {cells.All(c => c.GetNumberValue() == c.SolutionValue)}");
Console.WriteLine($"Count: {cells.Count}");
Console.WriteLine($"First value: {cells[0].Value}");
