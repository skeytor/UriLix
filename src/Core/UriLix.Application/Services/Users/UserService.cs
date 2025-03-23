using UriLix.Application.DOTs;
using UriLix.Application.Extensions;
using UriLix.Domain.Entities;
using UriLix.Domain.Repositories;
using UriLix.Shared.Results;
using UriLix.Shared.UnitOfWork;

namespace UriLix.Application.Services.Users;

public class UserService(IUserRepository userRepository, IUnitOfWork unitOfWork) : IUserService
{
    public async Task<Result<UserProfileResponse>> GetProfileAsync(Guid userId)
    {
        ApplicationUser? user = await userRepository.FindByAsync(x => x.Id == userId.ToString());
        if (user is null)
        {
            return Result.Failure<UserProfileResponse>(Error.NotFound(
                "User.NotFound", 
                $"User with {userId} not found"));
        }
        return user.MapToResponse();
    }

    public async Task<Result<Guid>> RegisterAsync(CreateUserRequest request)
    {
        if (await userRepository.EmailExistsAsync(request.Email))
        {
            return Result.Failure<Guid>(Error.Validation("Email.Exists", $"Email {request.Email} already exists"));
        }
        ApplicationUser user = request.MapToEntity();
        await userRepository.InsertAsync(user);
        await unitOfWork.SaveChangesAsync();
        return Guid.Parse(user.Id);
    }
}
