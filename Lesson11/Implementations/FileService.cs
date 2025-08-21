using System.Text.Json;
using Lesson11.Data.Model;
using Lesson11.Interfaces;

namespace Lesson11.Implementations;

public class FileService : IFileService
{
    private readonly string _filePath;
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true
    };

    public List<Results> Results { get; set; } = new();

    public FileService()
    {
        var dataDir = Path.Combine(AppContext.BaseDirectory, "Data");
        Directory.CreateDirectory(dataDir);
        _filePath = Path.Combine(dataDir, "SearchResult.json");
    }
    
    private List<Results> Load()
    {
        if (!File.Exists(_filePath)) return new();
        var json = File.ReadAllText(_filePath);
        if (string.IsNullOrWhiteSpace(json)) return new();
        try { return JsonSerializer.Deserialize<List<Results>>(json, JsonOptions) ?? new(); }
        catch { return new(); } 
    }

    private void SaveAll(List<Results> list)
    {
        var json = JsonSerializer.Serialize(list, JsonOptions);
        File.WriteAllText(_filePath, json);
    }
    

    public void Save(Results results)
    {
        if (results is null || string.IsNullOrWhiteSpace(results.title)) return;

        var list = Load();
        list.Add(new Results
        {
            title = results.title.Trim(),
            release_date = results.release_date?.Trim()
        });

        Results = list;
        SaveAll(list);
    }

    public void DeleteMovie(string movieName)
    {
        if (string.IsNullOrWhiteSpace(movieName)) return;

        var list = Load();
        list.RemoveAll(r =>
            string.Equals(r.title?.Trim(), movieName.Trim(), StringComparison.OrdinalIgnoreCase));

        Results = list;
        SaveAll(list);

        Console.WriteLine("Movie deleted");
    }
}