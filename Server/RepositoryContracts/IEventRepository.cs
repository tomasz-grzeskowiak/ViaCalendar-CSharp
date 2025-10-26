using Entities;

namespace RepositoryContracts;

public interface IEventRepository
{
    Task<Event> CreateAsync(Event payload);
    Task UpdateAsync(Event payload);
    Task DeleteAsync(int id);
    Task<Event> GetSingleAsync(int id);
    
    IQueryable<Event> GetManyAsync();
}