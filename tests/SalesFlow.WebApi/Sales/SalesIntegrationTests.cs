using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using SalesFlow.Communication.Request.Sales;
using SalesFlow.Communication.Response.Sales;
using System.Text.Json;

namespace SalesFlow.WebApi.Tests.Sales;
public class SalesIntegrationTests : IntegrationTestsBase
{
    private readonly RequestSaleCreateJson _validSaleRequest;

    public SalesIntegrationTests(WebApplicationFactory<Program> factory) : base(factory)
    {
        _validSaleRequest = new RequestSaleCreateJson
        {
            CustomerName = "Test Customer",
            Items = new List<RequestSaleItemCreateJson>
            {
                new()
                {
                    ProductName = "Test Product",
                    Quantity = 2,
                    UnitPrice = 10.0m
                }
            }
        };
    }

    [Fact]
    public async Task CreateSale_WhenValidData_ShouldReturnCreated()
    {
        // Arrange
        await AuthenticateClient();

        // Act
        var response = await _client.PostAsync("/api/sales", GetStringContent(_validSaleRequest));
        var content = await response.Content.ReadAsStringAsync();
        var saleResponse = JsonSerializer.Deserialize<ResponseSaleJson>(content, _jsonOptions);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        saleResponse.Should().NotBeNull();
        saleResponse!.CustomerName.Should().Be(_validSaleRequest.CustomerName);
        saleResponse.TotalAmount.Should().Be(20.0m); // 2 * 10.0
    }

    [Fact]
    public async Task GetAll_WhenHasSales_ShouldReturnSalesList()
    {
        // Arrange
        await AuthenticateClient();
        await _client.PostAsync("/api/sales", GetStringContent(_validSaleRequest));

        // Act
        var response = await _client.GetAsync("/api/sales");
        var content = await response.Content.ReadAsStringAsync();
        var salesResponse = JsonSerializer.Deserialize<ResponseSalesJson>(content, _jsonOptions);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        salesResponse.Should().NotBeNull();
        salesResponse!.Sales.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetById_WhenSaleExists_ShouldReturnSale()
    {
        // Arrange
        await AuthenticateClient();
        var createResponse = await _client.PostAsync("/api/sales", GetStringContent(_validSaleRequest));
        var createContent = await createResponse.Content.ReadAsStringAsync();
        var createdSale = JsonSerializer.Deserialize<ResponseSaleJson>(createContent, _jsonOptions);

        // Act
        var response = await _client.GetAsync($"/api/sales/{createdSale!.Id}");
        var content = await response.Content.ReadAsStringAsync();
        var saleResponse = JsonSerializer.Deserialize<ResponseSaleJson>(content, _jsonOptions);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        saleResponse.Should().NotBeNull();
        saleResponse!.Id.Should().Be(createdSale.Id);
    }

    [Fact]
    public async Task UpdateSale_WhenValidData_ShouldReturnNoContent()
    {
        // Arrange
        await AuthenticateClient();
        var createResponse = await _client.PostAsync("/api/sales", GetStringContent(_validSaleRequest));
        var createContent = await createResponse.Content.ReadAsStringAsync();
        var createdSale = JsonSerializer.Deserialize<ResponseSaleJson>(createContent, _jsonOptions);

        var updateRequest = new RequestSaleUpdateJson
        {
            CustomerName = "Updated Customer"
        };

        // Act
        var response = await _client.PutAsync($"/api/sales/{createdSale!.Id}", GetStringContent(updateRequest));

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

        // Verify update
        var getResponse = await _client.GetAsync($"/api/sales/{createdSale.Id}");
        var getContent = await getResponse.Content.ReadAsStringAsync();
        var updatedSale = JsonSerializer.Deserialize<ResponseSaleJson>(getContent, _jsonOptions);

        updatedSale.Should().NotBeNull();
        updatedSale!.CustomerName.Should().Be("Updated Customer");
    }

    [Fact]
    public async Task UpdateSaleItem_WhenValidData_ShouldReturnNoContent()
    {
        // Arrange
        await AuthenticateClient();
        var createResponse = await _client.PostAsync("/api/sales", GetStringContent(_validSaleRequest));
        var createContent = await createResponse.Content.ReadAsStringAsync();
        var createdSale = JsonSerializer.Deserialize<ResponseSaleJson>(createContent, _jsonOptions);

        var updateRequest = new RequestSaleItemUpdateJson
        {
            Id = createdSale!.Items.First().Id,
            ProductName = "Updated Product",
            Quantity = 3,
            UnitPrice = 15.0m
        };

        // Act
        var response = await _client.PutAsync(
            $"/api/sales/{createdSale.Id}/items/{updateRequest.Id}",
            GetStringContent(updateRequest));

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

        // Verify update
        var getResponse = await _client.GetAsync($"/api/sales/{createdSale.Id}");
        var getContent = await getResponse.Content.ReadAsStringAsync();
        var updatedSale = JsonSerializer.Deserialize<ResponseSaleJson>(getContent, _jsonOptions);

        updatedSale.Should().NotBeNull();
        var updatedItem = updatedSale!.Items.First();
        updatedItem.ProductName.Should().Be("Updated Product");
        updatedItem.Quantity.Should().Be(3);
        updatedItem.UnitPrice.Should().Be(15.0m);
    }
}