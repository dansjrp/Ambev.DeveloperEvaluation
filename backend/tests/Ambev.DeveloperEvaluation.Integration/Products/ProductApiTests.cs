using System.Net;
using System.Net.Http.Json;
using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Ambev.DeveloperEvaluation.WebApi;

namespace Ambev.DeveloperEvaluation.Integration.Products
{
    public class ProductApiTests : IClassFixture<TestContainerFixture>
    {
        private readonly HttpClient _client;

        public ProductApiTests(TestContainerFixture fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async Task Should_Create_And_Get_Product()
        {
            // Arrange
            var productRequest = new {
                Title = "Integration Product",
                Price = 99.99M,
                Description = "Test product",
                Category = "Test",
                Image = "img.png",
                Rating = new { Rate = 5, Count = 10 }
            };
            var createResponse = await _client.PostAsJsonAsync("/api/products", productRequest);

            // Assert
            createResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            var productJson = await createResponse.Content.ReadFromJsonAsync<System.Text.Json.JsonElement>();
            if (!productJson.TryGetProperty("data", out var dataElement))
            {
                throw new Xunit.Sdk.XunitException($"JSON de resposta não contém 'data': {productJson}");
            }
            var productId = dataElement.GetProperty("id").GetGuid();

            // Act
            var getResponse = await _client.GetAsync($"/api/products/{productId}");

            // Assert
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var getProductJson = await getResponse.Content.ReadFromJsonAsync<System.Text.Json.JsonElement>();
            getProductJson.GetProperty("id").GetGuid().Should().Be(productId);
        }
    }
}
