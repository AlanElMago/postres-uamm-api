using Firebase.Auth;
using Microsoft.AspNetCore.Mvc;
using PostresUAMMApi.Enums;
using PostresUAMMApi.Models;
using PostresUAMMApi.Models.Forms;
using PostresUAMMApi.Services;
using System.Net;

namespace PostresUAMMApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerController(
    IFirebaseAuthService firebaseAuthService,
    IUserService userService,
    ICustomerService customerService) : ControllerBase
{
    private readonly IFirebaseAuthService _firebaseAuthService = firebaseAuthService;
    private readonly IUserService _userService = userService;
    private readonly ICustomerService _customerService = customerService;

    [HttpPost]
    public async Task<HttpResponseMessage> RegisterCustomer(CustomerRegistrationForm form)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(form);
            ArgumentNullException.ThrowIfNull(form.Email);
            ArgumentNullException.ThrowIfNull(form.Password);

            if (!form.Email.EndsWith("@alumnos.uat.edu.mx") && !form.Email.EndsWith("@docentes.uat.edu.mx"))
            {
                return new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent("El correo electrónico debe ser de la UAT")
                };
            }

            UserCredential userCredential = await _firebaseAuthService.SignUp(form.Email, form.Password);

            Models.User newUser = await _userService.RegisterUserAsync(new Models.User
            {
                FirebaseAuthUid = userCredential.User.Uid,
                FullName = form.FullName,
                Roles = [ RolesEnum.Customer ]
            });

            await _customerService.RegisterCustomerAsync(new Customer
            {
                UserId = newUser.Id,
                CustomerType = form.Email.EndsWith("@alumnos.uat.edu.mx")
                    ? CustomerTypesEnum.Student
                    : CustomerTypesEnum.Teacher,
            });

        }
        catch (ArgumentNullException)
        {
            return new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.BadRequest,
                Content = new StringContent("Los datos no pueden ser nulos")
            };
        }
        catch (FirebaseAuthException)
        {
            return new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.BadRequest,
                Content = new StringContent("El correo electónico ya ha sido registrado")
            };
        }
        catch (Exception)
        {
            return new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Content = new StringContent("Ha ocurrido un error al registrar el usuario")
            };
        }

        return new HttpResponseMessage()
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent("El usuario se ha registrado correctamente")
        };
    }
}
