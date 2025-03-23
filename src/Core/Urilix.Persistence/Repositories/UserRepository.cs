using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UriLix.Domain.Entities;
using UriLix.Domain.Repositories;
using UriLix.Persistence.Abstractions;

namespace UriLix.Persistence.Repositories;

public class UserRepository(IApplicationDbContext context) : BaseRepository(context), IUserRepository
{
    public Task<bool> EmailExistsAsync(string email) 
        => Context.Users.AnyAsync(u => u.Email == email);

    public Task<ApplicationUser?> FindByAsync(Expression<Func<ApplicationUser, bool>> predicate) 
        => Context.Users.FirstOrDefaultAsync(predicate);

    public async Task<ApplicationUser> InsertAsync(ApplicationUser user)
    {
        await Context.Users.AddAsync(user);
        return user;
    }
}
