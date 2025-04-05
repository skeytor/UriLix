using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UriLix.API.Extensions;
using UriLix.Application.DOTs;
using UriLix.Application.Services.UrlShortening;

namespace UriLix.API.Controllers;

[Route("api/[controller]")]
[Authorize]
public class LinksController(
    IUrlShorteningService shorteningService) : ApiBaseController
{
    [HttpPost]
    [AllowAnonymous]
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
    [AllowAnonymous]
    [ProducesResponseType<string>(StatusCodes.Status302Found)]
    [ProducesResponseType<NotFound>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<BadRequest<ValidationProblemDetails>>(StatusCodes.Status400BadRequest)]
    public async Task<Results<RedirectHttpResult, NotFound<ValidationProblemDetails>>> ResolveUrlAsync(
        string code)
    {
        var result = await shorteningService.GetOriginalUrlAsync(code, Request);
        return result.IsSuccess
            ? TypedResults.Redirect(result.Value)
            : TypedResults.NotFound(result.ToValidationProblemDetails());
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType<Guid>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<Results<Ok<Guid>, NotFound<ValidationProblemDetails>>> UpdateAsync(
        [FromRoute] Guid id, 
        [FromBody] UpdateShortenedUrlRequest request)
    {
        var result = await shorteningService.UpdateAsync(id, request);
        return result.IsSuccess
            ? TypedResults.Ok(result.Value)
            : TypedResults.NotFound(result.ToValidationProblemDetails());
    }
}
