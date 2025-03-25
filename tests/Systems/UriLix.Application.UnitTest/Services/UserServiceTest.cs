using Microsoft.AspNetCore.Identity;
using Moq;
using System.Security.Claims;
using UriLix.Application.DOTs;
using UriLix.Application.Services.Users;
using UriLix.Application.UnitTest.Helpers;
using UriLix.Domain.Entities;
using UriLix.Shared.Enums;

namespace UriLix.Application.UnitTest.Services;

public class UserServiceTest
{
    [Fact]
    public async Task CreateAsync_Should_ReturnId_When_EmailNoExist()
    {
        var mockUserManager = MockHelper
            .GetMockedUserManager<ApplicationUser>();
        CreateUserRequest request = new("name", "last name", "email", "password");
        UserService sut = new(mockUserManager.Object);

        mockUserManager.Setup(manager => manager.CreateAsync(
            It.IsAny<ApplicationUser>(), 
            It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);

        var result = await sut.CreateAsync(request);

        Assert.True(result.IsSuccess);
        Assert.IsType<string>(result.Value);
    }

    [Fact]
    public async Task CreateAsync_Should_ReturnFailure_When_EmailExists()
    {
        var mockUserManager = MockHelper.GetMockedUserManager<ApplicationUser>(); ;
        CreateUserRequest request = new("name", "last name", "email", "password");
        UserService sut = new(mockUserManager.Object);
        mockUserManager.Setup(manager => manager.CreateAsync(
            It.IsAny<ApplicationUser>(),
            It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Code = "", Description = ""}));

        var result = await sut.CreateAsync(request);

        Assert.True(result.IsFailure);
        Assert.Equal(ErrorType.Validation, result.Error.Type);
        Assert.Equal("User.CreateFailed", result.Error.Code);
    }

    [Fact]
    public async Task GetProfileAsync_Should_ReturnUserData_When_UserExists()
    {
        string id = Guid.NewGuid().ToString();
        var mockUserManager = MockHelper.GetMockedUserManager<ApplicationUser>();
        ApplicationUser user = new()
        {
            Id = id,
            FirstName = "Test",
            LastName = "Test1",
            Email = "test@email.com"
        };
        ClaimsPrincipal claimsPrincipal = new(new ClaimsIdentity(
        [
            new(ClaimTypes.NameIdentifier, id)
        ]));
        mockUserManager.Setup(manager => manager.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
            .ReturnsAsync(user);
        UserService sut = new(mockUserManager.Object);

        var result = await sut.GetUserAsync(claimsPrincipal);

        Assert.True(result.IsSuccess);
        Assert.Equal(result.Value.FullName, $"{user.FirstName} {user.LastName}");
    }

    [Fact]
    public async Task GetProfileAsync_Should_ReturnFailure_When_UserNoExists()
    {
        var mockUserManager = MockHelper.GetMockedUserManager<ApplicationUser>();
        UserService sut = new(mockUserManager.Object);
        string id = Guid.NewGuid().ToString();
        ClaimsPrincipal claimsPrincipal = new(new ClaimsIdentity(
        [
            new(ClaimTypes.NameIdentifier, id)
        ]));
        mockUserManager.Setup(manager => manager.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
            .ReturnsAsync((ApplicationUser?)null);

        var result = await sut.GetUserAsync(claimsPrincipal);

        Assert.True(result.IsFailure);
        Assert.Equal(ErrorType.NotFound, result.Error.Type);
        Assert.Equal("User.NotFound", result.Error.Code);
    }
}
