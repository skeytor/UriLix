using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UriLix.API.Extensions;
using UriLix.Application.DOTs;
using UriLix.Application.Services.UrlShortening;

namespace UriLix.API.Controllers;

[Route("api/[controller]")]
public class UrlShorteningController(IUrlShorteningService shorteningService) : ApiBaseController
{
    [HttpPost]
    [ProducesResponseType<string>(StatusCodes.Status201Created)]
    [ProducesResponseType<BadRequest<ValidationProblemDetails>>(StatusCodes.Status400BadRequest)]
    public async Task<Results<CreatedAtRoute<string>, BadRequest<ValidationProblemDetails>>> ShortenUrlAsync(
        [FromBody] CreateShortenedUrlRequest request)
    {
        var result = await shorteningService.ShortenUrlAsync(request, HttpContext.User);
        return result.IsSuccess
            ? TypedResults.CreatedAtRoute(
                result.Value, 
                nameof(ResolveUrlAsync), 
                new { code = result.Value })
            : TypedResults.BadRequest(result.ToValidationProblemDetails());
    }

    [HttpGet("{code}", Name = nameof(ResolveUrlAsync))]
    [ProducesResponseType<string>(StatusCodes.Status302Found)]
    [ProducesResponseType<NotFound>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<BadRequest<ValidationProblemDetails>>(StatusCodes.Status400BadRequest)]
    public async Task<Results<RedirectHttpResult, NotFound<ValidationProblemDetails>>> ResolveUrlAsync(
        string code)
    {
        var result = await shorteningService.GetOriginalUrlAsync(code);
        return result.IsSuccess
            ? TypedResults.Redirect(result.Value)
            : TypedResults.NotFound(result.ToValidationProblemDetails());
    }

    [HttpGet]
    [Authorize]
    [ProducesResponseType<List<ShortenedUrlResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<BadRequest>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<UnauthorizedHttpResult>(StatusCodes.Status401Unauthorized)]
    public async Task<Results<Ok<IReadOnlyList<ShortenedUrlResponse>>, BadRequest<ValidationProblemDetails>>> GetShortenedURLsAsync()
    {
        var result = await shorteningService.GetAllURLsAsync(HttpContext.User);
        return result.IsSuccess
            ? TypedResults.Ok(result.Value)
            : TypedResults.BadRequest(result.ToValidationProblemDetails());
    }
}
