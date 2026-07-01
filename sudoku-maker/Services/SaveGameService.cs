using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using sudoku_maker.Models;

namespace sudoku_maker.Services;

public class SaveGameService
{
    private readonly string _saveFilePath;
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        WriteIndented = true
    };

    public SaveGameService()
    {
        var appDataPath =Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var folderPath = Path.Combine(appDataPath, "sudoku-maker");

        Directory.CreateDirectory(folderPath);
        _saveFilePath = Path.Combine(folderPath, "savegame.json");
    }

    public List<SaveGame> GetAll()
    {
        if(!File.Exists(_saveFilePath))
        {
            return new List<SaveGame>();
        }

        var json = File.ReadAllText(_saveFilePath);

        if(string.IsNullOrWhiteSpace(json))
        {
            return new List<SaveGame>();
        }

        try
        {
            return JsonSerializer.Deserialize<List<SaveGame>>(json, SerializerOptions) ?? new List<SaveGame>();
        }
        catch (JsonException)
        {
            return new List<SaveGame>();
        }
        catch (NotSupportedException)
        {
            return new List<SaveGame>();
        }
    }
    
    public void SaveAll(List<SaveGame> saves)
    {
        var json = JsonSerializer.Serialize(saves, SerializerOptions);
        File.WriteAllText(_saveFilePath, json);
    }

    public SaveGame CreateOrUpdate(SaveGame saveGame)
    {
        var saves = GetAll();

        var existingSave = saves.FirstOrDefault(s => s.Id == saveGame.Id);

        saveGame.UpdatedAt = DateTime.Now;

        if(existingSave == null)
        {
            saveGame.CreatedAt = DateTime.Now;
            saves.Add(saveGame);
        }
        else
        {
            saveGame.CreatedAt = existingSave.CreatedAt;
            
            var index = saves.IndexOf(existingSave);
            saves[index] = saveGame;
        }

        SaveAll(saves);

        return saveGame;
    }

    public SaveGame? GetById(string id)
    {
        var saves = GetAll();
        
        return saves.FirstOrDefault(s => s.Id == id);
    }

    public void Delete(string id)
    {
        var saves = GetAll();
        var saveToDelete = saves.FirstOrDefault(s => s.Id == id);

        if(saveToDelete != null)
        {
            saves.Remove(saveToDelete);
            SaveAll(saves);
        }
    }
}
