using Xunit;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Microsoft.EntityFrameworkCore;
using Ambev.DeveloperEvaluation.ORM;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Repositories
{
    public class ProductRepositoryTests
    {
        private readonly DefaultContext _context;
        private readonly ProductRepository _repo;

        public ProductRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<DefaultContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new DefaultContext(options);
            _repo = new ProductRepository(_context);
        }

        [Fact]
        public async Task GetAllAsync_Should_Return_All_Products()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = Guid.NewGuid(), Title = "P1" },
                new Product { Id = Guid.NewGuid(), Title = "P2" }
            };
            await _context.Products.AddRangeAsync(products);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repo.GetAllAsync(System.Threading.CancellationToken.None);

            // Assert
            Assert.Equal(products.Count, result.Count());
            Assert.All(products, p => Assert.Contains(result, r => r.Id == p.Id));
        }

        [Fact]
        public async Task CreateAsync_Should_Add_And_Save()
        {
            // Arrange
            var product = new Product { Id = Guid.NewGuid(), Title = "P3" };

            // Act
            var result = await _repo.CreateAsync(product, System.Threading.CancellationToken.None);

            // Assert
            Assert.Equal(product, result);
            Assert.Contains(product, _context.Products);
        }

        [Fact]
        public async Task GetExistingProductIdsAsync_Should_Return_Existing_Ids()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = Guid.NewGuid(), Title = "P1" },
                new Product { Id = Guid.NewGuid(), Title = "P2" }
            };
            await _context.Products.AddRangeAsync(products);
            await _context.SaveChangesAsync();
            var ids = new List<Guid> { products[0].Id, Guid.NewGuid() };

            // Act
            var result = await _repo.GetExistingProductIdsAsync(ids, System.Threading.CancellationToken.None);

            // Assert
            Assert.Single(result);
            Assert.Equal(products[0].Id, result[0]);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_Product()
        {
            // Arrange
            var product = new Product { Id = Guid.NewGuid(), Title = "P1" };
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repo.GetByIdAsync(product.Id, System.Threading.CancellationToken.None);

            // Assert
            Assert.Equal(product.Id, result?.Id);
        }

        [Fact]
        public async Task GetPaginatedAsync_Should_Return_Paginated_Products()
        {
            // Arrange
            for (int i = 0; i < 10; i++)
                await _context.Products.AddAsync(new Product { Id = Guid.NewGuid(), Title = $"P{i}" });
            await _context.SaveChangesAsync();

            // Act
            var (products, total) = await _repo.GetPaginatedAsync(2, 3, "title asc", System.Threading.CancellationToken.None);

            // Assert
            Assert.Equal(10, total);
            Assert.Equal(3, products.Count());
        }

        [Fact]
        public async Task GetPaginatedByCategoryAsync_Should_Return_Filtered_Products()
        {
            // Arrange
            await _context.Products.AddAsync(new Product { Id = Guid.NewGuid(), Title = "P1", Category = "cat1" });
            await _context.Products.AddAsync(new Product { Id = Guid.NewGuid(), Title = "P2", Category = "cat2" });
            await _context.SaveChangesAsync();

            // Act
            var (products, total) = await _repo.GetPaginatedByCategoryAsync("cat1", 1, 10, null, System.Threading.CancellationToken.None);

            // Assert
            Assert.Single(products);
            Assert.Equal(1, total);
        }

        [Fact]
        public async Task GetAllCategoriesAsync_Should_Return_All_Categories()
        {
            // Arrange
            await _context.Products.AddAsync(new Product { Id = Guid.NewGuid(), Title = "P1", Category = "cat1" });
            await _context.Products.AddAsync(new Product { Id = Guid.NewGuid(), Title = "P2", Category = "cat2" });
            await _context.SaveChangesAsync();

            // Act
            var categories = await _repo.GetAllCategoriesAsync(System.Threading.CancellationToken.None);

            // Assert
            Assert.Contains("cat1", categories);
            Assert.Contains("cat2", categories);
        }

        [Fact]
        public async Task UpdateAsync_Should_Update_Product()
        {
            // Arrange
            var product = new Product { Id = Guid.NewGuid(), Title = "P1" };
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            product.Title = "Updated";

            // Act
            var updated = await _repo.UpdateAsync(product, System.Threading.CancellationToken.None);

            // Assert
            Assert.Equal("Updated", updated.Title);
        }

        [Fact]
        public async Task DeleteAsync_Should_Remove_Product()
        {
            // Arrange
            var product = new Product { Id = Guid.NewGuid(), Title = "P1" };
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            // Act
            var deleted = await _repo.DeleteAsync(product.Id, System.Threading.CancellationToken.None);

            // Assert
            Assert.True(deleted);
            Assert.DoesNotContain(product, _context.Products);
        }
    }
}
