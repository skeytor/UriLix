using System.Net.Http.Json;
using UriLix.Application.DOTs;
using Xunit.Abstractions;

namespace UriLix.API.IntegrationTest.Sys.Controllers;

public class UserControllerTest(
    IntegrationTestWebApplication<Program> _factory, 
    ITestOutputHelper _outputHelper) 
    : BaseWebApplicationTest(_factory, _outputHelper)
{
    [Fact]
    public async Task PostCreateAsync_Should_Return201Created_When_UserIsValid()
    {
        CreateUserRequest request = new("test", "test", "email@mail.com", "Password123");

        HttpResponseMessage response = await httpClient.PostAsJsonAsync("/api/User", request);

        response.EnsureSuccessStatusCode();
        Guid id = await response.Content.ReadFromJsonAsync<Guid>();

        Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
        Assert.NotEqual(Guid.Empty, id);
    }
    [Fact]
    public async Task PostCreateAsync_Should_Return400BadRequest_When_UserEmailAlreadyExists()
    {
        CreateUserRequest request = new("test", "test", "john@email.com", "Test123");

        HttpResponseMessage response = await httpClient.PostAsJsonAsync("/api/User", request);

        string message = await response.Content.ReadAsStringAsync();
        outputHelper.WriteLine($"ERROR MESSAGE:\n\t {message}");

        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }
}
