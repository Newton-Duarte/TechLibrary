using TechLibrary.Api.Domain.Entities;
using TechLibrary.Api.Infra.DataAccess;
using TechLibrary.Communication.Requests;
using TechLibrary.Communication.Responses;

namespace TechLibrary.Api.UseCases.Books.List;

public class ListBooksUseCase
{
    private const int PAGE_SIZE = 10;

    public ResponseListBooksJson Execute(RequestBooksJson request)
    {
        var dbContext = new TechLibraryDbContext();

        var skip = (request.Page - 1) * PAGE_SIZE;

        var books = dbContext
            .Books
            .Where(book => string.IsNullOrEmpty(request.Title) || book.Title.Contains(request.Title))
            .OrderBy(book => book.Title).ThenBy(book => book.Author)
            .Skip(skip)
            .Take(PAGE_SIZE)
            .ToList();

        var totalCount = dbContext.Books.Count(book => string.IsNullOrEmpty(request.Title) || book.Title.Contains(request.Title));

        return new ResponseListBooksJson
        {
            Pagination = new ResponsePaginationJson
            {
                Page = request.Page,
                TotalCount = totalCount,
            },
            Books = books.Select(book => new ResponseBookJson
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
            }).ToList()
        };
    }
}
