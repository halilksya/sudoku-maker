using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using sudoku_maker.Models;
using sudoku_maker.Services;

namespace sudoku_maker.ViewModels;

public partial class SavedGameViewModel : ObservableObject
{
    private readonly SaveGameService _saveGameService = new();

    [ObservableProperty]
    private ObservableCollection<SavedGameItemViewModel> _savedGames = new();

    [ObservableProperty]
    private SavedGameItemViewModel? _selectedSavedGame;

    public SavedGameViewModel()
    {
        LoadSavedGames();
    }

    private void LoadSavedGames()
    {
        SavedGames.Clear();

        var saves = _saveGameService.GetAll().OrderByDescending(s => s.UpdatedAt);

        foreach (var save in saves)
        {
            SavedGames.Add(new SavedGameItemViewModel(save));
        }
    }

    public Action<SaveGame>? SaveGameSelected { get; set; }

    [RelayCommand]
    private void DeleteSelected()
    {
        if (SelectedSavedGame == null)
        {
            return;
        }

        _saveGameService.Delete(SelectedSavedGame.SaveGame.Id);

        LoadSavedGames();
    }

    [RelayCommand]
    private void OpenSelected()
    {
        if (SelectedSavedGame != null)
        {
            SaveGameSelected?.Invoke(SelectedSavedGame.SaveGame);
        }
    }

    public Action? CancelRequested { get; set; }
    [RelayCommand]
    private void Cancel()
    {
        CancelRequested?.Invoke();
    }
}