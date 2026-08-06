using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using sudoku_maker.Models;
using sudoku_maker.Services;

namespace sudoku_maker.ViewModels;

public partial class LeaderboardViewModel : ObservableObject
{
    private readonly SaveGameService _saveGameService = new();

    [ObservableProperty]
    private ObservableCollection<LeaderboardEntryViewModel> _easyEntries = new();

    [ObservableProperty]
    private ObservableCollection<LeaderboardEntryViewModel> _mediumEntries = new();

    [ObservableProperty]
    private ObservableCollection<LeaderboardEntryViewModel> _hardEntries = new();

    [ObservableProperty]
    private bool _sortByScore = true;

    public LeaderboardViewModel()
    {
        Load();
    }

    [RelayCommand]
    private void SortByScoreOption()
    {
        SortByScore = true;
        Load();
    }

    [RelayCommand]
    private void SortByTimeOption()
    {
        SortByScore = false;
        Load();
    }

    private void Load()
    {
        var completedSaves = _saveGameService.GetAll().Where(s => s.IsCompleted).ToList();

        EasyEntries = BuildEntries(completedSaves.Where(s => s.Difficulty == Difficulty.Easy));
        MediumEntries = BuildEntries(completedSaves.Where(s => s.Difficulty == Difficulty.Medium));
        HardEntries = BuildEntries(completedSaves.Where(s => s.Difficulty == Difficulty.Hard));
    }

    private ObservableCollection<LeaderboardEntryViewModel> BuildEntries(IEnumerable<SaveGame> saves)
    {
        var ordered = SortByScore
            ? saves.OrderByDescending(s => s.Score).Take(10)
            : saves.OrderBy(s => s.TimeElapsed).Take(10);

        var result = new ObservableCollection<LeaderboardEntryViewModel>();
        int rank = 1;

        foreach (var save in ordered)
        {
            result.Add(new LeaderboardEntryViewModel(save, rank));
            rank++;
        }

        return result;
    }
}