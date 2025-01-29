using FluentValidation;
using SalesFlow.Communication.Request.Sales;
using SalesFlow.Exception;

namespace SalesFlow.Application.UseCases.Sales.Validator;
public class SaleUpdateValidator : AbstractValidator<RequestSaleUpdateJson>
{
    public SaleUpdateValidator()
    {
        RuleFor(x => x.CustomerName)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.CUSTOMER_NAME_REQUIRED);
    }
}