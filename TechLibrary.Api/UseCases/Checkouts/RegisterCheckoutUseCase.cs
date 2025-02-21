using TechLibrary.Api.Domain.Entities;
using TechLibrary.Api.Infra.DataAccess;
using TechLibrary.Api.Services.LoggedUser;
using TechLibrary.Exception;

namespace TechLibrary.Api.UseCases.Checkouts;

public class RegisterCheckoutUseCase
{
    private const int MAX_LOAN_DAYS = 7;

    private readonly LoggedUserService _loggedUserService;

    public RegisterCheckoutUseCase(LoggedUserService loggedUserService)
    {
        _loggedUserService = loggedUserService;
    }

    public void Execute(Guid bookId)
    {
        var dbContext = new TechLibraryDbContext();

        Validate(dbContext, bookId);

        var book = dbContext.Books.Find(bookId);
        var user = _loggedUserService.GetLoggedUser(dbContext);

        dbContext.Checkouts.Add(new Checkout
        {
            UserId = user.Id,
            BookId = bookId,
            ExpectedReturnDate = DateTime.UtcNow.AddDays(MAX_LOAN_DAYS)
        });

        dbContext.SaveChanges();
    }

    private void Validate(TechLibraryDbContext dbContext, Guid bookId)
    {
        var book = dbContext.Books.FirstOrDefault(book => book.Id == bookId);

        if (book is null)
        {
            throw new NotFoundException("Book not found");
        }

        var notReturnedBooks = dbContext.Checkouts.Count(checkout => checkout.BookId == bookId && checkout.ReturnedDate == null);

        if (notReturnedBooks == book.Amount)
        {
            throw new ConflictException("Book is already checked out");
        }
    }
}
