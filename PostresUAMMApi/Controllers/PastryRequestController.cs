using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using PostresUAMMApi.Models;
using PostresUAMMApi.Models.Forms;
using PostresUAMMApi.Services;
using System.Net;

namespace PostresUAMMApi.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class PastryRequestController(IPastryRequestService pastryRequestService) : ControllerBase
{
    private readonly IPastryRequestService _pastryRequestService = pastryRequestService;

    [HttpPost]
    [Authorize]
    [Route("SendPastryRequest")]
    public async Task<ActionResult<PastryRequest>> SendPastryRequest(PastryRequestForm form)
    {
        try
        {
            StringValues accessToken = await HttpContext.GetTokenAsync(HeaderNames.Authorization);

            PastryRequest pastryRequest = await _pastryRequestService.MakePastryRequest(form);

            return StatusCode((int)HttpStatusCode.Created, pastryRequest);
        }
        catch (Exception e) when (e is ArgumentException || e is ArgumentNullException)
        {
            return StatusCode((int)HttpStatusCode.BadRequest, e.Message);
        }
        catch (Exception)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, "Un error interno ha ocurrido");
        }
    }
}
