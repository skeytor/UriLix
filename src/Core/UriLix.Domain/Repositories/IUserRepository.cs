using UriLix.Domain.Entities;

namespace UriLix.Domain.Repositories;

/// <summary>
/// Interface for the User Repository.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Inserts a new user into the database.
    /// </summary>
    /// <param name="user">An user object</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains
    /// the newly created <see cref="ApplicationUser"/>.
    /// </returns>
    public Task<ApplicationUser> InsertAsync(ApplicationUser user);

    /// <summary>
    /// Finds a user by their ID.
    /// </summary>
    /// <param name="id">The user's unique identifier as a string</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains
    /// the found <see cref="ApplicationUser"/> or <c>null</c> if no user with the specified ID exists
    /// </returns>
    public Task<ApplicationUser?> FindByIdAsync(string id);
    /// <summary>
    /// Finds a user by their email
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    public Task<ApplicationUser?> FindByEmailAsync(string email);

    /// <summary>
    /// Checks if a user with the given email already exists in the database.
    /// </summary>
    /// <param name="email">The user's email as a string to check</param>
    /// <returns>
    /// A task that represents the asynchronous operation. 
    /// The task result contains <c>true</c> if the email exists, <c>false</c> otherwise.
    /// </returns>
    public Task<bool> EmailExistsAsync(string email);
}
