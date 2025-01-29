using AutoMapper;
using SalesFlow.Application.UseCases.Sales.Interfaces;
using SalesFlow.Domain.Repositories.Interfaces;
using SalesFlow.Domain.Repositories.Sales;
using SalesFlow.Domain.Services.LoggedUser;
using SalesFlow.Exception;
using SalesFlow.Exception.ExceptionBase;

namespace SalesFlow.Application.UseCases.Sales;
public class SaleDeleteUseCase : ISaleDeleteUseCase
{
    private readonly ISalesWriteOnlyRepository _salesWriteOnlyRepository;
    private readonly ISalesReadOnlyRepository _salesReadOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILoggedUser _loggedUser;

    public SaleDeleteUseCase(ISalesWriteOnlyRepository salesWriteOnlyRepository, ISalesReadOnlyRepository salesReadOnlyRepository, IUnitOfWork unitOfWork, IMapper mapper, ILoggedUser loggedUser)
    {
        _mapper = mapper;
        _salesWriteOnlyRepository = salesWriteOnlyRepository;
        _unitOfWork = unitOfWork;
        _salesReadOnlyRepository = salesReadOnlyRepository;
        _loggedUser = loggedUser;

    }

    public async Task Delete(long id)
    {
        var user = await _loggedUser.Get();
        var expense = await _salesReadOnlyRepository.UpdateOrRemoveGetById(user, id);

        if (expense is null)
            throw new NotFoundException(ResourceErrorMessages.SALE_NOT_FOUND);

        await _salesWriteOnlyRepository.Delete(id);
        await _unitOfWork.Commit();
    }
}