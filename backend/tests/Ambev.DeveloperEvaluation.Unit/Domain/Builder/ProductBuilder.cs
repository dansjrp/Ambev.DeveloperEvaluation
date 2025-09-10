namespace Ambev.DeveloperEvaluation.Unit.Domain.Builder
{
    using Ambev.DeveloperEvaluation.Domain.Entities;
    using Bogus;

    public class ProductBuilder
    {
        private Faker<Product> _faker;

        public ProductBuilder()
        {
            _faker = new Faker<Product>()
                .RuleFor(p => p.Id, f => Guid.NewGuid())
                .RuleFor(p => p.Title, f => f.Commerce.ProductName())
                .RuleFor(p => p.Price, f => f.Random.Decimal(1, 1000))
                .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
                .RuleFor(p => p.Category, f => f.Commerce.Categories(1)[0])
                .RuleFor(p => p.Image, f => f.Image.PicsumUrl())
                .RuleFor(p => p.Rating, f => new Rating {
                    Rate = f.Random.Decimal(1, 5),
                    Count = f.Random.Int(0, 1000)
                });
        }

        public Product Build()
        {
            return _faker.Generate();
        }
    }
}