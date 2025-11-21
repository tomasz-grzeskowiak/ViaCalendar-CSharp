using Entities;

namespace RepositoryContracts;

public interface IUserRepository
{
    Task<User> CreateAsync(User payload);
    Task UpdateAsync(User payload);
    Task DeleteAsync(int id);
    Task<User> GetSingleAsync(int id);
    
    IQueryable<User> GetManyAsync();
}