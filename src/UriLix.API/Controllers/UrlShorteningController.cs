using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using UriLix.Application.DOTs;
using UriLix.Application.Services.UrlShortening;

namespace UriLix.API.Controllers;

[Route("api/[controller]")]
public class UrlShorteningController(IUrlShorteningService shorteningService) : ApiBaseController
{
    [HttpPost]
    [ProducesResponseType<string>(StatusCodes.Status201Created)]
    [ProducesResponseType<BadRequest<ValidationProblemDetails>>(StatusCodes.Status400BadRequest)]
    public async Task<Results<Created<string>, BadRequest>> ShortenUrl(
        [FromBody] CreateShortenedUrlRequest requestData)
    {
        var result = await shorteningService.ShortenUrlAsync(requestData);
        return result.IsSuccess
            ? TypedResults.Created(nameof(ShortenUrl), result.Value)
            : TypedResults.BadRequest();
    }

    [HttpGet("{shortCode}")]
    [ProducesResponseType<string>(StatusCodes.Status307TemporaryRedirect)]
    [ProducesResponseType<NotFound>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<BadRequest<ValidationProblemDetails>>(StatusCodes.Status404NotFound)]
    public async Task<Results<RedirectHttpResult, BadRequest<ValidationProblemDetails>, NotFound>> GetOriginalUrl(
        [FromRoute, Length(5, 5)] string shortCode)
    {
        var result = await shorteningService.GetOriginalUrlAsync(shortCode);
        return result.IsSuccess
            ? TypedResults.Redirect(result.Value, permanent: true)
            : TypedResults.NotFound();
    }
}
