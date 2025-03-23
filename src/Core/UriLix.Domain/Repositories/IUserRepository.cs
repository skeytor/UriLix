using System.Linq.Expressions;
using UriLix.Domain.Entities;

namespace UriLix.Domain.Repositories;

public interface IUserRepository
{
    public Task<ApplicationUser> InsertAsync(ApplicationUser user);
    public Task<ApplicationUser?> FindByAsync(Expression<Func<ApplicationUser, bool>> predicate);
    public Task<bool> EmailExistsAsync(string email);
}
