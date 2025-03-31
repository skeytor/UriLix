using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;
using UriLix.Application.DOTs;
using UriLix.Persistence.Abstractions;
using Xunit.Abstractions;

namespace UriLix.API.IntegrationTest.Sys.Controllers;

public class UserControllerTest(
    IntegrationTestWebApplication<Program> _factory,
    ITestOutputHelper _outputHelper)
    : BaseWebApplicationTest(_factory, _outputHelper)
{
    [Theory]
    [InlineData("/api/User/register")]
    public async Task POST_CreateAsync_Should_Return201Created_When_UserIsValid(string requestUri)
    {
        CreateUserRequest request = new("test", "test", "email@mail.com", "Password123@");

        HttpResponseMessage response = await httpClient.PostAsJsonAsync(requestUri, request);

        response.EnsureSuccessStatusCode();
        string id = await response.Content.ReadAsStringAsync();

        outputHelper.WriteLine($"Response Body: {id}");
        outputHelper.WriteLine($"Location:\n\t{response.Headers.Location}");

        Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
        Assert.NotEqual(string.Empty, id);
    }

    [Theory]
    [InlineData("/api/User/register")]
    public async Task POST_CreateAsync_Should_Return400BadRequest_When_UserAlreadyExists(
        string requestUri)
    {
        CreateUserRequest request = new("Jhon Doe", "test", "john@email.com", "Test123@");

        HttpResponseMessage response = await httpClient.PostAsJsonAsync(requestUri, request);

        string message = await response.Content.ReadAsStringAsync();
        outputHelper.WriteLine($"ERROR MESSAGE:\n\t {message}");

        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GET_GetUserProfileAsync_Should_Return200OK_When_UserAlreadyExists()
    {
        using IServiceScope scope = factory.Services.CreateScope();
        IApplicationDbContext appDbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
        string id = (await appDbContext.Users.FirstAsync()).Id;

        HttpResponseMessage response = await httpClient.GetAsync($"/api/User/me");
        response.EnsureSuccessStatusCode();

        UserProfileResponse? userProfile = await response.Content.ReadFromJsonAsync<UserProfileResponse>();
        outputHelper.WriteLine($"Response Body: {userProfile}");
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(userProfile);
    }

    [Fact]
    public async Task GET_GetUserProfileAsync_Should_Return404NotFound_When_UserNoExists()
    {
        string id = Guid.NewGuid().ToString();

        HttpResponseMessage response = await httpClient.GetAsync($"/api/User/{id}");

        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
    }
}
