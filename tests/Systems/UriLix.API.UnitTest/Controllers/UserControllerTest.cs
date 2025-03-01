using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using UriLix.API.Controllers;
using UriLix.Application.DOTs;
using UriLix.Application.Services.Users;
using UriLix.Shared.Results;

namespace UriLix.API.UnitTest.Controllers;

public class UserControllerTest
{
    [Fact]
    public async Task CreateAsync_Should_Return201CreatedStatusCode_When_RegisterOperationIsSuccessfully()
    {
        Guid idExpected = Guid.NewGuid();
        Mock<IUserService> mockUserService = new();
        CreateUserRequest request = new("John Doe", "Smit", "email@example.com", "123Password");
        UserController sut = new(mockUserService.Object);

        mockUserService.Setup(service => service.RegisterAsync(It.IsAny<CreateUserRequest>()))
            .ReturnsAsync(idExpected);

        var result = await sut.CreateAsync(request);
        var createdResult = result.Result as Created<Guid>;

        Assert.NotNull(createdResult);
        Assert.Equal(StatusCodes.Status201Created, createdResult.StatusCode);
        Assert.Equal(idExpected, createdResult.Value);
    }

    [Fact]
    public async Task CreateAsync_Should_Return400BadRequest_When_RegisterOperationFails()
    {
        var errorFailure = Result.Failure<Guid>(Error.Conflict("UserEmail.Unique", ""));

        Mock<IUserService> mockUserService = new();
        CreateUserRequest request = new("John Doe", "Smit", "email@email.com", "Test13");
        UserController sut = new(mockUserService.Object);
        mockUserService.Setup(service => service.RegisterAsync(It.IsAny<CreateUserRequest>()))
            .ReturnsAsync(errorFailure);

        var result = await sut.CreateAsync(request);

        var badRequestResult = result.Result as BadRequest;
        Assert.NotNull(badRequestResult);
    }
}
