using AutoMapper;
using SalesFlow.Application.UseCases.Sales.Interfaces;
using SalesFlow.Communication.Response.Sales;
using SalesFlow.Domain.Repositories.Sales;
using SalesFlow.Domain.Services.LoggedUser;

namespace SalesFlow.Application.UseCases.Sales;
public class SaleGetAllUseCase(ISalesReadOnlyRepository salesReadOnlyRepository, IMapper mapper, ILoggedUser loggedUser) : ISaleGetAllUseCase
{
    private readonly ISalesReadOnlyRepository _salesReadOnlyRepository = salesReadOnlyRepository;
    private readonly IMapper _mapper = mapper;
    private readonly ILoggedUser _loggedUser = loggedUser;

    public async Task<ResponseSalesJson> GetAll()
    {
        var user = await _loggedUser.Get();
        var sales = await _salesReadOnlyRepository.GetAll(user);
        return _mapper.Map<ResponseSalesJson>(sales);
    }
}