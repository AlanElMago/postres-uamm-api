using PostresUAMMApi.Models;
using PostresUAMMApi.Repositories;

namespace PostresUAMMApi.Services;

public interface IUserService
{
    Task<User> RegisterUserAsync(User user);
}

public class UserService(IUserRepository userRepository) : IUserService
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<User> RegisterUserAsync(User user)
    {
        ArgumentNullException.ThrowIfNull(user);

        User newUser = await _userRepository.AddUserAsync(user);

        return newUser;
    }
}
