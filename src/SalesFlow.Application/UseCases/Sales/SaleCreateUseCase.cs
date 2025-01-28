using AutoMapper;
using SalesFlow.Application.UseCases.Sales.Interfaces;
using SalesFlow.Application.UseCases.Sales.Validator;
using SalesFlow.Communication.Request.Sales;
using SalesFlow.Communication.Response.Sales;
using SalesFlow.Domain.Entities;
using SalesFlow.Domain.Repositories.Interfaces;
using SalesFlow.Domain.Repositories.Sales;
using SalesFlow.Exception.ExceptionBase;

namespace SalesFlow.Application.UseCases.Sales;
public class SaleCreateUseCase : ISaleCreateUseCase
{
    private readonly ISalesWriteOnlyRepository _salesWriteOnlyRepository;
    private readonly ISalesReadOnlyRepository _salesReadOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public SaleCreateUseCase(ISalesWriteOnlyRepository salesWriteOnlyRepository, ISalesReadOnlyRepository salesReadOnlyRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _salesWriteOnlyRepository = salesWriteOnlyRepository;
        _unitOfWork = unitOfWork;
        _salesReadOnlyRepository = salesReadOnlyRepository;
    }
    public async Task<ResponseSaleCreateJson> Create(RequestSaleCreateOrUpdateJson request)
    {
        Validate(request);

        var sale = _mapper.Map<Sale>(request);
        sale.TotalAmount = sale.Items.Sum(item => item!.TotalPrice);

        var countProducts = await _salesReadOnlyRepository.GetAll();
        if (countProducts.Count > 0)
            sale.Number = countProducts.Count + 1;
        else
            sale.Number = 1;

        await _salesWriteOnlyRepository.Create(sale);
        await _unitOfWork.Commit();

        return _mapper.Map<ResponseSaleCreateJson>(sale);
    }
    private void Validate(RequestSaleCreateOrUpdateJson request)
    {
        var validator = new SaleRequestValidator();

        var result = validator.Validate(request);
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
