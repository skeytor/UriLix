using Moq;
using UriLix.Application.DOTs;
using UriLix.Application.Services.Users;
using UriLix.Domain.Entities;
using UriLix.Domain.Repositories;
using UriLix.Shared.Enums;
using UriLix.Shared.UnitOfWork;

namespace UriLix.Application.UnitTest.Services;

public class UserServiceTest
{
    [Fact]
    public async Task RegisterAsync_Should_ReturnId_When_EmailNoExist()
    {
        Mock<IUserRepository> mockRepository = new();
        Mock<IUnitOfWork> mockUnitOfWork = new();
        CreateUserRequest request = new("name", "last name", "email", "password");
        UserService sut = new(mockRepository.Object, mockUnitOfWork.Object);

        mockRepository.Setup(repo => repo.EmailExistsAsync(It.IsAny<string>()))
            .ReturnsAsync(false);

        var result = await sut.RegisterAsync(request);

        Assert.True(result.IsSuccess);
        Assert.IsType<Guid>(result.Value);

        mockRepository.Verify(x => x.InsertAsync(It.IsAny<User>()), Times.Once);
        mockUnitOfWork.Verify(x => x.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task RegisterAsync_Should_ReturnFailure_When_EmailExists()
    {
        Mock<IUserRepository> mockRepository = new();
        Mock<IUnitOfWork> mockUnitOfWork = new();
        CreateUserRequest request = new("name", "last name", "email", "password");
        UserService sut = new(mockRepository.Object, mockUnitOfWork.Object);

        mockRepository.Setup(repo => repo.EmailExistsAsync(It.IsAny<string>()))
            .ReturnsAsync(true);

        var result = await sut.RegisterAsync(request);

        Assert.True(result.IsFailure);
        Assert.Equal(ErrorType.Validation, result.Error.Type);
        Assert.Equal("Email.Exists", result.Error.Code);
    }
}
