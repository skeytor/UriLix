using System.ComponentModel.DataAnnotations;

namespace UriLix.Application.DOTs;

public sealed record QueryFilter(
    [Required, Length(5, 20)]
    [RegularExpression("^[a-zA-Z0-9-]*$", ErrorMessage = "The code can only contain letters, numbers and underscore")]
    string Code,

    [Required]
    [EnumDataType(typeof(UrlTypeFilter))]
    UrlTypeFilter Type);

public enum UrlTypeFilter
{
    Alias,
    ShortCode
}
