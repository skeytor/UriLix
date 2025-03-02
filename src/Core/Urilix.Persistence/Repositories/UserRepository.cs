using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UriLix.Domain.Entities;
using UriLix.Domain.Repositories;
using UriLix.Persistence.Abstractions;

namespace UriLix.Persistence.Repositories;

public class UserRepository(IAppDbContext _context) : BaseRepository(_context), IUserRepository
{
    public Task<bool> EmailExistsAsync(string email) => context.Users.AnyAsync(u => u.Email == email);

    public Task<User?> FindByAsync(Expression<Func<User, bool>> filter) 
        => context.Users.FirstOrDefaultAsync(filter);

    public async Task<User> InsertAsync(User user)
    {
        await context.Users.AddAsync(user);
        return user;
    }
}
