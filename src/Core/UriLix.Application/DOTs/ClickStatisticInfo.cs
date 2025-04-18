namespace UriLix.Application.DOTs;

/// <summary>
/// Represents statistical information about a click event, including client and traffic source details.
/// </summary>
/// <param name="UserAgent">
/// The Uer-Agent of the HTTP request.
/// Example: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.3
/// </param>
/// <param name="Referer">
/// The Referer header of the HTTP request.
/// Example: https://www.example.com/source-page
/// </param>
public sealed record ClickStatisticInfo(
    string UserAgent,
    string Referer
    );
