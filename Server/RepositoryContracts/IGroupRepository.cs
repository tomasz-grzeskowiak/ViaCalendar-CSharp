using Entities;

namespace RepositoryContracts;

public interface IGroupRepository
{
    Task<Group> CreateAsync(Group payload);
    Task UpdateAsync(Group payload);
    Task DeleteAsync(int id);
    Task<Group> GetSingleAsync(int id);
    
    IQueryable<Group> GetManyAsync();
}