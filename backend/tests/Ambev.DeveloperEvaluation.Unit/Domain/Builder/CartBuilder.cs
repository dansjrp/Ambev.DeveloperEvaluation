namespace Ambev.DeveloperEvaluation.Unit.Domain.Builder
{
    using Ambev.DeveloperEvaluation.Domain.Entities;
    using Bogus;

    public class CartBuilder
    {
        private Faker<Cart> _faker;

        public CartBuilder()
        {
            var userBuilder = new UserBuilder();
            var productBuilder = new ProductBuilder();

            _faker = new Faker<Cart>()
                .RuleFor(c => c.Id, f => Guid.NewGuid())
                .RuleFor(c => c.UserId, f => Guid.NewGuid())
                .RuleFor(c => c.User, f => userBuilder.Build())
                .RuleFor(c => c.Date, f => f.Date.Past())
                .RuleFor(c => c.Products, f => new List<CartProduct> {
                    new CartProduct {
                        ProductId = Guid.NewGuid(),
                        Product = productBuilder.Build(),
                        Quantity = f.Random.Int(1, 10)
                    },
                    new CartProduct {
                        ProductId = Guid.NewGuid(),
                        Product = productBuilder.Build(),
                        Quantity = f.Random.Int(1, 10)
                    }
                });
        }

        public Cart Build()
        {
            return _faker.Generate();
        }
    }
}