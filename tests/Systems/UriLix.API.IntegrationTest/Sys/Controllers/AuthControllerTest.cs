using Microsoft.AspNetCore.Authentication.BearerToken;
using System.Net.Http.Json;
using UriLix.Application.DOTs;
using Xunit.Abstractions;

namespace UriLix.API.IntegrationTest.Sys.Controllers;

public class AuthControllerTest(
    IntegrationTestWebApplication<Program> factory, 
    ITestOutputHelper outputHelper) 
    : BaseWebApplicationTest(factory, outputHelper)
{
    [Theory]
    [InlineData("/api/Auth/login", "john@email.com", "Test123@")]
    public async Task POST_SignIn_Should_ReturnJWTAccessToken_When_CredentialsAreValid(
        string uri, string email, string password)
    {
        LoginRequest request = new(email, password);

        HttpResponseMessage response = await httpClient.PostAsJsonAsync(uri, request);
        response.EnsureSuccessStatusCode();

        AccessTokenResponse? token = await response.Content.ReadFromJsonAsync<AccessTokenResponse>();

        Assert.NotNull(token);
        outputHelper.WriteLine($"== RESPONSE MESSAGE==\n\t{token?.AccessToken}");
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
    }
}
