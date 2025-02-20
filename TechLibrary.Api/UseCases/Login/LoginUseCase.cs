using TechLibrary.Api.Infra.DataAccess;
using TechLibrary.Api.Infra.Security.Cryptography;
using TechLibrary.Api.Infra.Security.Tokens.Access;
using TechLibrary.Communication.Requests;
using TechLibrary.Communication.Responses;
using TechLibrary.Exception;

namespace TechLibrary.Api.UseCases.Login;

public class LoginUseCase
{
    public ResponseLoginJson Execute(RequestLoginJson request)
    {
        var dbContext = new TechLibraryDbContext();

        var user = dbContext.Users.FirstOrDefault(user => user.Email == request.Email);

        if (user is null)
        {
            throw new InvalidLoginException();
        }

        var cryptography = new BCryptAlgorithm();
        var passwordIsvalid = cryptography.VerifyPassword(request.Password, user);

        if (!passwordIsvalid)
        {
            throw new InvalidLoginException();
        }

        var tokenGenerator = new JwtTokenGenerator();

        return new ResponseLoginJson
        {
            Name = user.Name,
            AccessToken = tokenGenerator.Generate(user),
        };
    }
}
