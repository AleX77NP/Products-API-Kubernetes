using System;
namespace products.Entities
{
    public record Product
    {
        public Guid Id { get; init; }
        public string Title { get; init; }
        public decimal Price { get; init; }
        public DateTimeOffset CreatedDate { get; init; }
    }
}
