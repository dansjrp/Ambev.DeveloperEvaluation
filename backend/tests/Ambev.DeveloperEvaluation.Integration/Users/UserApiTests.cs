using System.Net;
using System.Net.Http.Json;
using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Ambev.DeveloperEvaluation.WebApi;

namespace Ambev.DeveloperEvaluation.Integration.Users
{
    public class UserApiTests : IClassFixture<TestContainerFixture>
    {
        private readonly HttpClient _client;

        public UserApiTests(TestContainerFixture fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async Task Should_Create_And_Get_User()
        {
            // Arrange
            var userRequest = new {
                Username = "integrationuser",
                Password = "Test@123",
                Email = "integration@x.com",
                Phone = "11999999999",
                Status = 1, // Active
                Role = 1 // Customer
            };
            var createResponse = await _client.PostAsJsonAsync("/api/users", userRequest);

            // Assert
            createResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            var userJson = await createResponse.Content.ReadFromJsonAsync<System.Text.Json.JsonElement>();
            Guid userId;
            if (userJson.TryGetProperty("data", out var dataElement))
            {
                userId = dataElement.GetProperty("id").GetGuid();
            }
            else if (userJson.TryGetProperty("id", out var idElement))
            {
                userId = idElement.GetGuid();
            }
            else
            {
                throw new Xunit.Sdk.XunitException($"Resposta inesperada do endpoint: {userJson}");
            }

            // Act
            var getResponse = await _client.GetAsync($"/api/users/{userId}");

            // Assert
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var getUserJson = await getResponse.Content.ReadFromJsonAsync<System.Text.Json.JsonElement>();
            Guid getUserId;
            if (getUserJson.TryGetProperty("data", out var getDataElement))
            {
                getUserId = getDataElement.GetProperty("id").GetGuid();
            }
            else if (getUserJson.TryGetProperty("id", out var getIdElement))
            {
                getUserId = getIdElement.GetGuid();
            }
            else
            {
                throw new Xunit.Sdk.XunitException($"Resposta inesperada do endpoint: {getUserJson}");
            }
            getUserId.Should().Be(userId);
        }
    }
}
