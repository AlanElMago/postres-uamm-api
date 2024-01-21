using Firebase.Auth;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using PostresUAMMApi.Enums;
using PostresUAMMApi.Models;
using PostresUAMMApi.Models.Forms;
using PostresUAMMApi.Services;
using System.Net;

namespace PostresUAMMApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(
    IFirebaseAuthService firebaseAuthService,
    IUserService userService) : ControllerBase
{
    private readonly IFirebaseAuthService _firebaseAuthService = firebaseAuthService;
    private readonly IUserService _userService = userService;

    [HttpPost]
    [Route("RegisterCustomer")]
    public async Task<ActionResult<Customer>> RegisterCustomer(UserRegistrationForm form)
    {
        try
        {
            UserCredential userCredential = await _firebaseAuthService.SignUpCustomerAsync(form);
            Customer newCustomer = await _userService.AddCustomerAsync(userCredential);

            // TODO: Set location uri header after creating the get customer endpoint
            return StatusCode((int)HttpStatusCode.Created, newCustomer);
        }
        catch (Exception e) when (e is ArgumentException || e is ArgumentNullException)
        {
            return StatusCode((int)HttpStatusCode.BadRequest, e.Message);
        }
        catch (FirebaseAuthException e)
        {
            return e.Reason switch
            {
                AuthErrorReason.EmailExists
                    => StatusCode((int)HttpStatusCode.Conflict, "El correo ya fué registrado"),
                AuthErrorReason.WeakPassword
                    => StatusCode((int)HttpStatusCode.BadRequest, "La contraseña debe tener al menos 6 caracteres"),
                AuthErrorReason.InvalidEmailAddress
                    => StatusCode((int)HttpStatusCode.BadRequest, "El correo electrónico no es válido"),
                AuthErrorReason.MissingEmail
                    => StatusCode((int)HttpStatusCode.BadRequest, "El correo electrónico es requerido"),
                AuthErrorReason.MissingPassword
                    => StatusCode((int)HttpStatusCode.BadRequest, "La contraseña es requerida"),
                _   => throw new Exception("Uncaught firebase authentication error")
            };
        }
        catch (Exception)
        {
            // TODO: Log exception
            return StatusCode((int)HttpStatusCode.InternalServerError, "Un error interno ha ocurrido");
        }
    }
}
