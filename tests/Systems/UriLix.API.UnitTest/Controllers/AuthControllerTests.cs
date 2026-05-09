using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using UriLix.API.Controllers;
using UriLix.Application.DOTs;
using UriLix.Application.Services.Auth;
using UriLix.Shared.Results;

namespace UriLix.API.UnitTest.Controllers;

public class AuthControllerTests
{
    [Theory]
    [InlineData("test@email.com", "Test123")]
    public async Task SignIn_Should_ReturnSuccess_When_CredentialsAreValid(string email, string password)
    {
        // Arrange
        Mock<IAuthService> mockAuthService = new();
        JwtAccessTokenResponse expectedResponse = new("token", ExpiresIn: 3600, "refresh_token");
        mockAuthService
            .Setup(service => service.SignIn(It.IsAny<LoginRequest>()))
            .ReturnsAsync(Result.Success(expectedResponse));
        AuthController sut = new(authService: mockAuthService.Object, default!, default!);
        LoginRequest request = new(email, password);

        // Act
        var result = await sut.Login(request);

        var okResult = result.Result as Ok<JwtAccessTokenResponse>;

        // Assert
        Assert.NotNull(okResult);
    }

    [Theory]
    [InlineData("invalid@email.com", "Test123")]
    public async Task SignIn_Should_ReturnFailure_When_CredentialsAreNotValid(string email, string password)
    {
        // Arrange
        Mock<IAuthService> mockAuthService = new();
        mockAuthService.Setup(service => service.SignIn(It.IsAny<LoginRequest>()))
            .ReturnsAsync(
            Result.Failure<JwtAccessTokenResponse>(
                Error.Validation("User.InvalidCredentials", "Invalid credentials")));
        AuthController sut = new(authService: mockAuthService.Object, default!, default!);
        LoginRequest request = new(email, password);

        // Act
        var result = await sut.Login(request);
        var badRequestResult = result.Result as BadRequest<ValidationProblem>;

        // Assert
        Assert.Null(badRequestResult);
    }
}
