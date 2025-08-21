using System.Text.Json;
using ToDo.Application.Contracts.Repositories;

namespace ToDo.Infrastructure.Persistence.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{

    private readonly string _filePath;


    private readonly List<T> _values;

    public BaseRepository()
    {

        if (Directory.Exists(_filePath))
        {
            var json = File.ReadAllText(_filePath);
            _values = JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
        }
        else
        {
            _values = new List<T>();
            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;

            _filePath = Path.Combine(appDirectory, "tasks.json");
        }
    }
    public void AddTask(T value) => _values.Add(value);
    public void DeleteTask(T value) => _values.Remove(value);

    public void Save()
    {
        var json = JsonSerializer.Serialize(_values, new JsonSerializerOptions { WriteIndented = true });

        File.WriteAllText(_filePath, json);
    }

    public void UpdateTask(T value)
    {
        string updatedJson = JsonSerializer.Serialize(value, new JsonSerializerOptions { WriteIndented = true });

        File.WriteAllText(_filePath, updatedJson);
    }

    public IQueryable<T> ViewAllValues() => _values.AsQueryable();
}
