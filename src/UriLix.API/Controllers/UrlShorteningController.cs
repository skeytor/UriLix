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
    public async Task<Results<CreatedAtRoute<string>, BadRequest>> ShortenUrl(
        [FromBody] CreateShortenedUrlRequest requestData)
    {
        var result = await shorteningService.ShortenUrlAsync(requestData);
        return result.IsSuccess
            ? TypedResults.CreatedAtRoute(result.Value, nameof(GetOriginalUrl), new { shortCode = result.Value })
            : TypedResults.BadRequest();
    }

    //[HttpGet("{shortCode:length(5):regex(^[[A-Za-z0-9]]{{5}}$)}", Name = nameof(GetOriginalUrl))]
    [HttpGet]
    [ProducesResponseType<string>(StatusCodes.Status307TemporaryRedirect)]
    [ProducesResponseType<NotFound>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<BadRequest<ValidationProblemDetails>>(StatusCodes.Status400BadRequest)]
    public async Task<Results<RedirectHttpResult, NotFound, BadRequest<ValidationProblemDetails>>> GetOriginalUrl(
        [FromQuery] QueryFilter filter)
    {
        var result = await shorteningService.GetOriginalUrlAsync(filter);
        return result.IsSuccess
            ? TypedResults.Redirect(result.Value, permanent: true)
            : TypedResults.NotFound();
    }
}
