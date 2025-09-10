using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Ambev.DeveloperEvaluation.Integration;
using Ambev.DeveloperEvaluation.WebApi.Features.Auth.AuthenticateUserFeature;

namespace Ambev.DeveloperEvaluation.Integration.Auth
{
    public class AuthApiTests : IClassFixture<TestContainerFixture>
    {
        private readonly TestContainerFixture _fixture;
        public AuthApiTests(TestContainerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Should_Return_Unauthorized_For_Invalid_Credentials()
        {
            var client = _fixture.Client;
            var request = new AuthenticateUserRequest
            {
                Email = "invalid@email.com",
                Password = "wrongpassword"
            };
            var response = await client.PostAsJsonAsync("/api/auth", request);
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}
