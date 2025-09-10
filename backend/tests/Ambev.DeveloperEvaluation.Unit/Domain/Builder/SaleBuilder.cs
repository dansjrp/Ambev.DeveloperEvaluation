namespace Ambev.DeveloperEvaluation.Unit.Domain.Builder
{
    using Ambev.DeveloperEvaluation.Domain.Entities;
    using Bogus;
    using System.Collections.Generic;

    public class SaleBuilder
    {
        private Faker<Sale> _faker;

        public SaleBuilder()
        {
            var userBuilder = new UserBuilder();
            var productBuilder = new ProductBuilder();

            _faker = new Faker<Sale>()
                .RuleFor(s => s.Id, f => Guid.NewGuid())
                .RuleFor(s => s.Number, f => f.Random.Int(1000, 9999))
                .RuleFor(s => s.Date, f => f.Date.Past())
                .RuleFor(s => s.UserId, f => Guid.NewGuid())
                .RuleFor(s => s.Total, f => f.Random.Decimal(10, 5000))
                .RuleFor(s => s.Branch, f => f.Company.CompanyName())
                .RuleFor(s => s.SaleItems, f => new List<SaleItem> {
                    BuildSaleItem(),
                    BuildSaleItem()
                });
        }

        private SaleItem BuildSaleItem()
        {
            var faker = new Faker();
            var product = new ProductBuilder().Build();
            return new SaleItem {
                Id = Guid.NewGuid(),
                SaleId = Guid.NewGuid(),
                ProductId = product.Id,
                Quantity = faker.Random.Int(1, 10),
                Price = product.Price,
                Discounts = faker.Random.Decimal(0, 50),
                TotalPrice = product.Price * faker.Random.Int(1, 10),
                Cancelled = faker.Random.Bool(),
                Sale = null!
            };
        }

        public Sale Build()
        {
            var sale = _faker.Generate();
            foreach (var item in sale.SaleItems)
            {
                item.Sale = sale;
            }
            return sale;
        }
    }
}