using System;
using sudoku_maker.Models;

namespace sudoku_maker.ViewModels;

public class LeaderboardEntryViewModel
{
    public SaveGame SaveGame { get; }
    public int Rank { get; }

    public int Score => SaveGame.Score;

    public string FormattedTime
    {
        get
        {
            var timeSpan = TimeSpan.FromSeconds(SaveGame.TimeElapsed);
            return timeSpan.TotalHours >= 1
                ? timeSpan.ToString(@"hh\:mm\:ss")
                : timeSpan.ToString(@"mm\:ss");
        }
    }

    public string DisplayText => $"{Rank}. {FormattedTime} - {Score} pts";

    public LeaderboardEntryViewModel(SaveGame saveGame, int rank)
    {
        SaveGame = saveGame;
        Rank = rank;
    }
}