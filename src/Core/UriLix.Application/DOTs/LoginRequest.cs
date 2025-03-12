using System.ComponentModel.DataAnnotations;

namespace UriLix.Application.DOTs;

public sealed record LoginRequest(string UserName, [DataType(DataType.Password)] string Password);
