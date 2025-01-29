using AutoMapper;
using SalesFlow.Application.UseCases.Sales.Interfaces;
using SalesFlow.Communication.Response.Sales;
using SalesFlow.Domain.Repositories.Sales;
using SalesFlow.Domain.Services.LoggedUser;
using SalesFlow.Exception;
using SalesFlow.Exception.ExceptionBase;

namespace SalesFlow.Application.UseCases.Sales;
public class SaleGetByIdUseCase : ISaleGetByIdUseCase
{
    private readonly ISalesReadOnlyRepository _salesReadOnlyRepository;
    private readonly IMapper _mapper;
    private readonly ILoggedUser _loggedUser;

    public SaleGetByIdUseCase(ISalesReadOnlyRepository salesReadOnlyRepository, IMapper mapper, ILoggedUser loggedUser)
    {
        _salesReadOnlyRepository = salesReadOnlyRepository;
        _mapper = mapper;
        _loggedUser = loggedUser;

    }

    public async Task<ResponseSaleJson?> GetById(long id)
    {
        var user = await _loggedUser.Get();
        var sale = await _salesReadOnlyRepository.GetById(user, id);

        if (sale is null)
            throw new NotFoundException(ResourceErrorMessages.SALE_NOT_FOUND);


        return _mapper.Map<ResponseSaleJson>(sale);
    }
}
