using FluentValidation;
using SalesFlow.Communication.Request.Sales;
using SalesFlow.Exception;

namespace SalesFlow.Application.UseCases.Sales.Validator;
public class SaleItemUpdateValidator : AbstractValidator<RequestSaleItemUpdateJson>
{
    public SaleItemUpdateValidator()
    {
        RuleFor(x => x.ProductName)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.PRODUCT_NAME_REQUIRED);

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage(ResourceErrorMessages.QUANTITY_MUST_GREATER_THAN_ZERO);

        RuleFor(x => x.UnitPrice)
            .GreaterThan(0)
            .WithMessage(ResourceErrorMessages.UNITY_VALUE_MUST_GREATER_THAN_ZERO);
    }
}