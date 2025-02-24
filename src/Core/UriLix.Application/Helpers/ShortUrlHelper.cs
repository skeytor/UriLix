namespace UriLix.Application.Helpers;

internal static class ShortUrlHelper
{
    private const int MAX_LENGTH = 5;
    private const string ALLOWED_CHARACTERS = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    internal static string GenerateShortUrl()
    {
        char[] chars = [.. Enumerable.Range(0, MAX_LENGTH).Select(_ => ALLOWED_CHARACTERS[Random.Shared.Next(ALLOWED_CHARACTERS.Length)])];
        return new string(chars);
    }
}
