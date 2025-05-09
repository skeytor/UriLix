﻿using System.Security.Claims;
using UriLix.Shared.Results;

namespace UriLix.Application.Services.UrlShortening.Delete;

public interface IDeleteUrlService
{
    Task<Result<Guid>> ExecuteAsync(Guid id, ClaimsPrincipal user);
}
