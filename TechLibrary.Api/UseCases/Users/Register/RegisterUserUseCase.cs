﻿using TechLibrary.Api.Domain.Entities;
using TechLibrary.Api.Infra.DataAccess;
using TechLibrary.Api.Infra.Security.Cryptography;
using TechLibrary.Communication.Requests;
using TechLibrary.Communication.Responses;
using TechLibrary.Exception;

namespace TechLibrary.Api.UseCases.Users.Register;

public class RegisterUserUseCase
{
    public ResponseUserJson Execute(RequestUserJson request)
    {
        Validate(request);

        var cryptography = new BCryptAlgorithm();
        var entity = new User
        {
            Name = request.Name,
            Email = request.Email,
            Password = cryptography.HashPassword(request.Password),
        };

        var dbContext = new TechLibraryDbContext();
        dbContext.Users.Add(entity);
        dbContext.SaveChanges();

        return new ResponseUserJson
        {
            Name = entity.Name,
        };
    }

    private void Validate(RequestUserJson request)
    {
        var validator = new RegisterUserValidator();
        var result = validator.Validate(request);

        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
