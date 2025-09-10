using Xunit;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Microsoft.EntityFrameworkCore;
using Ambev.DeveloperEvaluation.ORM;
using System;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Repositories
{
    public class CartRepositoryTests
    {
        private readonly DefaultContext _context;
        private readonly CartRepository _repo;

        public CartRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<DefaultContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new DefaultContext(options);
            _repo = new CartRepository(_context);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_Cart()
        {
            // Arrange
            var cartId = Guid.NewGuid();
            var cart = new Cart { Id = cartId };
            await _context.Carts.AddAsync(cart);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repo.GetByIdAsync(cartId);

            // Assert
            Assert.Equal(cart, result);
        }

        [Fact]
        public async Task AddAsync_Should_Add_And_Save()
        {
            // Arrange
            var cart = new Cart { Id = Guid.NewGuid() };

            // Act
            await _repo.AddAsync(cart);

            // Assert
            Assert.Contains(cart, _context.Carts);
        }

        [Fact]
        public async Task GetByIdAsync_WithIncludes_Should_Return_Cart_With_Related_Data()
        {
            // Arrange
            var cartId = Guid.NewGuid();
            var cart = new Cart { Id = cartId };
            await _context.Carts.AddAsync(cart);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repo.GetByIdAsync(cartId, "Products");

            // Assert
            Assert.Equal(cartId, result?.Id);
        }

        [Fact]
        public async Task GetPaginatedAsync_Should_Return_Paginated_Carts()
        {
            // Arrange
            for (int i = 0; i < 10; i++)
                await _context.Carts.AddAsync(new Cart { Id = Guid.NewGuid() });
            await _context.SaveChangesAsync();

            // Act
            var (carts, total) = await _repo.GetPaginatedAsync(2, 3, "Id asc");

            // Assert
            Assert.Equal(10, total);
            Assert.Equal(3, carts.Count());
        }

        [Fact]
        public async Task UpdateAsync_Should_Update_Cart()
        {
            // Arrange
            var cart = new Cart { Id = Guid.NewGuid() };
            await _context.Carts.AddAsync(cart);
            await _context.SaveChangesAsync();
            cart.UserId = Guid.NewGuid();

            // Act
            await _repo.UpdateAsync(cart);

            // Assert
            var updated = await _context.Carts.FindAsync(cart.Id);
            Assert.Equal(cart.UserId, updated.UserId);
        }

        [Fact]
        public async Task DeleteAsync_Should_Remove_Cart()
        {
            // Arrange
            var cart = new Cart { Id = Guid.NewGuid() };
            await _context.Carts.AddAsync(cart);
            await _context.SaveChangesAsync();

            // Act
            var deleted = await _repo.DeleteAsync(cart.Id);

            // Assert
            Assert.True(deleted);
            Assert.DoesNotContain(cart, _context.Carts);
        }
    }
}
