using AutoMapper;
using SalesFlow.Application.UseCases.Sales.Interfaces;
using SalesFlow.Application.UseCases.Sales.Validator;
using SalesFlow.Communication.Request.Sales;
using SalesFlow.Domain.Entities;
using SalesFlow.Domain.Repositories.Interfaces;
using SalesFlow.Domain.Repositories.Sales;
using SalesFlow.Exception;
using SalesFlow.Exception.ExceptionBase;

namespace SalesFlow.Application.UseCases.Sales;
public class SaleItemUpdateUseCase : ISaleItemUpdateUseCase
{
    private readonly ISalesWriteOnlyRepository _salesWriteOnlyRepository;
    private readonly ISalesReadOnlyRepository _salesReadOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public SaleItemUpdateUseCase(ISalesWriteOnlyRepository salesWriteOnlyRepository, ISalesReadOnlyRepository salesReadOnlyRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _salesWriteOnlyRepository = salesWriteOnlyRepository;
        _unitOfWork = unitOfWork;
        _salesReadOnlyRepository = salesReadOnlyRepository;
    }

    public async Task UpdateItem(long saleId, RequestSaleItemUpdateJson request)
    {
        Validate(request);

        var sale = await _salesReadOnlyRepository.UpdateOrRemoveGetById(saleId);
        if (sale is null)
            throw new NotFoundException(ResourceErrorMessages.SALE_NOT_FOUND);

        var item = sale.Items.FirstOrDefault(i => i?.Id == request.Id);
        if (item is null)
            throw new NotFoundException(string.Format(ResourceErrorMessages.SALE_ITEM_NOT_FOUND, request.Id));

        UpdateItem(item, request);
        UpdateSaleTotalAmount(sale);

        _salesWriteOnlyRepository.Update(sale);
        await _unitOfWork.Commit();
    }

    private void UpdateItem(SaleItem item, RequestSaleItemUpdateJson request)
    {
        _mapper.Map(request, item);
        item.CalculateTotalPrice();
    }

    private static void UpdateSaleTotalAmount(Sale sale)
    {
        sale.TotalAmount = sale.Items.Sum(item => item?.TotalPrice ?? 0);
    }

    private void Validate(RequestSaleItemUpdateJson request)
    {
        var validator = new SaleItemUpdateValidator();
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