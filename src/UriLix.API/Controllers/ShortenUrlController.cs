using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using UriLix.Application.DOTs;
using UriLix.Application.Services.UrlShortening;

namespace UriLix.API.Controllers;

[Route("api/[controller]")]
public class ShortenUrlController(IUrlShorteningService shorteningService) : ApiBaseController
{
    [HttpPut]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ShortenUrl([FromBody] CreateShortenedUrlRequest requestData)
    {
        var result = await shorteningService.ShortenUrlAsync(requestData);
        return result.IsSuccess
            ? Ok(result.Value)
            : BadRequest();
    }

    [HttpGet("{shortCode}")]
    [ProducesResponseType<string>(StatusCodes.Status308PermanentRedirect)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<Results<RedirectHttpResult, NotFound>> GetOriginalUrl(
        [FromRoute, Length(5, 5)] string shortCode)
    {
        var result = await shorteningService.GetOriginalUrlAsync(shortCode);
        return result.IsSuccess
            ? TypedResults.Redirect(result.Value)
            : TypedResults.NotFound();
    }
}
