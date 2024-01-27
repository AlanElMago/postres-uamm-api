using PostresUAMMApi.Models;
using PostresUAMMApi.Repositories;

namespace PostresUAMMApi.Services;

public interface IUserService
{
    // Task<Models.User> AddUserAsync(Models.User user);

    // Task<Models.Customer> AddCustomerAsync(UserCredential userCredential);
}

public class UserService(
    IUserRepository userRepository,
    ICustomerRepository customerRepository) : IUserService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ICustomerRepository _customerRepository = customerRepository;

    public async Task<User> AddUserAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<Customer> AddCustomerAsync()
    {
        throw new NotImplementedException();
    }
}
