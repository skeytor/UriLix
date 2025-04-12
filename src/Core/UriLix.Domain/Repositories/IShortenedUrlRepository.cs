using UriLix.Domain.Entities;
using UriLix.Shared.Pagination;

namespace UriLix.Domain.Repositories;

/// <summary>
/// Interface for the Shortened URL Repository.
/// </summary>
public interface IShortenedUrlRepository
{
    /// <summary>
    /// Inserts a new shortened URL into the database.
    /// </summary>
    /// <param name="shortenedUrl">A <see cref="ShortenedUrl"/> object</param>
    /// <returns> A task that represents an asynchronous operation. The task result contains a newly 
    /// <see cref="ShortenedUrl"/> object.
    /// </returns>
    Task<ShortenedUrl> InsertAsync(ShortenedUrl shortenedUrl);

    /// <summary>
    /// Finds a shortened URL by its ID.
    /// </summary>
    /// <param name="id">
    /// </param>
    /// <returns></returns>
    ValueTask<ShortenedUrl?> FindByIdAsync(Guid id);

    /// <summary>
    /// Finds a shortened URL by its short code.
    /// </summary>
    /// <param name="shortCode"></param>
    /// <returns></returns>
    Task<ShortenedUrl?> FindByShortCodeAsync(string shortCode);

    /// <summary>
    /// Retrieves all shortened URLs for a specific user, with pagination support.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="paginationQuery"></param>
    /// <returns></returns>
    Task<IReadOnlyList<ShortenedUrl>> GetAllByUserIdAsync(string userId, PaginationQuery paginationQuery);
    /// <summary>
    /// Counts the number of shortened URLs for a specific user.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<int> CountByUserIdAsync(string userId);
    /// <summary>
    /// Retrieves the original URL associated with a given short code.
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    Task<string?> GetOriginalUrlAsync(string code);
    /// <summary>
    /// Checks if a shortened URL with the given short code already exists in the database.
    /// </summary>
    /// <param name="shortCode"></param>
    /// <returns></returns>
    Task<bool> ShortUrlExistsAsync(string shortCode);
    /// <summary>
    /// Updates an existing shortened URL in the database.
    /// </summary>
    /// <param name="shortenedUrl"></param>
    void Update(ShortenedUrl shortenedUrl);
    /// <summary>
    /// Deletes a shortened URL from the database.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="shortenedUrl"></param>
    void Delete(Guid id, ShortenedUrl shortenedUrl);
}
