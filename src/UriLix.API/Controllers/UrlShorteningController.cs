using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UriLix.Application.DOTs;
using UriLix.Application.Services.UrlShortening;

namespace UriLix.API.Controllers;

[Route("api/[controller]")]
public class UrlShorteningController(IUrlShorteningService shorteningService) : ApiBaseController
{
    [HttpPost]
    [ProducesResponseType<string>(StatusCodes.Status201Created)]
    [ProducesResponseType<BadRequest<ValidationProblemDetails>>(StatusCodes.Status400BadRequest)]
    public async Task<Results<CreatedAtRoute<string>, BadRequest>> ShortenUrl(
        [FromBody] CreateShortenedUrlRequest request)
    {
        var result = await shorteningService.ShortenUrlAsync(request);
        return result.IsSuccess
            ? TypedResults.CreatedAtRoute(
                result.Value.Code, 
                nameof(ResolveUrl), 
                new { result.Value.Code, Type = (int)result.Value.Type })
            : TypedResults.BadRequest();
    }

    [HttpGet(Name = nameof(ResolveUrl))]
    [ProducesResponseType<string>(StatusCodes.Status307TemporaryRedirect)]
    [ProducesResponseType<NotFound>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<BadRequest<ValidationProblemDetails>>(StatusCodes.Status400BadRequest)]
    public async Task<Results<RedirectHttpResult, NotFound, BadRequest<ValidationProblemDetails>>> ResolveUrl(
        [FromQuery] GetOriginalUrlQueryParam queryParams)
    {
        var result = await shorteningService.GetOriginalUrlAsync(queryParams);
        return result.IsSuccess
            ? TypedResults.Redirect(result.Value, permanent: true)
            : TypedResults.NotFound();
    }
}
