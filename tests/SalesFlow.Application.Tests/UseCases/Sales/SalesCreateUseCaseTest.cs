using CommonTests.Fixtures.Sales;
using FluentAssertions;
using Moq;
using SalesFlow.Communication.Response.Sales;
using SalesFlow.Domain.Entities;
using SalesFlow.Exception.ExceptionBase;

namespace SalesFlow.Application.Tests.UseCases.Sales;
public class SalesCreateUseCaseTest : IClassFixture<SalesTestFixture>
{
    private readonly SalesTestFixture _fixture;

    public SalesCreateUseCaseTest(SalesTestFixture fixture)
    {
        _fixture = fixture;
        _fixture.ResetMocks();
    }

    [Fact]
    public async Task Success_Create_And_ResponseSales()
    {
        var request = _fixture.GetValidCreateRequest();
        var sale = _fixture.GetValidSale();

        _fixture.MapperMock.Setup(x => x.Map<Sale>(request)).Returns(sale);
        _fixture.MapperMock.Setup(x => x.Map<ResponseSaleJson>(sale)).Returns(new ResponseSaleJson { Id = sale.Id, CustomerName = sale.CustomerName });
        _fixture.SalesReadOnlyRepositoryMock.Setup(x => x.GetAll(It.IsAny<User>())).ReturnsAsync(new List<Sale>());

        var response = await _fixture.CreateUseCase.Create(request);

        response.Should().NotBeNull();
        response.CustomerName.Should().Be(request.CustomerName);

        _fixture.SalesWriteOnlyRepositoryMock.Verify(x => x.Create(It.IsAny<Sale>()), Times.Once);
        _fixture.UnitOfWorkMock.Verify(x => x.Commit(), Times.Once);
    }

    [Fact]
    public async Task Create_CustomerName_Empty_Exception()
    {
        var request = _fixture.GetValidCreateRequest();
        request.CustomerName = string.Empty;

        await Assert.ThrowsAsync<ErrorOnValidationException>(() => _fixture.CreateUseCase.Create(request));
    }
}