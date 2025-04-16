using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UriLix.API.Extensions;
using UriLix.Application.DOTs;
using UriLix.Application.Services.UrlShortening.Delete;
using UriLix.Application.Services.UrlShortening.GetAll;
using UriLix.Application.Services.UrlShortening.GetOriginalUrl;
using UriLix.Application.Services.UrlShortening.Shortening;
using UriLix.Application.Services.UrlShortening.Update;
using UriLix.Shared.Pagination;

namespace UriLix.API.Controllers;

[Route("api/[controller]")]
[Authorize]
public class LinksController(
    IShortenUrlService shortenUrlService,
    IUrlRedirectionService urlRedirectionService,
    IUrlUpdateService urlUpdateService,
    IUrlQueryService urlQueryService,
    IUrlDeleteService urlDeleteService) : ApiBaseController
{
    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType<string>(StatusCodes.Status201Created)]
    [ProducesResponseType<BadRequest<ValidationProblemDetails>>(StatusCodes.Status400BadRequest)]
    public async Task<Results<CreatedAtRoute<string>, BadRequest<ValidationProblemDetails>>> ShortenUrlAsync(
        [FromBody] CreateShortenUrlRequest request)
    {
        var result = await shortenUrlService.ExecuteAsync(request, User);
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
        var result = await urlRedirectionService.ExecuteAsync(code, Request.Headers);
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
        var result = await urlQueryService.ExecuteAsync(paginationQuery, User);
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
        var result = await urlUpdateService.ExecuteAsync(id, request, User);
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
        var result = await urlDeleteService.ExecuteAsync(id, User);
        return result.IsSuccess
            ? TypedResults.Ok(result.Value)
            : TypedResults.NotFound(result.ToValidationProblemDetails());
    }
}
