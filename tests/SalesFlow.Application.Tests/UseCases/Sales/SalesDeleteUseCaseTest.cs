using CommonTests.Fixtures.Sales;
using FluentAssertions;
using Moq;
using SalesFlow.Domain.Entities;
using SalesFlow.Exception;
using SalesFlow.Exception.ExceptionBase;

namespace SalesFlow.Application.Tests.UseCases.Sales;
public class SalesDeleteUseCaseTest : IClassFixture<SalesTestFixture>
{
    private readonly SalesTestFixture _fixture;

    public SalesDeleteUseCaseTest(SalesTestFixture fixture)
    {
        _fixture = fixture;
        _fixture.ResetMocks();
    }

    [Fact]
    public async Task Success_Delete_Sale()
    {
        var sale = _fixture.GetValidSale();

        _fixture.SalesReadOnlyRepositoryMock.Setup(x => x.UpdateOrRemoveGetById(It.IsAny<User>(), sale.Id)).ReturnsAsync(sale);
        await _fixture.DeleteUseCase.Delete(sale.Id);

        _fixture.SalesWriteOnlyRepositoryMock.Verify(x => x.Delete(sale.Id), Times.Once);
        _fixture.UnitOfWorkMock.Verify(x => x.Commit(), Times.Once);
    }

    [Fact]
    public async Task Delete_WhenSaleDoesNotExist_Exception()
    {
        long invalidId = 999;

        _fixture.SalesReadOnlyRepositoryMock.Setup(x => x.UpdateOrRemoveGetById(It.IsAny<User>(), invalidId)).ReturnsAsync((Sale)null!);
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _fixture.DeleteUseCase.Delete(invalidId));
        exception.Message.Should().Be(ResourceErrorMessages.SALE_NOT_FOUND);

        _fixture.SalesWriteOnlyRepositoryMock.Verify(x => x.Delete(It.IsAny<long>()), Times.Never);
        _fixture.UnitOfWorkMock.Verify(x => x.Commit(), Times.Never);
    }

    [Fact]
    public async Task Delete_WhenUserNotAuthorized_Exception()
    {
        var sale = _fixture.GetValidSale();
        var unauthorizedUser = new User { Id = 999 };

        _fixture.LoggedUserMock.Setup(x => x.Get()).ReturnsAsync(unauthorizedUser);
        _fixture.SalesReadOnlyRepositoryMock.Setup(x => x.UpdateOrRemoveGetById(unauthorizedUser, sale.Id)).ReturnsAsync((Sale)null!);

        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _fixture.DeleteUseCase.Delete(sale.Id));
        exception.Message.Should().Be(ResourceErrorMessages.SALE_NOT_FOUND);

        _fixture.SalesWriteOnlyRepositoryMock.Verify(x => x.Delete(It.IsAny<long>()), Times.Never);
        _fixture.UnitOfWorkMock.Verify(x => x.Commit(), Times.Never);
    }
}