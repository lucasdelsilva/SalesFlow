using CommonTests.Fixtures.Sales;
using Moq;
using SalesFlow.Communication.Request.Sales;
using SalesFlow.Domain.Entities;
using SalesFlow.Exception.ExceptionBase;

namespace SalesFlow.Application.Tests.UseCases.Sales;
public class SalesUpdateUseCaseTest : IClassFixture<SalesTestFixture>
{
    private readonly SalesTestFixture _fixture;

    public SalesUpdateUseCaseTest(SalesTestFixture fixture)
    {
        _fixture = fixture;
        _fixture.ResetMocks();
    }

    [Fact]
    public async Task Update_Success_UpdateSale()
    {
        var sale = _fixture.GetValidSale();
        var request = new RequestSaleUpdateJson { CustomerName = "Updated Customer" };

        _fixture.SalesReadOnlyRepositoryMock.Setup(x => x.UpdateOrRemoveGetById(It.IsAny<User>(), sale.Id)).ReturnsAsync(sale);
        await _fixture.UpdateUseCase.Update(sale.Id, request);

        _fixture.SalesWriteOnlyRepositoryMock.Verify(x => x.Update(It.IsAny<Sale>()), Times.Once);
        _fixture.UnitOfWorkMock.Verify(x => x.Commit(), Times.Once);
    }

    [Fact]
    public async Task Update_WhenSaleDoesNotExist_Exception()
    {
        long invalidId = 999;
        var request = new RequestSaleUpdateJson { CustomerName = "Updated Customer" };

        _fixture.SalesReadOnlyRepositoryMock.Setup(x => x.UpdateOrRemoveGetById(It.IsAny<User>(), invalidId)).ReturnsAsync((Sale)null!);
        await Assert.ThrowsAsync<NotFoundException>(() => _fixture.UpdateUseCase.Update(invalidId, request));
    }
}