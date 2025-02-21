using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechLibrary.Api.Services.LoggedUser;
using TechLibrary.Api.UseCases.Checkouts;

namespace TechLibrary.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class CheckoutsController : ControllerBase
{
    [HttpPost]
    [Route("{bookId}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult BookCheckout(Guid bookId)
    {
        var loggedUserService = new LoggedUserService(HttpContext);

        var useCase = new RegisterCheckoutUseCase(loggedUserService);

        useCase.Execute(bookId);

        return Created("", null);
    }
}
