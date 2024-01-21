using Firebase.Auth;
using PostresUAMMApi.Enums;
using PostresUAMMApi.Models;
using PostresUAMMApi.Models.Forms;
using PostresUAMMApi.Repositories;

namespace PostresUAMMApi.Services;

public interface ICustomerService
{
    Task<Customer> RegisterCustomerAsync(CustomerRegistrationForm customer);
}

public class CustomerService(
    ICustomerRepository customerRepository,
    IFirebaseAuthService firebaseAuthService,
    IUserService userService ) : ICustomerService
{
    private readonly ICustomerRepository _customerRepository = customerRepository;
    private readonly IFirebaseAuthService _firebaseAuthService = firebaseAuthService;
    private readonly IUserService _userService = userService;

    public async Task<Customer> RegisterCustomerAsync(CustomerRegistrationForm form)
    {
        ArgumentNullException.ThrowIfNull(form);
        ArgumentNullException.ThrowIfNull(form.Email);
        ArgumentNullException.ThrowIfNull(form.Password);

        if (!form.Email.EndsWith("@alumnos.uat.edu.mx") && !form.Email.EndsWith("@docentes.uat.edu.mx"))
        {
            throw new ArgumentException("El correo electrónico debe ser de la UAT");
        }

        UserCredential userCredential = await _firebaseAuthService.SignUp(form.Email, form.Password);

        Models.User newUser = await _userService.RegisterUserAsync(new Models.User
        {
            FirebaseAuthUid = userCredential.User.Uid,
            FullName = form.FullName,
            Roles = [ RolesEnum.Customer ]
        });

        Customer newCustomer = await _customerRepository.AddCustomerAsync(new Customer
        {
            UserId = newUser.Id,
            CustomerType = form.Email.EndsWith("@alumnos.uat.edu.mx")
                ? CustomerTypesEnum.Student
                : CustomerTypesEnum.Teacher
        });

        return newCustomer;
    }
}
