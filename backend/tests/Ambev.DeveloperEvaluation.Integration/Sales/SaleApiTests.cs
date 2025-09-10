using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Ambev.DeveloperEvaluation.Integration;

namespace Ambev.DeveloperEvaluation.Integration.Sales
{
    public class SaleApiTests : IClassFixture<TestContainerFixture>
    {
        private readonly TestContainerFixture _fixture;
        public SaleApiTests(TestContainerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Should_Return_NotFound_For_Nonexistent_Sale()
        {
            var client = _fixture.Client;
            var response = await client.GetAsync("/api/sales/999999");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
