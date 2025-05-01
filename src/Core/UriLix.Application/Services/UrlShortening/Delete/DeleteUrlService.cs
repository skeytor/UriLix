using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using UriLix.Domain.Entities;
using UriLix.Domain.Repositories;
using UriLix.Shared.Results;
using UriLix.Shared.UnitOfWork;

namespace UriLix.Application.Services.UrlShortening.Delete;

public class DeleteUrlService(
    IShortenedUrlRepository shortenedUrlRepository,
    IAuthorizationService authorizationService,
    IUnitOfWork unitOfWork) : IDeleteUrlService
{
    public async Task<Result<Guid>> ExecuteAsync(Guid id, ClaimsPrincipal user)
    {
        ShortenedUrl? shortenedUrl = await shortenedUrlRepository.FindByIdAsync(id);
        if (shortenedUrl is null)
        {
            return Result.Failure<Guid>(Error.NotFound(
            "Url.NotFound",
                $"The URL with id: {id} was not found"));
        }
        AuthorizationResult authResult = await authorizationService.AuthorizeAsync(user, shortenedUrl, "EditPolicy");
        if (!authResult.Succeeded)
        {
            return Result.Failure<Guid>(Error.Failure(
                "Url.Forbidden",
                "You are not authorized to delete this URL"));
        }
        shortenedUrlRepository.Delete(id, shortenedUrl);
        await unitOfWork.SaveChangesAsync();
        return id;
    }
}
