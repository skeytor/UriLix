﻿using UriLix.Application.DOTs;
using UriLix.Shared.Results;

namespace UriLix.Application.Services.UrlShortening;

public interface IUrlShorteningService
{
    public Task<Result<string>> ShortenUrlAsync(CreateShortenedUrlRequest request);
    public Task<Result<string>> GetOriginalUrlAsync(QueryFilter filter);   
    public Task<IReadOnlyList<GetShortenedUrlResponse>> GetAllURLsAsync(Guid userId);
}
