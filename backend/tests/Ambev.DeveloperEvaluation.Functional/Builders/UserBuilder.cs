using Bogus;

namespace Ambev.DeveloperEvaluation.Functional.Builders
{
    public class UserBuilder
    {
        private readonly Faker _faker = new();
        private string? _email;
        private string? _password = "Test@1234!";
        private string? _phone = "+5511912345678";
        private int _role = 1;
        private int _status = 1;
        private string? _username;
        private object? _address;
        private object? _name;

        public UserBuilder()
        {
            _username = (_faker.Internet.UserName() + _faker.Random.AlphaNumeric(8)).Substring(0, 8);
            _email = _faker.Internet.Email();
            _address = new {
                City = "TestCity",
                Number = "123",
                Street = "TestStreet",
                Zipcode = "12345-678",
                Geolocation = new { Lat = "-23.5505", Long = "-46.6333" }
            };
            _name = new {
                Firstname = _faker.Name.FirstName(),
                Lastname = _faker.Name.LastName()
            };
        }

        public UserBuilder WithEmail(string email) { _email = email; return this; }
        public UserBuilder WithRole(int role) { _role = role; return this; }
        public UserBuilder WithStatus(int status) { _status = status; return this; }
        public UserBuilder WithUsername(string username) { _username = username; return this; }
        public UserBuilder WithAddress(object address) { _address = address; return this; }
        public UserBuilder WithName(object name) { _name = name; return this; }
        public UserBuilder WithPassword(string password) { _password = password; return this; }

        public object Build() => new {
            Email = _email,
            Password = _password,
            Phone = _phone,
            Role = _role,
            Status = _status,
            Username = _username,
            Address = _address,
            Name = _name
        };
    }
}
