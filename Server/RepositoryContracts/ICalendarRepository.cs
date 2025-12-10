using Entities;

namespace RepositoryContracts;

public interface ICalendarRepository
{
    Task<Calendar> CreateAsync(Calendar payload);
    Task UpdateAsync(Calendar payload);
    Task DeleteAsync(int id);
    Task<Calendar> GetSingleAsync(int id);
    
    IQueryable<Calendar> GetManyAsync();
}