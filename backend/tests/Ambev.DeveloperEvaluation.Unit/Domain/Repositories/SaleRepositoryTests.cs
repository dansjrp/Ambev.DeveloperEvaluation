using Xunit;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Microsoft.EntityFrameworkCore;
using Ambev.DeveloperEvaluation.ORM;
using System;
using Microsoft.EntityFrameworkCore.InMemory;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Repositories
{
    public class SaleRepositoryTests
    {
        private readonly DefaultContext _context;
        private readonly SaleRepository _repo;

        public SaleRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<DefaultContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new DefaultContext(options);
            _repo = new SaleRepository(_context);
        }

        [Fact]
        public async Task CreateAsync_Should_Add_And_Save()
        {
            // Arrange
            var sale = new Sale { Id = Guid.NewGuid(), Number = 1234 };

            // Act
            var result = await _repo.CreateAsync(sale);

            // Assert
            Assert.Equal(sale, result);
            Assert.Contains(sale, _context.Sales);
        }

        [Fact]
        public async Task DeleteAsync_Should_Remove_And_Save()
        {
            // Arrange
            var saleId = Guid.NewGuid();
            var sale = new Sale { Id = saleId };
            await _context.Sales.AddAsync(sale);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repo.DeleteAsync(saleId);

            // Assert
            Assert.True(result);
            Assert.DoesNotContain(sale, _context.Sales);
        }

        [Fact]
        public async Task GetByNumberAsync_Should_Return_Sale()
        {
            // Arrange
            var sale = new Sale { Id = Guid.NewGuid(), Number = 1234 };
            await _context.Sales.AddAsync(sale);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repo.GetByNumberAsync(1234);

            // Assert
            Assert.Equal(sale.Id, result?.Id);
        }

        [Fact]
        public async Task UpdateAsync_Should_Update_Sale()
        {
            // Arrange
            var sale = new Sale { Id = Guid.NewGuid(), Number = 1234 };
            await _context.Sales.AddAsync(sale);
            await _context.SaveChangesAsync();
            sale.Number = 4321;

            // Act
            var updated = await _repo.UpdateAsync(sale);

            // Assert
            Assert.Equal(4321, updated.Number);
        }

        [Fact]
        public async Task GetByIdAsync_WithIncludes_Should_Return_Sale()
        {
            // Arrange
            var saleId = Guid.NewGuid();
            var sale = new Sale { Id = saleId, Number = 1234 };
            await _context.Sales.AddAsync(sale);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repo.GetByIdAsync(saleId, "SaleItems");

            // Assert
            Assert.Equal(saleId, result?.Id);
        }
    }
}
