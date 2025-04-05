using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using System.Security.Claims;
using UriLix.API.Controllers;
using UriLix.Application.DOTs;
using UriLix.Application.Services.Users;
using UriLix.Shared.Results;
using Xunit.Abstractions;

namespace UriLix.API.UnitTest.Controllers;

public class UserControllerTest(ITestOutputHelper output)
{
    [Fact]
    public async Task CreateAsync_Should_Return200Ok_When_RegisterOperationIsSuccessfully()
    {
        string idExpected = Guid.NewGuid().ToString();
        Mock<IUserService> mockUserService = new();
        CreateUserRequest request = new("John Doe", "Smit", "email@example.com", "123Password");
        UsersController sut = new(mockUserService.Object);

        mockUserService.Setup(service => service.CreateAsync(It.IsAny<CreateUserRequest>()))
            .ReturnsAsync(idExpected);

        var result = await sut.Register(request);
        var createdResult = result.Result as Created<string>;

        Assert.NotNull(createdResult);
        Assert.Equal(StatusCodes.Status201Created, createdResult.StatusCode);
        Assert.Equal(idExpected, createdResult.Value);
    }

    [Fact]
    public async Task CreateAsync_Should_Return400BadRequest_When_RegisterOperationFails()
    {
        var errorFailure = Result.Failure<string>(Error.Conflict("UserEmail.Unique", ""));

        Mock<IUserService> mockUserService = new();
        CreateUserRequest request = new("John Doe", "Smit", "email@email.com", "Test13");
        UsersController sut = new(mockUserService.Object);
        mockUserService.Setup(service => service.CreateAsync(It.IsAny<CreateUserRequest>()))
            .ReturnsAsync(errorFailure);

        var result = await sut.Register(request);

        var badRequestResult = result.Result as BadRequest;
        Assert.NotNull(badRequestResult);
    }

    [Fact]
    public async Task GetUserProfile_Should_ReturnUserProfile_When_UserAlreadyExists()
    {
        Mock<IUserService> mockUserService = new();
        UserProfileResponse userProfile = new("John Doe Smit", "email@example.com");
        DefaultHttpContext defaultHttpContext = new()
        {
            User = new ClaimsPrincipal(new ClaimsIdentity(
            [
                new(ClaimTypes.NameIdentifier, "test-user-id")
            ]))
        };
        UsersController sut = new(mockUserService.Object)
        {
            ControllerContext = new()
            {
                HttpContext = defaultHttpContext,
            },
        };
        mockUserService.Setup(service => service.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
            .ReturnsAsync(userProfile);

        var result = await sut.GetUserInfo();

        var okResult = result.Result as Ok<UserProfileResponse>;
        Assert.NotNull(okResult);
        Assert.Equal(userProfile.Email, okResult?.Value?.Email);
    }
}
