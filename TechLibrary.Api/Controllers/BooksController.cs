using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechLibrary.Api.UseCases.Books.List;
using TechLibrary.Communication.Requests;
using TechLibrary.Communication.Responses;

namespace TechLibrary.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(ResponseListBooksJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseListBooksJson), StatusCodes.Status204NoContent)]
    public IActionResult Get(int page, string? title)
    {
        var useCase = new ListBooksUseCase();
        var result = useCase.Execute(new RequestBooksJson
        {
            Page = page,
            Title = title
        });

        return Ok(result);
    }
}
