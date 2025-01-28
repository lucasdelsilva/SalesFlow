using FluentValidation;
using SalesFlow.Communication.Request.Sales;
using SalesFlow.Exception;

namespace SalesFlow.Application.UseCases.Sales.Validator;
public class SaleRequestValidator : AbstractValidator<RequestSaleCreateOrUpdateJson>
{
    public SaleRequestValidator()
    {
        RuleFor(x => x.CustomerName)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.CUSTOMER_NAME_REQUIRED);

        RuleFor(x => x.Items)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.SALE_MUST_HAVE_ONE_ITEM)
            .Must(items => items != null && items.Any())
            .WithMessage(ResourceErrorMessages.SALE_ITEM_MUST_HAVE_ONE_ITEM);

        RuleForEach(x => x.Items)
            .SetValidator(new SaleItemRequestValidator());
    }
}