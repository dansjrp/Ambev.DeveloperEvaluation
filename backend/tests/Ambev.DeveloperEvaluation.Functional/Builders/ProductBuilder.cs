using Bogus;

namespace Ambev.DeveloperEvaluation.Functional.Builders
{
    public class ProductBuilder
    {
        private readonly Faker _faker = new();
        private string? _title = "Produto Teste";
        private string? _description = "Descrição do produto teste";
        private string? _category = "categoria";
        private double _price = 10.0;
        private string? _image = "https://via.placeholder.com/150";
        private object? _rating = new { Rate = 0.0m, Count = 0 };

        public ProductBuilder WithTitle(string title) { _title = title; return this; }
        public ProductBuilder WithDescription(string description) { _description = description; return this; }
        public ProductBuilder WithCategory(string category) { _category = category; return this; }
        public ProductBuilder WithPrice(double price) { _price = price; return this; }
        public ProductBuilder WithImage(string image) { _image = image; return this; }
        public ProductBuilder WithRating(object rating) { _rating = rating; return this; }

        public object Build() => new {
            Title = _title,
            Description = _description,
            Category = _category,
            Price = _price,
            Image = _image,
            Rating = _rating
        };
    }
}
