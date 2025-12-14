using backend.Infrastructure.Entities;
using backend.Models;

namespace backend.Infrastructure.Repositories;

public interface IUserRepository
{
    Task<UserAuth?> GetForAuthByUsernameAsync(string username);
    Task<User?> GetAsync(Guid id);
    Task<IEnumerable<User>> GetAllAsync();
    Task<User> AddAsync(Guid guid, string email, string name, string passwordHash);
    Task UpdateAsync(Guid id, string email, string name, string passwordHash);
    Task DeleteAsync(Guid id);
    Task<bool> ExistsByUsernameAsync(string username);
    Task<bool> ExistsByEmailAsync(string email);

}