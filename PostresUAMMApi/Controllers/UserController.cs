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
            UserCredential userCredential = await _firebaseAuthService.SignUpCustomer(form);
            Customer newCustomer = await _userService.AddCustomerAsync(userCredential);

            return StatusCode((int) HttpStatusCode.Created, newCustomer);
        }
        catch (Exception e) when (e is ArgumentException || e is ArgumentNullException)
        {
            return StatusCode((int) HttpStatusCode.BadRequest, e.Message);
        }
        catch (Exception e)
        {
            return StatusCode((int) HttpStatusCode.InternalServerError, e.Message);
        }
    }
}
