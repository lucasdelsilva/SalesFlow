using FluentValidation;
using SalesFlow.Communication.Request.User;
using SalesFlow.Exception;

namespace SalesFlow.Application.UseCases.User.Validator;
public class RegisterUserValidator : AbstractValidator<RequestRegisterUserJson>
{
    public RegisterUserValidator()
    {
        RuleFor(user => user.Name).NotEmpty().WithMessage(ResourceErrorMessages.NAME_EMPTY);
        RuleFor(user => user.Email)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.EMAIL_EMPTY)
            .EmailAddress()
            .When(e => string.IsNullOrWhiteSpace(e.Email).Equals(false), ApplyConditionTo.CurrentValidator)
            .WithMessage(ResourceErrorMessages.EMAIL_INVALID);

        RuleFor(user => user.Password).SetValidator(new PasswordValidator<RequestRegisterUserJson>());
        RuleFor(user => user.PasswordConfirm).Equal(user => user.Password).WithMessage(ResourceErrorMessages.PASSWORD_EQUAL);
    }
}