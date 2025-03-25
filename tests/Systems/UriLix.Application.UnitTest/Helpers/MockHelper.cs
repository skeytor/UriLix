using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using UriLix.Domain.Entities;

namespace UriLix.Application.UnitTest.Helpers;

internal static class MockHelper
{
    public static Mock<UserManager<TUser>> GetMockedUserManager<TUser>() where TUser : IdentityUser
    {
        var userStoreMock = new Mock<IUserStore<TUser>>();
        var optionsMock = new Mock<IOptions<IdentityOptions>>();
        var passwordHasherMock = new Mock<IPasswordHasher<TUser>>();
        var userValidatorsMock = new List<IUserValidator<TUser>> { new Mock<IUserValidator<TUser>>().Object };
        var passwordValidatorsMock = new List<IPasswordValidator<TUser>> { new Mock<IPasswordValidator<TUser>>().Object };
        var normalizerMock = new Mock<ILookupNormalizer>();
        var describerMock = new Mock<IdentityErrorDescriber>();
        var serviceProviderMock = new Mock<IServiceProvider>();
        var loggerMock = new Mock<ILogger<UserManager<TUser>>>();

        return new Mock<UserManager<TUser>>(
            userStoreMock.Object,
            optionsMock.Object,
            passwordHasherMock.Object,
            userValidatorsMock,
            passwordValidatorsMock,
            normalizerMock.Object,
            describerMock.Object,
            serviceProviderMock.Object,
            loggerMock.Object);
    }
}
