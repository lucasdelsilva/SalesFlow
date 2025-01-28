using AutoMapper;
using SalesFlow.Application.UseCases.Sales.Interfaces;
using SalesFlow.Communication.Response.Sales;
using SalesFlow.Domain.Repositories.Sales;

namespace SalesFlow.Application.UseCases.Sales;
public class SaleGetAllUseCase(ISalesReadOnlyRepository salesReadOnlyRepository, IMapper mapper) : ISaleGetAllUseCase
{
    private readonly ISalesReadOnlyRepository _salesReadOnlyRepository = salesReadOnlyRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<ResponseSalesJson> GetAll()
    {
        var sales = await _salesReadOnlyRepository.GetAll();
        return _mapper.Map<ResponseSalesJson>(sales);
    }
}