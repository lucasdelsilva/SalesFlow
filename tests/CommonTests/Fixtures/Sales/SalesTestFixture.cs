using AutoMapper;
using Moq;
using SalesFlow.Application.UseCases.Sales;
using SalesFlow.Communication.Request.Sales;
using SalesFlow.Domain.Entities;
using SalesFlow.Domain.Repositories.Interfaces;
using SalesFlow.Domain.Repositories.Sales;
using SalesFlow.Domain.Services.LoggedUser;

namespace CommonTests.Fixtures.Sales;
public class SalesTestFixture : IDisposable
{
    public Mock<IMapper> MapperMock { get; private set; }
    public Mock<ISalesWriteOnlyRepository> SalesWriteOnlyRepositoryMock { get; private set; }
    public Mock<ISalesReadOnlyRepository> SalesReadOnlyRepositoryMock { get; private set; }
    public Mock<IUnitOfWork> UnitOfWorkMock { get; private set; }
    public Mock<ILoggedUser> LoggedUserMock { get; private set; }

    public SaleCreateUseCase CreateUseCase { get; private set; }
    public SaleGetAllUseCase GetAllUseCase { get; private set; }
    public SaleGetByIdUseCase GetByIdUseCase { get; private set; }
    public SaleUpdateUseCase UpdateUseCase { get; private set; }
    public SaleItemUpdateUseCase ItemUpdateUseCase { get; private set; }
    public SaleDeleteUseCase DeleteUseCase { get; private set; }

    private readonly User _defaultUser;
    private readonly string _defaultCustomerName = "Test Customer";

    public SalesTestFixture()
    {
        MapperMock = new Mock<IMapper>();
        SalesWriteOnlyRepositoryMock = new Mock<ISalesWriteOnlyRepository>();
        SalesReadOnlyRepositoryMock = new Mock<ISalesReadOnlyRepository>();
        UnitOfWorkMock = new Mock<IUnitOfWork>();
        LoggedUserMock = new Mock<ILoggedUser>();

        _defaultUser = new User { Id = 1, Name = "Test User" };
        LoggedUserMock.Setup(x => x.Get()).ReturnsAsync(_defaultUser);

        CreateUseCase = new SaleCreateUseCase(
            SalesWriteOnlyRepositoryMock.Object,
            SalesReadOnlyRepositoryMock.Object,
            UnitOfWorkMock.Object,
            MapperMock.Object,
            LoggedUserMock.Object
        );

        GetAllUseCase = new SaleGetAllUseCase(
            SalesReadOnlyRepositoryMock.Object,
            MapperMock.Object,
            LoggedUserMock.Object
        );

        GetByIdUseCase = new SaleGetByIdUseCase(
            SalesReadOnlyRepositoryMock.Object,
            MapperMock.Object,
            LoggedUserMock.Object
        );

        UpdateUseCase = new SaleUpdateUseCase(
            SalesWriteOnlyRepositoryMock.Object,
            SalesReadOnlyRepositoryMock.Object,
            UnitOfWorkMock.Object,
            LoggedUserMock.Object
        );

        ItemUpdateUseCase = new SaleItemUpdateUseCase(
            SalesWriteOnlyRepositoryMock.Object,
            SalesReadOnlyRepositoryMock.Object,
            UnitOfWorkMock.Object,
            MapperMock.Object,
            LoggedUserMock.Object
        );

        DeleteUseCase = new SaleDeleteUseCase(
            SalesWriteOnlyRepositoryMock.Object,
            SalesReadOnlyRepositoryMock.Object,
            UnitOfWorkMock.Object,
            MapperMock.Object,
            LoggedUserMock.Object
        );
    }

    public RequestSaleCreateJson GetValidCreateRequest()
    {
        return new RequestSaleCreateJson
        {
            CustomerName = _defaultCustomerName,
            Items = new List<RequestSaleItemCreateJson>
            {
                new()
                {
                    ProductName = "Product 1",
                    Quantity = 2,
                    UnitPrice = 10.0m
                }
            }
        };
    }

    public Sale GetValidSale()
    {
        return new Sale
        {
            Id = 1,
            CustomerName = _defaultCustomerName,
            UserId = _defaultUser.Id,
            TotalAmount = 20.0m,
            Items = new List<SaleItem?>
            {
                new SaleItem
                {
                    Id = 1,
                    ProductName = "Product 1",
                    Quantity = 2,
                    UnitPrice = 10.0m,
                    TotalPrice = 20.0m
                }
            }
        };
    }

    public List<Sale> GetValidSalesList()
    {
        return new List<Sale> { GetValidSale() };
    }

    public void ResetMocks()
    {
        MapperMock.Reset();
        SalesWriteOnlyRepositoryMock.Reset();
        SalesReadOnlyRepositoryMock.Reset();
        UnitOfWorkMock.Reset();
        LoggedUserMock.Reset();
        LoggedUserMock.Setup(x => x.Get()).ReturnsAsync(_defaultUser);
    }

    public void Dispose()
    {
        ResetMocks();
    }
}