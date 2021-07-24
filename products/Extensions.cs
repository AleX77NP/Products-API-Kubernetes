using System;
using products.Dtos;
using products.Entities;

namespace products
{
    public static class Extensions
    {
        public static ProductDto AsDto(this Product product)
        {
            return new ProductDto {
                Id = product.Id,
                Title = product.Title,
                Price = product.Price,
                CreatedDate = product.CreatedDate
            };
        }
    }
}
