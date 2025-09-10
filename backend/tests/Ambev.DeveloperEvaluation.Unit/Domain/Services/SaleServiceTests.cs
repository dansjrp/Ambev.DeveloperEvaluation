using Xunit;
using FluentAssertions;
using System;
using Ambev.DeveloperEvaluation.ORM.Service;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Services
{
    public class SaleServiceTests
    {
        private readonly SaleService _service;

        public SaleServiceTests()
        {
            _service = new SaleService();
        }

        [Theory]
        [InlineData(1, 100, 0)]
        [InlineData(3, 50, 0)]
        [InlineData(4, 10, 4)] // 10% de desconto
        [InlineData(9, 10, 9)] // 10% de desconto
        [InlineData(10, 10, 20)] // 20% de desconto
        [InlineData(20, 10, 40)] // 20% de desconto
        public void CalculateDiscount_Should_Return_Correct_Discount(int quantity, decimal unitPrice, decimal expectedDiscount)
        {
            // Arrange
            // (já feito via parâmetros)

            // Act
            var discount = _service.CalculateDiscount(quantity, unitPrice);

            // Assert
            discount.Should().Be(expectedDiscount);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void CalculateDiscount_Should_Throw_ArgumentException_When_Quantity_Less_Than_One(int quantity)
        {
            // Arrange
            // (já feito via parâmetro)

            // Act
            Action act = () => _service.CalculateDiscount(quantity, 10);

            // Assert
            act.Should().Throw<ArgumentException>().WithMessage("Quantity must be greater than zero.");
        }

        [Theory]
        [InlineData(21)]
        [InlineData(100)]
        public void CalculateDiscount_Should_Throw_InvalidOperationException_When_Quantity_Greater_Than_20(int quantity)
        {
            // Arrange
            // (já feito via parâmetro)

            // Act
            Action act = () => _service.CalculateDiscount(quantity, 10);

            // Assert
            act.Should().Throw<InvalidOperationException>().WithMessage("Cannot sell more than 20 identical items.");
        }
    }
}
