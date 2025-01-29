using AutoMapper;
using FluentValidation.Results;
using SalesFlow.Application.UseCases.User.Interface;
using SalesFlow.Application.UseCases.User.Validator;
using SalesFlow.Communication.Request.User;
using SalesFlow.Communication.Response.User;
using SalesFlow.Domain.Repositories.Interfaces;
using SalesFlow.Domain.Repositories.User;
using SalesFlow.Domain.Security.Cryptography;
using SalesFlow.Domain.Security.Tokens;
using SalesFlow.Exception;
using SalesFlow.Exception.ExceptionBase;

namespace SalesFlow.Application.UseCases.User;
public class RegisterUserUseCase : IRegisterUserUseCase
{
    private readonly IMapper _mapper;
    private readonly IPasswordEncripter _passwordEncripter;
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IUserWriteOnlyRepository _userWriteOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAccessTokenGenerator _accessTokenGenerator;
    public RegisterUserUseCase(IMapper mapper, IPasswordEncripter passwordEncripter,
        IUserReadOnlyRepository userReadOnlyRepository, IUserWriteOnlyRepository userWriteOnlyRepository,
        IUnitOfWork unitOfWork, IAccessTokenGenerator accessTokenGenerator)
    {
        _mapper = mapper;
        _passwordEncripter = passwordEncripter;
        _userReadOnlyRepository = userReadOnlyRepository;
        _userWriteOnlyRepository = userWriteOnlyRepository;
        _unitOfWork = unitOfWork;
        _accessTokenGenerator = accessTokenGenerator;
    }

    public async Task<ResponseUserJson> Register(RequestRegisterUserJson request)
    {
        await Validate(request);

        var user = _mapper.Map<Domain.Entities.User>(request);
        user.Password = _passwordEncripter.Encrypt(request.Password);

        await _userWriteOnlyRepository.Add(user);
        await _unitOfWork.Commit();

        return new ResponseUserJson
        {
            Name = user.Name,
            Token = _accessTokenGenerator.GeneratorToken(user)
        };
    }

    private async Task Validate(RequestRegisterUserJson request)
    {
        var result = new RegisterUserValidator().Validate(request);

        var exist = await _userReadOnlyRepository.ExistActiveUserWithEmail(request.Email);
        if (exist)
            result.Errors.Add(new ValidationFailure("Email", ResourceErrorMessages.EMAIL_ALREADY_REGISTERED));

        if (!result.IsValid)
        {
            var objErros = new List<Object>();
            foreach (var item in result.Errors)
            {
                var objModel = new { item.PropertyName, Message = item.ErrorMessage };
                objErros.Add(objModel);
            }
            throw new ErrorOnValidationException(objErros);
        }
    }
}
