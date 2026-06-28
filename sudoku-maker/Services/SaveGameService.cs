using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using sudoku_maker.Models;

namespace sudoku_maker.Services;

public class SaveGameService
{
    private readonly string _saveFilePath;
    private static readonly JsonSerializerOptions SerializerOptions = CreateSerializerOptions();

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

    private static JsonSerializerOptions CreateSerializerOptions()
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        options.Converters.Add(new Int2DArrayJsonConverter());
        return options;
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

internal sealed class Int2DArrayJsonConverter : JsonConverter<int[,]>
{
    public override int[,] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return new int[0, 0];
        }

        using var document = JsonDocument.ParseValue(ref reader);

        if (document.RootElement.ValueKind != JsonValueKind.Array)
        {
            throw new JsonException("Expected an array for int[,] value.");
        }

        var rows = document.RootElement.EnumerateArray().ToArray();
        if (rows.Length == 0)
        {
            return new int[0, 0];
        }

        if (rows[0].ValueKind != JsonValueKind.Array)
        {
            throw new JsonException("Expected nested arrays for int[,] value.");
        }

        int columnCount = rows[0].GetArrayLength();
        var result = new int[rows.Length, columnCount];

        for (int rowIndex = 0; rowIndex < rows.Length; rowIndex++)
        {
            var row = rows[rowIndex];
            if (row.ValueKind != JsonValueKind.Array)
            {
                throw new JsonException("Expected nested arrays for int[,] value.");
            }

            if (row.GetArrayLength() != columnCount)
            {
                throw new JsonException("All rows must have the same number of columns for int[,] value.");
            }

            int columnIndex = 0;
            foreach (var value in row.EnumerateArray())
            {
                result[rowIndex, columnIndex] = value.GetInt32();
                columnIndex++;
            }
        }

        return result;
    }

    public override void Write(Utf8JsonWriter writer, int[,] value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();

        int rowCount = value.GetLength(0);
        int columnCount = value.GetLength(1);

        for (int row = 0; row < rowCount; row++)
        {
            writer.WriteStartArray();
            for (int column = 0; column < columnCount; column++)
            {
                writer.WriteNumberValue(value[row, column]);
            }
            writer.WriteEndArray();
        }

        writer.WriteEndArray();
    }
}