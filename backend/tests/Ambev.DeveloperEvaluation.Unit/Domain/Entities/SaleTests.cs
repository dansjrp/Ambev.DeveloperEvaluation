using Xunit;
using FluentAssertions;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Domain.Builder;
using System.Linq;

namespace Ambev.DeveloperEvaluation.Unit.Domain
{
    public class SaleTests
    {
        [Fact]
        public void Should_Create_Sale_With_Valid_Data()
        {
            // Arrange
            var sale = new SaleBuilder().Build();

            // Act
            // Nenhuma ação adicional necessária

            // Assert
            sale.Number.Should().BeGreaterThan(0);
            sale.Date.Should().BeBefore(System.DateTime.UtcNow.AddMinutes(1));
            sale.UserId.Should().NotBeEmpty();
            sale.Total.Should().BeGreaterThan(0);
            sale.Branch.Should().NotBeNullOrEmpty();
            sale.SaleItems.Should().NotBeNull();
            sale.SaleItems.Should().NotBeEmpty();
            sale.SaleItems.All(i => i.Quantity > 0 && i.Price > 0).Should().BeTrue();
        }
    }
}
