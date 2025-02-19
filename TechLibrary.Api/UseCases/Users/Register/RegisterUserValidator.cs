using FluentValidation;
using TechLibrary.Communication.Requests;

namespace TechLibrary.Api.UseCases.Users.Register;

public class RegisterUserValidator : AbstractValidator<RequestUserJson>
{
    public RegisterUserValidator()
    {
        RuleFor(request => request.Name).NotEmpty().WithMessage("O nome é obrigatório.");
        RuleFor(request => request.Email).NotEmpty().EmailAddress().WithMessage("E-mail inválido.");
        When(request => !string.IsNullOrEmpty(request.Password), () =>
        {
            RuleFor(request => request.Password.Length).GreaterThanOrEqualTo(6).WithMessage("A senha deve ter 6 ou mais caracteres..");
        });
    }
}
