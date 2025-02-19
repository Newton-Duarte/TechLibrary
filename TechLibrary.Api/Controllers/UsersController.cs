using Microsoft.AspNetCore.Mvc;
using TechLibrary.Api.UseCases.Users.Register;
using TechLibrary.Communication.Requests;
using TechLibrary.Communication.Responses;
using TechLibrary.Exception;

namespace TechLibrary.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(ResponseUserJson), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorMessagesJson), StatusCodes.Status400BadRequest)]
        public IActionResult Create(RequestUserJson request)
        {
            try
            {
                var useCase = new RegisterUserUseCase();
                var response = useCase.Execute(request);

                return Created(string.Empty, response);
            }
            catch (TechLibraryException ex)
            {
                return BadRequest(new ResponseErrorMessagesJson
                {
                    ErrorMessages = ex.GetErrorMessages()
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorMessagesJson
                {
                    ErrorMessages = new List<string> { "An error occurred while processing the request." }
                });
            }
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}
