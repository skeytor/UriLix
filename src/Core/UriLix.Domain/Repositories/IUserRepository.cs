using System.Linq.Expressions;
using UriLix.Domain.Entities;

namespace UriLix.Domain.Repositories;

public interface IUserRepository
{
    public Task<User> InsertAsync(User user);
    public Task<User?> FindByAsync(Expression<Func<User, bool>> predicate);
    public Task<bool> EmailExistsAsync(string email);
}
