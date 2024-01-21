using Firebase.Auth;
using PostresUAMMApi.Enums;
using PostresUAMMApi.Repositories;

namespace PostresUAMMApi.Services;

public interface IUserService
{
    Task<Models.User> AddUserAsync(Models.User user);

    Task<Models.Customer> AddCustomerAsync(UserCredential userCredential);
}

public class UserService(
    IUserRepository userRepository,
    ICustomerRepository customerRepository) : IUserService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ICustomerRepository _customerRepository = customerRepository;

    public async Task<Models.User> AddUserAsync(Models.User user)
    {
        Models.User newUser = await _userRepository.AddUserAsync(user);

        return newUser;
    }

    public async Task<Models.Customer> AddCustomerAsync(UserCredential userCredential)
    {
        Models.User newUser = await AddUserAsync(new Models.User
        {
            FirebaseAuthUid = userCredential.User.Uid,
            Roles = [ RolesEnum.Customer ]
        });

        return await _customerRepository.AddCustomerAsync(new Models.Customer
        {
            UserId = newUser.Id,
            CustomerType = userCredential.User.Info.Email.EndsWith("@alumnos.uat.edu.mx")
                ? CustomerTypesEnum.Student
                : CustomerTypesEnum.Teacher,
            User = newUser
        });
    }
}
