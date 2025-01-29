using AutoMapper;
using SalesFlow.Application.UseCases.Sales.Interfaces;
using SalesFlow.Communication.Response.Sales;
using SalesFlow.Domain.Repositories.Sales;
using SalesFlow.Exception;
using SalesFlow.Exception.ExceptionBase;

namespace SalesFlow.Application.UseCases.Sales;
public class SaleGetByIdUseCase : ISaleGetByIdUseCase
{
    private readonly ISalesReadOnlyRepository _salesReadOnlyRepository;
    private readonly IMapper _mapper;

    public SaleGetByIdUseCase(ISalesReadOnlyRepository salesReadOnlyRepository, IMapper mapper)
    {
        _salesReadOnlyRepository = salesReadOnlyRepository;
        _mapper = mapper;
    }

    public async Task<ResponseSaleJson?> GetById(long id)
    {
        var sale = await _salesReadOnlyRepository.GetById(id);

        if (sale is null)
            throw new NotFoundException(ResourceErrorMessages.SALE_NOT_FOUND);


        return _mapper.Map<ResponseSaleJson>(sale);
    }
}
