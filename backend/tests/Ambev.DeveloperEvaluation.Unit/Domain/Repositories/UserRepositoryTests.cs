using Xunit;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Microsoft.EntityFrameworkCore;
using Ambev.DeveloperEvaluation.ORM;
using System;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Repositories
{
    public class UserRepositoryTests
    {
        private readonly DefaultContext _context;
        private readonly UserRepository _repo;

        public UserRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<DefaultContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new DefaultContext(options);
            _repo = new UserRepository(_context);
        }

        [Fact]
        public async Task CreateAsync_Should_Add_User_And_Save()
        {
            // Arrange
            var user = new User { Id = Guid.NewGuid(), Email = "test@x.com" };

            // Act
            var result = await _repo.CreateAsync(user);

            // Assert
            Assert.Equal(user, result);
            Assert.Contains(user, _context.Users);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_User()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User { Id = userId, Email = "test@x.com" };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repo.GetByIdAsync(userId);

            // Assert
            Assert.Equal(user, result);
        }

        [Fact]
        public async Task GetByEmailAsync_Should_Return_User()
        {
            // Arrange
            var user = new User { Id = Guid.NewGuid(), Email = "test@x.com" };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repo.GetByEmailAsync("test@x.com");

            // Assert
            Assert.Equal(user.Id, result?.Id);
        }

        [Fact]
        public async Task DeleteAsync_Should_Remove_User()
        {
            // Arrange
            var user = new User { Id = Guid.NewGuid(), Email = "test@x.com" };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            // Act
            var deleted = await _repo.DeleteAsync(user.Id);

            // Assert
            Assert.True(deleted);
            Assert.DoesNotContain(user, _context.Users);
        }

        [Fact]
        public async Task GetPaginatedAsync_Should_Return_Paginated_Users()
        {
            // Arrange
            for (int i = 0; i < 10; i++)
                await _context.Users.AddAsync(new User { Id = Guid.NewGuid(), Email = $"user{i}@x.com" });
            await _context.SaveChangesAsync();

            // Act
            var (users, total) = await _repo.GetPaginatedAsync(2, 3, "email asc");

            // Assert
            Assert.Equal(10, total);
            Assert.Equal(3, users.Count());
        }
    }
}
