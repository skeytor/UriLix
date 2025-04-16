using Microsoft.AspNetCore.Http;
using UriLix.Shared.Results;

namespace UriLix.Application.Services.UrlShortening.GetOriginalUrl;

public interface IUrlRedirectionService
{
    Task<Result<string>> ExecuteAsync(string code, IHeaderDictionary headersInfo); 
}
