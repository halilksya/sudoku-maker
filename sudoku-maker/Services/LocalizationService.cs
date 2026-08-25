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
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Item[]"));
            }
        }
    }

    public string this[string key]
    {
        get
        {
            if (_translations.TryGetValue(key, out var languages) &&
                languages.TryGetValue(CurrentLanguage, out var text))
            {
                return text;
            }

            return key;
        }
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
        ["Congratulations"] = new() { [AppLanguage.English] = "Congratulations, you solved the puzzle!", [AppLanguage.Turkish] = "Tebrikler, bulmacayı çözdünüz!" },
        ["NewPuzzleQuestion"] = new() { [AppLanguage.English] = "Would you like to start a new puzzle?", [AppLanguage.Turkish] = "Yeni bir bulmaca başlatmak ister misiniz?" },
        ["Yes"] = new() { [AppLanguage.English] = "Yes, new puzzle", [AppLanguage.Turkish] = "Evet, yeni bulmaca" },
        ["No"] = new() { [AppLanguage.English] = "No, stay here", [AppLanguage.Turkish] = "Hayır, burada kal" },
        ["Score"] = new() { [AppLanguage.English] = "Score", [AppLanguage.Turkish] = "Puan" },
    };
}