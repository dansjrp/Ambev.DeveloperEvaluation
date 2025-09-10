namespace Ambev.DeveloperEvaluation.Unit.Domain.Builder
{
    using Ambev.DeveloperEvaluation.Domain.Entities;
    using Ambev.DeveloperEvaluation.Domain.Enums;
    using Bogus;

    public class UserBuilder
    {
        private Faker<User> _faker;

        public UserBuilder()
        {
            _faker = new Faker<User>()
                .RuleFor(u => u.Id, f => Guid.NewGuid())
                .RuleFor(u => u.Username, f => f.Internet.UserName())
                .RuleFor(u => u.Name, f => new Name { Firstname = f.Name.FirstName(), Lastname = f.Name.LastName() })
                .RuleFor(u => u.Email, f => f.Internet.Email())
                .RuleFor(u => u.Phone, f => f.Phone.PhoneNumber("(##) #####-####"))
                .RuleFor(u => u.Address, f => new Address {
                    City = f.Address.City(),
                    Street = f.Address.StreetName(),
                    Number = f.Random.Int(1, 9999),
                    Zipcode = f.Address.ZipCode(),
                    Geolocation = new Geolocation {
                        Lat = f.Address.Latitude().ToString(),
                        Long = f.Address.Longitude().ToString()
                    }
                })
                .RuleFor(u => u.Password, f => GenerateValidPassword(f))
                .RuleFor(u => u.Role, f => f.PickRandom<UserRole>(UserRole.Admin, UserRole.Manager, UserRole.Customer))
                .RuleFor(u => u.Status, f => f.PickRandom<UserStatus>(UserStatus.Active, UserStatus.Inactive, UserStatus.Suspended));
        }

        private string GenerateValidPassword(Faker f)
        {
            // At least 8 chars, 1 uppercase, 1 lowercase, 1 number, 1 special
            var upper = f.Random.Char('A', 'Z');
            var lower = f.Random.Char('a', 'z');
            var digit = f.Random.Int(0, 9).ToString();
            var special = f.Random.ArrayElement(new[] { "@", "#", "$", "%", "!", "&" });
            var rest = f.Random.String2(4, "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789");
            var password = string.Concat(upper, lower, digit, special, rest);
            return password;
        }

        public User Build()
        {
            return _faker.Generate();
        }
    }
}