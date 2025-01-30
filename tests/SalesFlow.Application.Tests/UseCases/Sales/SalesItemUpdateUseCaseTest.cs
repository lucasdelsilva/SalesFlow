using CommonTests.Fixtures.Sales;
using Moq;
using SalesFlow.Communication.Request.Sales;
using SalesFlow.Domain.Entities;
using SalesFlow.Exception.ExceptionBase;

namespace SalesFlow.Application.Tests.UseCases.Sales;
public class SalesItemUpdateUseCaseTest : IClassFixture<SalesTestFixture>
{
    private readonly SalesTestFixture _fixture;

    public SalesItemUpdateUseCaseTest(SalesTestFixture fixture)
    {
        _fixture = fixture;
        _fixture.ResetMocks();
    }

    [Fact]
    public async Task UpdateItem_Success_UpdateSaleItem()
    {
        var sale = _fixture.GetValidSale();
        var request = new RequestSaleItemUpdateJson
        {
            Id = sale.Items.First()!.Id,
            ProductName = "Updated Product",
            Quantity = 3,
            UnitPrice = 15.0m
        };

        _fixture.SalesReadOnlyRepositoryMock.Setup(x => x.UpdateOrRemoveGetById(It.IsAny<User>(), sale.Id)).ReturnsAsync(sale);
        await _fixture.ItemUpdateUseCase.UpdateItem(sale.Id, request);

        _fixture.SalesWriteOnlyRepositoryMock.Verify(x => x.Update(It.IsAny<Sale>()), Times.Once);
        _fixture.UnitOfWorkMock.Verify(x => x.Commit(), Times.Once);
    }

    [Fact]
    public async Task UpdateItem_WhenItemNotFound_Exception()
    {
        var sale = _fixture.GetValidSale();
        var request = new RequestSaleItemUpdateJson { Id = 999 };

        _fixture.SalesReadOnlyRepositoryMock.Setup(x => x.UpdateOrRemoveGetById(It.IsAny<User>(), sale.Id)).ReturnsAsync(sale);
        await Assert.ThrowsAsync<ErrorOnValidationException>(() => _fixture.ItemUpdateUseCase.UpdateItem(sale.Id, request));
    }
}