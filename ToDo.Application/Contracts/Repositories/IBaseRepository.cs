namespace ToDo.Application.Contracts.Repositories;

public interface IBaseRepository<T> where T : class
{
    IQueryable<T> ViewAllValues();
    void AddTask(T value);
    void DeleteTask(T value);
    void UpdateTask(T value);
    void Save();
}
