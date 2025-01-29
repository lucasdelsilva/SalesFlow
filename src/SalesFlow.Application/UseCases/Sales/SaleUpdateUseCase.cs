using SalesFlow.Application.UseCases.Sales.Interfaces;
using SalesFlow.Application.UseCases.Sales.Validator;
using SalesFlow.Communication.Request.Sales;
using SalesFlow.Domain.Repositories.Interfaces;
using SalesFlow.Domain.Repositories.Sales;
using SalesFlow.Exception;
using SalesFlow.Exception.ExceptionBase;

namespace SalesFlow.Application.UseCases.Sales;

public class SaleUpdateUseCase : ISaleUpdateUseCase
{
    private readonly ISalesWriteOnlyRepository _salesWriteOnlyRepository;
    private readonly ISalesReadOnlyRepository _salesReadOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SaleUpdateUseCase(ISalesWriteOnlyRepository salesWriteOnlyRepository, ISalesReadOnlyRepository salesReadOnlyRepository, IUnitOfWork unitOfWork)
    {
        _salesWriteOnlyRepository = salesWriteOnlyRepository;
        _unitOfWork = unitOfWork;
        _salesReadOnlyRepository = salesReadOnlyRepository;
    }

    public async Task Update(long id, RequestSaleUpdateJson request)
    {
        Validate(request);

        var sale = await _salesReadOnlyRepository.UpdateOrRemoveGetById(id);
        if (sale is null)
            throw new NotFoundException(ResourceErrorMessages.SALE_NOT_FOUND);

        // Atualiza apenas o nome do cliente
        sale.CustomerName = request.CustomerName;

        _salesWriteOnlyRepository.Update(sale);
        await _unitOfWork.Commit();
    }

    private void Validate(RequestSaleUpdateJson request)
    {
        var validator = new SaleUpdateValidator();
        var result = validator.Validate(request);

        if (!result.IsValid)
        {
            var validationErrors = result.Errors
                .Select(error => new
                {
                    error.PropertyName,
                    Message = error.ErrorMessage
                })
                .ToList<object>();

            throw new ErrorOnValidationException(validationErrors);
        }
    }
}