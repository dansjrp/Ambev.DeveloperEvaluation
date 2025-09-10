using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Ambev.DeveloperEvaluation.Integration;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.CreateCart;

namespace Ambev.DeveloperEvaluation.Integration.Carts
{
    public class CartApiTests : IClassFixture<TestContainerFixture>
    {
        private readonly TestContainerFixture _fixture;
        public CartApiTests(TestContainerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Should_Create_And_Delete_Cart()
        {
            var client = _fixture.Client;
            // Cria usuário
            var userRequest = new
            {
                Username = "testuser_cart",
                Password = "Test@1234",
                Phone = "+5511999999999",
                Email = $"cart_{Guid.NewGuid()}@test.com",
                Status = 1, // UserStatus.Active
                Role = 1,   // UserRole.User
                Name = new {
                    Firstname = "Test",
                    Lastname = "User"
                },
                Address = new {
                    City = "SP",
                    Street = "Rua Teste",
                    Number = 123,
                    Zipcode = "01234567",
                    Geolocation = new {
                        Lat = "-23.5",
                        Long = "-46.6"
                    }
                }
            };
            var userResponse = await client.PostAsJsonAsync("/api/users", userRequest);
            if (!userResponse.IsSuccessStatusCode)
            {
                var errorBody = await userResponse.Content.ReadAsStringAsync();
                Assert.Fail($"Erro ao criar usuário: Status {(int)userResponse.StatusCode} - {userResponse.StatusCode}\nCorpo: {errorBody}");
            }
            var userJson = await userResponse.Content.ReadAsStringAsync();
            var userElement = System.Text.Json.JsonDocument.Parse(userJson).RootElement;
            string? userId = null;
            if (userElement.TryGetProperty("data", out var userDataProp) && userDataProp.TryGetProperty("id", out var userIdProp))
                userId = userIdProp.GetString();
            else if (userElement.TryGetProperty("id", out var userIdProp2))
                userId = userIdProp2.GetString();

            // Cria produto
            var productRequest = new
            {
                Title = "Produto Teste Cart",
                Price = 10.0,
                Description = "Produto para teste de carrinho",
                Category = "Teste",
                Image = "https://example.com/imagem.jpg",
                Rating = new { Rate = 5, Count = 1 }
            };
            var productResponse = await client.PostAsJsonAsync("/api/products", productRequest);
            if (!productResponse.IsSuccessStatusCode)
            {
                var errorBody = await productResponse.Content.ReadAsStringAsync();
                Assert.Fail($"Erro ao criar produto: Status {(int)productResponse.StatusCode} - {productResponse.StatusCode}\nCorpo: {errorBody}");
            }
            var productJson = await productResponse.Content.ReadAsStringAsync();
            var productElement = System.Text.Json.JsonDocument.Parse(productJson).RootElement;
            string? productId = null;
            if (productElement.TryGetProperty("data", out var productDataProp) && productDataProp.TryGetProperty("id", out var productIdProp))
                productId = productIdProp.GetString();
            else if (productElement.TryGetProperty("id", out var productIdProp2))
                productId = productIdProp2.GetString();

            // Cria carrinho
            var createRequest = new
            {
                UserId = userId,
                Items = new[] {
                    new { ProductId = productId, Quantity = 2 }
                }
            };
            var createResponse = await client.PostAsJsonAsync("/api/carts", createRequest);
            if (createResponse.StatusCode != HttpStatusCode.Created)
            {
                var errorBody = await createResponse.Content.ReadAsStringAsync();
                Assert.Fail($"Erro ao criar carrinho: Status {(int)createResponse.StatusCode} - {createResponse.StatusCode}\nCorpo: {errorBody}");
            }
            var cartJson = await createResponse.Content.ReadAsStringAsync();
            var cartElement = System.Text.Json.JsonDocument.Parse(cartJson).RootElement;
            string? cartId = null;
            if (cartElement.TryGetProperty("data", out var cartDataProp) && cartDataProp.TryGetProperty("id", out var cartIdProp))
                cartId = cartIdProp.GetString();
            else if (cartElement.TryGetProperty("id", out var cartIdProp2))
                cartId = cartIdProp2.GetString();

            cartId.Should().NotBeNull();
            // Delete
            if (cartId != null)
            {
                var deleteResponse = await client.DeleteAsync($"/api/carts/{cartId}");
                deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            }
        }
    }
}
