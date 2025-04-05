using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using UriLix.Application.DOTs;
using UriLix.Shared.Results;

namespace UriLix.Application.Services.UrlShortening;

public interface IUrlShorteningService
{
    public Task<Result<string>> ShortenUrlAsync(
        CreateShortenedUrlRequest request, ClaimsPrincipal? principal = null);
    public Task<Result<string>> GetOriginalUrlAsync(string Code, HttpRequest request);   
    public Task<Result<Guid>> UpdateAsync(Guid id, UpdateShortenedUrlRequest request);
}
