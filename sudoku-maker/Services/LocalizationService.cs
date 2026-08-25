using System.Collections.Generic;
using System.ComponentModel;

namespace sudoku_maker.Services;

public enum AppLanguage
{
    English,
    Turkish
}

public class LocalizationService : INotifyPropertyChanged
{
    public static LocalizationService Instance { get; } = new();

    public event PropertyChangedEventHandler? PropertyChanged;

    private AppLanguage _currentLanguage = AppLanguage.English;

    public AppLanguage CurrentLanguage
    {
        get => _currentLanguage;
        set
        {
            if (_currentLanguage != value)
            {
                _currentLanguage = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentLanguage)));
            }
        }
    }

    public string Get(string key)
    {
        if (_translations.TryGetValue(key, out var languages) &&
            languages.TryGetValue(CurrentLanguage, out var text))
        {
            return text;
        }

        return key;
    }

    private readonly Dictionary<string, Dictionary<AppLanguage, string>> _translations = new()
    {
        ["NewGame"] = new() { [AppLanguage.English] = "Create New Sudoku", [AppLanguage.Turkish] = "Yeni Sudoku Oluştur" },
        ["ContinueGame"] = new() { [AppLanguage.English] = "Continue Sudoku", [AppLanguage.Turkish] = "Sudoku'ya Devam Et" },
        ["Leaderboard"] = new() { [AppLanguage.English] = "Leaderboard", [AppLanguage.Turkish] = "Lider Tablosu" },
        ["Exit"] = new() { [AppLanguage.English] = "Exit", [AppLanguage.Turkish] = "Çıkış" },
        ["Back"] = new() { [AppLanguage.English] = "Back", [AppLanguage.Turkish] = "Geri" },
        ["Notes"] = new() { [AppLanguage.English] = "Notes", [AppLanguage.Turkish] = "Notlar" },
        ["Undo"] = new() { [AppLanguage.English] = "Undo", [AppLanguage.Turkish] = "Geri Al" },
        ["Redo"] = new() { [AppLanguage.English] = "Redo", [AppLanguage.Turkish] = "İleri Al" },
        ["ExportPdf"] = new() { [AppLanguage.English] = "Export PDF", [AppLanguage.Turkish] = "PDF'e Aktar" },
        ["SudokuMakerTitle"] = new() { [AppLanguage.English] = "Sudoku Maker", [AppLanguage.Turkish] = "Sudoku Maker" },
        ["Congratulations"] = new() { [AppLanguage.English] = "Congratulations, you solved the puzzle!", [AppLanguage.Turkish] = "Tebrikler, bulmacayı çözdünüz!" },
        ["NewPuzzleQuestion"] = new() { [AppLanguage.English] = "Would you like to start a new puzzle?", [AppLanguage.Turkish] = "Yeni bir bulmaca başlatmak ister misiniz?" },
        ["Yes"] = new() { [AppLanguage.English] = "Yes, new puzzle", [AppLanguage.Turkish] = "Evet, yeni bulmaca" },
        ["No"] = new() { [AppLanguage.English] = "No, stay here", [AppLanguage.Turkish] = "Hayır, burada kal" },
        ["Score"] = new() { [AppLanguage.English] = "Score", [AppLanguage.Turkish] = "Puan" },
        ["SortByScore"] = new() { [AppLanguage.English] = "Sort by Score", [AppLanguage.Turkish] = "Puana Göre Sırala" },
        ["SortByTime"] = new() { [AppLanguage.English] = "Sort by Time", [AppLanguage.Turkish] = "Süreye Göre Sırala" },
        ["Easy"] = new() { [AppLanguage.English] = "Easy", [AppLanguage.Turkish] = "Kolay" },
        ["Medium"] = new() { [AppLanguage.English] = "Medium", [AppLanguage.Turkish] = "Orta" },
        ["Hard"] = new() { [AppLanguage.English] = "Hard", [AppLanguage.Turkish] = "Zor" },
        ["SavedGames"] = new() { [AppLanguage.English] = "Saved Games", [AppLanguage.Turkish] = "Kayıtlı Oyunlar" },
        ["Open"] = new() { [AppLanguage.English] = "Open", [AppLanguage.Turkish] = "Aç" },
        ["Delete"] = new() { [AppLanguage.English] = "Delete", [AppLanguage.Turkish] = "Sil" },
        ["Cancel"] = new() { [AppLanguage.English] = "Cancel", [AppLanguage.Turkish] = "İptal" },
        ["SelectDifficulty"] = new() { [AppLanguage.English] = "Select Difficulty", [AppLanguage.Turkish] = "Zorluk Seç" },
        ["SelectDifficultyPrompt"] = new() { [AppLanguage.English] = "Please select a difficulty level:", [AppLanguage.Turkish] = "Lütfen bir zorluk seviyesi seçin:" },
        ["SaveChanges"] = new() { [AppLanguage.English] = "Save Changes", [AppLanguage.Turkish] = "Değişiklikleri Kaydet" },
        ["UnsavedChangesPrompt"] = new() { [AppLanguage.English] = "You have unsaved changes. Do you want to save them?", [AppLanguage.Turkish] = "Kaydedilmemiş değişiklikleriniz var. Kaydetmek ister misiniz?" },
        ["Save"] = new() { [AppLanguage.English] = "Save", [AppLanguage.Turkish] = "Kaydet" },
        ["DontSave"] = new() { [AppLanguage.English] = "Don't Save", [AppLanguage.Turkish] = "Kaydetme" },
        ["LanguageToggle"] = new() { [AppLanguage.English] = "Language: English", [AppLanguage.Turkish] = "Dil: Türkçe" },
        ["Points"] = new() { [AppLanguage.English] = "pts", [AppLanguage.Turkish] = "puan" },
        ["Hints"] = new() { [AppLanguage.English] = "hints", [AppLanguage.Turkish] = "ipucu" },
    };
}