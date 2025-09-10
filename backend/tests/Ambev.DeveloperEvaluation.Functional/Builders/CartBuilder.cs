namespace Ambev.DeveloperEvaluation.Functional.Builders
{
    public class CartBuilder
    {
        private object? _userId;
        private object[] _items = new object[0];

        public CartBuilder WithUserId(object userId) { _userId = userId; return this; }
        public CartBuilder WithItems(object[] items) { _items = items; return this; }
        public CartBuilder AddItem(object item)
        {
            var itemsList = _items.ToList();
            itemsList.Add(item);
            _items = itemsList.ToArray();
            return this;
        }

        public object Build() => new {
            UserId = _userId,
            Items = _items
        };
    }
}
