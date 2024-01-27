using Microsoft.AspNetCore.Mvc;
using PostresUAMMApi.Models;
using PostresUAMMApi.Models.Forms;
using PostresUAMMApi.Services;
using System.Net;

namespace PostresUAMMApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(
    // IFirebaseAuthService firebaseAuthService,
    IUserService userService) : ControllerBase
{
    // private readonly IFirebaseAuthService _firebaseAuthService = firebaseAuthService;
    private readonly IUserService _userService = userService;

    [HttpPost]
    [Route("RegisterCustomer")]
    public async Task<ActionResult> RegisterCustomer(UserRegistrationForm form)
    {
        throw new NotImplementedException();
    }

    [HttpPost]
    [Route("Login")]
    public async Task<ActionResult> Login(UserLoginForm form)
    {
        throw new NotImplementedException();
    }
}
