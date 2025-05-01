using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using UriLix.Application.DOTs;
using UriLix.Domain.Entities;
using UriLix.Domain.Repositories;
using UriLix.Shared.Results;
using UriLix.Shared.UnitOfWork;

namespace UriLix.Application.Services.UrlShortening.Update;

public class UpdateUrlService(
    IShortenedUrlRepository shortenedUrlRepository,
    IAuthorizationService authorizationService,
    IUnitOfWork unitOfWork) : IUpdateUrlService
{
    public async Task<Result<Guid>> ExecuteAsync(Guid id, UpdateShortenUrlRequest request, ClaimsPrincipal user)
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
                "You are not authorized to edit this URL"));
        }

        shortenedUrl.OriginalUrl = request.OriginalUrl;
        await unitOfWork.SaveChangesAsync();
        return shortenedUrl.Id;
    }
}
