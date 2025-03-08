using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace UriLix.Application.DOTs;

public sealed record GetOriginalUrlQueryParam(
    [Required]
    [MinLength(5), MaxLength(20)]
    [RegularExpression("^[a-zA-Z0-9-]*$", ErrorMessage = "The code can only contain letters, numbers and underscore")]
    string Code,

    [Required]
    FilterType Type);

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum FilterType
{
    Alias,
    ShortCode
}
