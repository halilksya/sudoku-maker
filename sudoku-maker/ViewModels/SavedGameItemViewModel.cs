using System;
using sudoku_maker.Models;

namespace sudoku_maker.ViewModels;

public class SavedGameItemViewModel
{
    public SaveGame SaveGame { get; }
    public string Id => SaveGame.Id;
    public Difficulty Difficulty => SaveGame.Difficulty;
    public DateTime UpdatedAt => SaveGame.UpdatedAt;
    public string DisplayText => $"{Difficulty} - {UpdatedAt.ToString("dd.MM.yyyy HH:mm")}";

    public SavedGameItemViewModel(SaveGame saveGame)
    {
        SaveGame = saveGame;
    }
}