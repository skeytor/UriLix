using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace UriLix.Application.DOTs;

public sealed record OriginalUrlQueryParam(
    [Required]
    [MinLength(5), MaxLength(20)]
    [RegularExpression("^[a-zA-Z0-9-]*$", ErrorMessage = "The code can only contain letters, numbers and underscore")]
    string Code,

    [Required]
    UrlCodeType CodeType);

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum UrlCodeType
{
    Alias,
    ShortCode
}
