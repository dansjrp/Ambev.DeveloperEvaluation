using Xunit;
using FluentAssertions;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Domain;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Unit.Domain.Builder;

namespace Ambev.DeveloperEvaluation.Unit.Domain
{
    public class UserTests
    {
        [Fact]
        public void Should_Create_User_With_Valid_Data()
        {
            var user = new UserBuilder().Build();
            var validation = user.Validate();
            validation.IsValid.Should().BeTrue();
        }

        [Theory]
        [InlineData("")]
        [InlineData("invalid-email")]
        public void Should_Fail_Validation_When_Email_Is_Invalid(string email)
        {
            var user = new UserBuilder().Build();
            user.Email = email;
            var validation = user.Validate();
            validation.IsValid.Should().BeFalse();
            validation.Errors.Should().NotBeEmpty();
        }

        [Theory]
        [InlineData("")]
        [InlineData("123")]
        public void Should_Fail_Validation_When_Username_Is_Invalid(string username)
        {
            var user = new UserBuilder().Build();
            user.Username = username;
            var validation = user.Validate();
            validation.IsValid.Should().BeFalse();
            validation.Errors.Should().NotBeEmpty();
        }
    }
}
