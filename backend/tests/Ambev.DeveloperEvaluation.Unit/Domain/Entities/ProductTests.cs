using Xunit;
using FluentAssertions;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Domain.Builder;

namespace Ambev.DeveloperEvaluation.Unit.Domain
{
    public class ProductTests
    {
        [Fact]
        public void Should_Create_Product_With_Valid_Data()
        {
            // Arrange
            var product = new ProductBuilder().Build();

            // Act
            // Nenhuma ação adicional necessária

            // Assert
            product.Title.Should().NotBeNullOrEmpty();
            product.Price.Should().BeGreaterThan(0);
            product.Description.Should().NotBeNullOrEmpty();
            product.Category.Should().NotBeNullOrEmpty();
            product.Image.Should().NotBeNullOrEmpty();
            product.Rating.Should().NotBeNull();
            product.Rating.Rate.Should().BeInRange(1, 5);
            product.Rating.Count.Should().BeGreaterOrEqualTo(0);
        }
    }
}
