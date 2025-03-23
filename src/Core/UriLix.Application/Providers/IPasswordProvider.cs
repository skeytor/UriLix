namespace UriLix.Application.Providers;

public interface IPasswordProvider
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string hash);
}
