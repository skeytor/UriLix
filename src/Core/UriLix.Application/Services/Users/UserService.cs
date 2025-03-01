using UriLix.Application.DOTs;
using UriLix.Application.Extensions;
using UriLix.Domain.Entities;
using UriLix.Domain.Repositories;
using UriLix.Shared.Results;
using UriLix.Shared.UnitOfWork;

namespace UriLix.Application.Services.Users;

public class UserService(IUserRepository userRepository, IUnitOfWork unitOfWork) : IUserService
{
    public async Task<Result<Guid>> RegisterAsync(CreateUserRequest request)
    {
        if (await userRepository.EmailExistsAsync(request.Email))
        {
            return Result.Failure<Guid>(Error.Validation("Email.Exists", $"Email {request.Email} already exists"));
        }
        User user = request.MapToEntity();
        await userRepository.InsertAsync(user);
        await unitOfWork.SaveChangesAsync();
        return user.Id;
    }
}
