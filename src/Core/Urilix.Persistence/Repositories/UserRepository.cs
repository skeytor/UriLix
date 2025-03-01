using Microsoft.EntityFrameworkCore;
using UriLix.Domain.Entities;
using UriLix.Domain.Repositories;
using UriLix.Persistence.Abstractions;

namespace UriLix.Persistence.Repositories;

public class UserRepository(IAppDbContext _context) : BaseRepository(_context), IUserRepository
{
    public async Task<bool> EmailExistsAsync(string email)
    {
        return await context.Users.AnyAsync(u => u.Email == email);
    }

    public async Task<User> InsertAsync(User user)
    {
        await context.Users.AddAsync(user);
        return user;
    }
}
