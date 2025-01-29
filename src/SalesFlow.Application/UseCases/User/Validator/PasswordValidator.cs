using FluentValidation;
using FluentValidation.Validators;
using SalesFlow.Exception;
using System.Text.RegularExpressions;

public partial class PasswordValidator<T> : PropertyValidator<T, string>
{
    public const string ERROR_MESSAGE_KEY = "ErrorMessage";
    public override string Name { get; } = "Password";

    protected override string GetDefaultMessageTemplate(string errorCode)
    {
        return $"{{{ERROR_MESSAGE_KEY}}}";
    }
    public override bool IsValid(ValidationContext<T> context, string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            context.MessageFormatter.AppendArgument(ERROR_MESSAGE_KEY, ResourceErrorMessages.INVALID_PASSWORD);
            return false;
        }

        if (password.Length < 8)
        {
            context.MessageFormatter.AppendArgument(ERROR_MESSAGE_KEY, ResourceErrorMessages.PASSWORD_LESSTHAN_8);
            return false;
        }

        if (!UpperCaseLetter().IsMatch(password))
        {
            context.MessageFormatter.AppendArgument(ERROR_MESSAGE_KEY, ResourceErrorMessages.PASSWORD_UPPER_CASE_LETTER);
            return false;
        }

        if (!LowerCaseLetter().IsMatch(password))
        {
            context.MessageFormatter.AppendArgument(ERROR_MESSAGE_KEY, ResourceErrorMessages.PASSWORD_LOWER_CASE_LETTER);
            return false;
        }

        if (!Numbers().IsMatch(password))
        {
            context.MessageFormatter.AppendArgument(ERROR_MESSAGE_KEY, ResourceErrorMessages.PASSWORD_NUMBER);
            return false;
        }

        if (!SpecialCharacters().IsMatch(password))
        {
            context.MessageFormatter.AppendArgument(ERROR_MESSAGE_KEY, ResourceErrorMessages.PASSWORD_SPECIAL_CHARACTERS);
            return false;
        }

        return true;
    }

    [GeneratedRegex(@"[A-Z]+")]
    private static partial Regex UpperCaseLetter();
    [GeneratedRegex(@"[a-z]+")]
    private static partial Regex LowerCaseLetter();
    [GeneratedRegex(@"[0-9]+")]
    private static partial Regex Numbers();
    [GeneratedRegex(@"[\!\@\#\*\.]+")]
    private static partial Regex SpecialCharacters();
}