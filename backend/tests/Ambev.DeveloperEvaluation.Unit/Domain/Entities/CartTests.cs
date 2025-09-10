using Xunit;
using FluentAssertions;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Domain.Builder;
using System.Linq;

namespace Ambev.DeveloperEvaluation.Unit.Domain
{
    public class CartTests
    {
        [Fact]
        public void Should_Create_Cart_With_Valid_Data()
        {
            // Arrange
            var cart = new CartBuilder().Build();

            // Act
            // Nenhuma ação adicional necessária

            // Assert
            cart.UserId.Should().NotBeEmpty();
            cart.User.Should().NotBeNull();
            cart.Date.Should().BeBefore(System.DateTime.UtcNow.AddMinutes(1));
            cart.Products.Should().NotBeNull();
            cart.Products.Should().NotBeEmpty();
            cart.Products.All(p => p.Quantity > 0).Should().BeTrue();
        }
    }
}
