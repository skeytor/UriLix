using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UriLix.API.Extensions;
using UriLix.Application.DOTs;
using UriLix.Application.Services.UrlShortening;
using UriLix.Shared.Pagination;

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
        [FromBody] CreateShortenUrlRequest request)
    {
        var result = await shorteningService.ShortenUrlAsync(request, User);
        return result.IsSuccess
            ? TypedResults.CreatedAtRoute(
                result.Value,
                nameof(ResolveUrlAsync),
                new { code = result.Value })
            : TypedResults.BadRequest(result.ToValidationProblemDetails());
    }

    [HttpGet("{code:maxlength(20)}", Name = nameof(ResolveUrlAsync))]
    [AllowAnonymous]
    [ProducesResponseType<string>(StatusCodes.Status302Found)]
    [ProducesResponseType<NotFound>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<BadRequest<ValidationProblemDetails>>(StatusCodes.Status400BadRequest)]
    public async Task<Results<RedirectHttpResult, NotFound<ValidationProblemDetails>>> ResolveUrlAsync(
        [FromRoute] string code)
    {
        var result = await shorteningService.GetOriginalUrlAsync(code, Request);
        return result.IsSuccess
            ? TypedResults.Redirect(result.Value)
            : TypedResults.NotFound(result.ToValidationProblemDetails());
    }

    [HttpGet]
    [Authorize]
    [ProducesResponseType<PagedResult<ShortenedUrlResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<NotFound<ValidationProblemDetails>>(StatusCodes.Status404NotFound)]
    public async Task<Results<Ok<PagedResult<ShortenedUrlResponse>>, NotFound<ValidationProblemDetails>>> GetAllAsync(
        [FromQuery] PaginationQuery paginationQuery)
    {
        var result = await shorteningService.GetAllPagedAsync(User, paginationQuery);
        return result.IsSuccess
            ? TypedResults.Ok(result.Value)
            : TypedResults.NotFound(result.ToValidationProblemDetails());
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType<Guid>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<Results<Ok<Guid>, NotFound<ValidationProblemDetails>>> UpdateAsync(
        [FromRoute] Guid id,
        [FromBody] UpdateShortenUrlRequest request)
    {
        var result = await shorteningService.UpdateAsync(id, request, User);
        return result.IsSuccess
            ? TypedResults.Ok(result.Value)
            : TypedResults.NotFound(result.ToValidationProblemDetails());
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType<Guid>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<Results<Ok<Guid>, NotFound<ValidationProblemDetails>>> DeleteAsync(
        [FromRoute] Guid id)
    {
        var result = await shorteningService.DeleteAsync(id, User);
        return result.IsSuccess
            ? TypedResults.Ok(result.Value)
            : TypedResults.NotFound(result.ToValidationProblemDetails());
    }
}
