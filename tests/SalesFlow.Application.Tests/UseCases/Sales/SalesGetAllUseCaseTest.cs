using CommonTests.Fixtures.Sales;
using FluentAssertions;
using Moq;
using SalesFlow.Communication.Response.Sales;
using SalesFlow.Domain.Entities;

namespace SalesFlow.Application.Tests.UseCases.Sales;
public class SalesGetAllUseCaseTest : IClassFixture<SalesTestFixture>
{
    private readonly SalesTestFixture _fixture;

    public SalesGetAllUseCaseTest(SalesTestFixture fixture)
    {
        _fixture = fixture;
        _fixture.ResetMocks();
    }

    [Fact]
    public async Task GetAll_Success_ReturnSalesList()
    {
        var sales = _fixture.GetValidSalesList();
        var expectedResponse = new ResponseSalesJson { Sales = [] };

        _fixture.SalesReadOnlyRepositoryMock.Setup(x => x.GetAll(It.IsAny<User>())).ReturnsAsync(sales);
        _fixture.MapperMock.Setup(x => x.Map<ResponseSalesJson>(sales)).Returns(expectedResponse);

        var response = await _fixture.GetAllUseCase.GetAll();
        response.Should().NotBeNull();

        _fixture.SalesReadOnlyRepositoryMock.Verify(x => x.GetAll(It.IsAny<User>()), Times.Once);
    }
}