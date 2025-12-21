using backend.Data;
using backend.Models;
using System.Text.Json;
using backend.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace backend.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{

    private readonly AppDbContext _db;

    public UserRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<UserAuth?> GetForAuthByUsernameAsync(string username)
    {
        return await _db.Users
            .Where(u => u.Username == username)
            .Select(u => new UserAuth
            {
                Id = u.Id,
                PasswordHash = u.PasswordHash
            })
            .FirstOrDefaultAsync();
    }

    public async Task<User?> GetAsync(Guid id)
    {
        return await _db.Users
        .AsNoTracking()
        .Where(u => u.Id == id)
        .Select(u => new User
        {
            Id = u.Id,
            Email = u.Email,
            Username = u.Username,
            PasswordHash = u.PasswordHash
        })
        .FirstOrDefaultAsync();

    }

    public async Task<IEnumerable<User>>GetAllAsync()
    {
        return await _db.Users
            .AsNoTracking()
            .Select(u => new User
            {
                Id = u.Id,
                Email = u.Email,
                Username = u.Username,
                PasswordHash = u.PasswordHash
            })
            .ToListAsync();
    }

    public async Task<User> AddAsync(Guid guid, string email, string name, string passwordHash)
    {
        var entity = new UserEntity
        {
            Id = guid,
            Email = email,
            Username = name,
            PasswordHash = passwordHash
        };

        _db.Users.Add(entity);
        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            throw new InvalidOperationException("User already exists");
        }


        return new User
        {
            Id = entity.Id,
            Email = entity.Email,
            Username = entity.Username,
            PasswordHash = entity.PasswordHash
        };
    }

  public async Task UpdateAsync(Guid id, string email, string name, string passwordHash)
    {
        var entity = await _db.Users.FindAsync(id);
        if (entity == null)
            throw new KeyNotFoundException($"User {id} not found");

        entity.Email = email;
        entity.Username = name;
        entity.PasswordHash = passwordHash;

        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _db.Users.FindAsync(id);
        if (entity == null)
            return;

        _db.Users.Remove(entity);
        await _db.SaveChangesAsync();
    }

    public async Task<bool> ExistsByUsernameAsync(string username)
    {
        return await _db.Users.AnyAsync(u => u.Username == username);
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await _db.Users.AnyAsync(u => u.Email == email);
    }

}