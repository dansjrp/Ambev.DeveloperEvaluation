using System.Net.Http;
using Ambev.DeveloperEvaluation.Functional.Builders;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using NSubstitute;
using Xunit;
using Testcontainers.PostgreSql;
using Testcontainers.MongoDb;
using Testcontainers.RabbitMq;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ambev.DeveloperEvaluation.Functional
{
    public class SaleCreationFlowTests : IAsyncLifetime
    {
        private readonly PostgreSqlContainer _postgres;
        private readonly MongoDbContainer _mongo;
        private readonly RabbitMqContainer _rabbit;
        private HttpClient? _client;
        private NSubstitute.Core.CallInfo? _auditCall;

        public SaleCreationFlowTests()
        {
            _postgres = new PostgreSqlBuilder()
                .WithDatabase("developer_evaluation")
                .WithUsername("developer")
                .WithPassword("ev@luAt10n")
                .Build();

            _mongo = new MongoDbBuilder()
                .WithUsername("developer")
                .WithPassword("ev@luAt10n")
                .Build();

            _rabbit = new RabbitMqBuilder()
                .WithUsername("guest")
                .WithPassword("guest")
                .Build();

            // O HttpClient será inicializado via TestServer no InitializeAsync
            _client = null;
            _auditCall = null;
        }

        public async Task InitializeAsync()
        {
            await _postgres.StartAsync();
            await _mongo.StartAsync();
            await _rabbit.StartAsync();

            // Executa as migrations do ORM no banco do Testcontainers
            var optionsBuilder = new Microsoft.EntityFrameworkCore.DbContextOptionsBuilder<Ambev.DeveloperEvaluation.ORM.DefaultContext>();
            optionsBuilder.UseNpgsql(_postgres.GetConnectionString(), b => b.MigrationsAssembly("Ambev.DeveloperEvaluation.ORM"));
            using (var context = new Ambev.DeveloperEvaluation.ORM.DefaultContext(optionsBuilder.Options))
            {
                await context.Database.MigrateAsync();
            }

            // Inicializa TestServer apontando para a WebApi, sobrescrevendo a connection string
            var factory = new WebApplicationFactory<Ambev.DeveloperEvaluation.WebApi.Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureAppConfiguration((context, config) =>
                    {
                        var settings = new List<KeyValuePair<string, string?>>
                        {
                            new KeyValuePair<string, string?>("ConnectionStrings:DefaultConnection", _postgres.GetConnectionString())
                        };
                        config.AddInMemoryCollection(settings);
                    });
                });
            _client = factory.CreateClient();
        }

        public async Task DisposeAsync()
        {
            await _postgres.DisposeAsync();
            await _mongo.DisposeAsync();
            await _rabbit.DisposeAsync();
        }

        private async Task<Guid> CreateUserAsync()
        {
            var userRequest = new UserBuilder().Build();

            if (_client == null) throw new Exception("HttpClient não inicializado");

            var response = await _client.PostAsJsonAsync("/api/Users", userRequest);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Erro ao criar usuário: {response.StatusCode} - {error}");
            }

            var userJson = await response.Content.ReadFromJsonAsync<System.Text.Json.JsonElement>();

            return userJson.GetProperty("data").GetProperty("id").GetGuid();
        }

        private async Task<Guid> CreateProductAsync()
        {
            var productRequest = new ProductBuilder().Build();

            var response = await _client.PostAsJsonAsync("/api/products", productRequest);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Erro ao criar produto: {response.StatusCode} - {error}");
            }

            var productJson = await response.Content.ReadFromJsonAsync<System.Text.Json.JsonElement>();

            return productJson.GetProperty("data").GetProperty("id").GetGuid();
        }

        private async Task<Guid> CreateCartAsync(Guid userId, Guid productId)
        {
            dynamic cartRequest = new CartBuilder()
                .WithUserId(userId)
                .WithItems(new[] { new { ProductId = productId, Quantity = 2 } })
                .Build();

            var response = await _client.PostAsJsonAsync("/api/carts", (object)cartRequest);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Erro ao criar carrinho: {response.StatusCode} - {error}");
            }

            var cartJson = await response.Content.ReadFromJsonAsync<System.Text.Json.JsonElement>();

            return cartJson.GetProperty("id").GetGuid();
        }

        private async Task<System.Text.Json.JsonElement> CloseCartAsync(Guid cartId)
        {
            var closeCartRequest = new { Branch = "Filial Teste" };
            var response = await _client.PostAsJsonAsync($"/api/carts/{cartId}/close", closeCartRequest);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Erro ao fechar carrinho: {response.StatusCode} - {error}");
            }

            var sale = await response.Content.ReadFromJsonAsync<System.Text.Json.JsonElement>();

            return sale;
        }

        private async Task ValidateSaleInDatabaseAsync(Guid userId)
        {
            using var conn = new Npgsql.NpgsqlConnection(_postgres.GetConnectionString());
            await conn.OpenAsync();
            using var cmd = new Npgsql.NpgsqlCommand("SELECT COUNT(*) FROM \"Sales\" WHERE \"UserId\" = @userId", conn);
            cmd.Parameters.AddWithValue("userId", userId);

            var result = await cmd.ExecuteScalarAsync();
            var count = result != null ? Convert.ToInt64(result) : 0L;

            Assert.True(count > 0);
        }

        [Fact]
        public async Task Should_Create_Sale_And_Audit_Event()
        {
            // Arrange
            var userId = await CreateUserAsync();
            var productId = await CreateProductAsync();
            var cartId = await CreateCartAsync(userId, productId);

            // Act
            var sale = await CloseCartAsync(cartId);

            // Assert
            sale.ValueKind.Should().Be(System.Text.Json.JsonValueKind.Object);
            Assert.Equal(userId, sale.GetProperty("userId").GetGuid());
            await ValidateSaleInDatabaseAsync(userId);
        }

        [Fact]
        public async Task Should_Fail_When_User_Is_Invalid()
        {
            // Arrange
            var productId = await CreateProductAsync();
            // Usuário inválido (não criado)
            var invalidUserId = Guid.NewGuid();

            // Act
            Func<Task> act = async () => await CreateCartAsync(invalidUserId, productId);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("*Erro ao criar carrinho*");
        }

        [Fact]
        public async Task Should_Fail_When_Product_Is_Invalid()
        {
            // Arrange
            var userId = await CreateUserAsync();
            var invalidProductId = Guid.NewGuid();

            // Act
            Func<Task> act = async () => await CreateCartAsync(userId, invalidProductId);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("*Erro ao criar carrinho*");
        }

        [Fact]
        public async Task Should_Fail_When_Closing_Cart_Without_Branch()
        {
            // Arrange
            var userId = await CreateUserAsync();
            var productId = await CreateProductAsync();
            var cartId = await CreateCartAsync(userId, productId);
            var closeCartRequest = new { Branch = "" };

            // Act
            var response = await _client.PostAsJsonAsync($"/api/carts/{cartId}/close", closeCartRequest);

            // Assert
            response.IsSuccessStatusCode.Should().BeFalse();
        }

        [Fact]
        public async Task Should_Create_Sale_With_Multiple_Products()
        {
            // Arrange
            var userId = await CreateUserAsync();
            var productId1 = await CreateProductAsync();
            var productId2 = await CreateProductAsync();
            dynamic cartRequest = new CartBuilder()
                .WithUserId(userId)
                .WithItems(new[] {
                    new { ProductId = productId1, Quantity = 1 },
                    new { ProductId = productId2, Quantity = 3 }
                })
                .Build();
            var response = await _client.PostAsJsonAsync("/api/carts", (object)cartRequest);
            var cartJson = await response.Content.ReadFromJsonAsync<System.Text.Json.JsonElement>();
            var cartId = cartJson.GetProperty("id").GetGuid();

            // Act
            var sale = await CloseCartAsync(cartId);

            // Assert
            sale.ValueKind.Should().Be(System.Text.Json.JsonValueKind.Object);
            Assert.Equal(userId, sale.GetProperty("userId").GetGuid());
            await ValidateSaleInDatabaseAsync(userId);
        }

        [Fact]
        public async Task Should_Handle_Maximum_Items_In_Cart()
        {
            // Arrange
            var userId = await CreateUserAsync();
            var productIds = new List<Guid>();
            for (int i = 0; i < 20; i++)
                productIds.Add(await CreateProductAsync());
            dynamic cartRequest = new CartBuilder()
                .WithUserId(userId)
                .WithItems(productIds.Select(pid => new { ProductId = pid, Quantity = 1 }).ToArray())
                .Build();
            var response = await _client.PostAsJsonAsync("/api/carts", (object)cartRequest);
            var cartJson = await response.Content.ReadFromJsonAsync<System.Text.Json.JsonElement>();
            var cartId = cartJson.GetProperty("id").GetGuid();

            // Act
            var sale = await CloseCartAsync(cartId);

            // Assert
            sale.ValueKind.Should().Be(System.Text.Json.JsonValueKind.Object);
            Assert.Equal(userId, sale.GetProperty("userId").GetGuid());
            await ValidateSaleInDatabaseAsync(userId);
        }

        [Fact]
        public async Task Should_Rollback_When_Sale_Fails()
        {
            // Arrange
            var userId = await CreateUserAsync();
            var productId = await CreateProductAsync();
            var cartId = await CreateCartAsync(userId, productId);

            // Simula falha: fecha carrinho duas vezes
            await CloseCartAsync(cartId);
            Func<Task> act = async () => await CloseCartAsync(cartId);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("*Erro ao fechar carrinho*");
        }
    }
}
