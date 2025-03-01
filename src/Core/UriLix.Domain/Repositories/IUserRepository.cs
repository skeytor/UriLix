using UriLix.Domain.Entities;

namespace UriLix.Domain.Repositories;

public interface IUserRepository
{
    public Task<User> InsertAsync(User user);
    public Task<bool> EmailExistsAsync(string email);
}
