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
public class CustomerController(ICustomerService customerService) : ControllerBase
{
    private readonly ICustomerService _customerService = customerService;

    [HttpPost]
    public async Task<HttpResponseMessage> RegisterCustomer(CustomerRegistrationForm form)
    {
        try
        {
            Customer customer = await _customerService.RegisterCustomerAsync(form);
        }
        catch (Exception e)
        {
            return new HttpResponseMessage ()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Content = new StringContent(e.Message)
            };
        }

        return new HttpResponseMessage()
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent("El usuario se ha registrado correctamente")
        };
    }
}
