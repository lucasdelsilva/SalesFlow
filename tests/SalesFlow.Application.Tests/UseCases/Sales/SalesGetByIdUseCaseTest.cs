using CommonTests.Fixtures.Sales;
using FluentAssertions;
using Moq;
using SalesFlow.Communication.Response.Sales;
using SalesFlow.Domain.Entities;
using SalesFlow.Exception.ExceptionBase;

namespace SalesFlow.Application.Tests.UseCases.Sales;
public class SalesGetByIdUseCaseTest : IClassFixture<SalesTestFixture>
{
    private readonly SalesTestFixture _fixture;

    public SalesGetByIdUseCaseTest(SalesTestFixture fixture)
    {
        _fixture = fixture;
        _fixture.ResetMocks();
    }

    [Fact]
    public async Task GetById_Success_ReturnSale()
    {
        var sale = _fixture.GetValidSale();
        var expectedResponse = new ResponseSaleJson { Id = sale.Id, CustomerName = sale.CustomerName };

        _fixture.SalesReadOnlyRepositoryMock.Setup(x => x.GetById(It.IsAny<User>(), sale.Id)).ReturnsAsync(sale);
        _fixture.MapperMock.Setup(x => x.Map<ResponseSaleJson>(sale)).Returns(expectedResponse);

        var response = await _fixture.GetByIdUseCase.GetById(sale.Id);

        response.Should().NotBeNull();
        response!.Id.Should().Be(sale.Id);
        response.CustomerName.Should().Be(sale.CustomerName);
    }

    [Fact]
    public async Task GetById_WhenSaleDoesNotExist_Exception()
    {
        long invalidId = 999;

        _fixture.SalesReadOnlyRepositoryMock.Setup(x => x.GetById(It.IsAny<User>(), invalidId)).ReturnsAsync((Sale)null!);
        await Assert.ThrowsAsync<NotFoundException>(() => _fixture.GetByIdUseCase.GetById(invalidId));
    }
}