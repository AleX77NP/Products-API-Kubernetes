using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using products.Entities;

namespace products.Repositories
{
    public class InMemProductsRepo : IProductsRepository
    {
        private readonly List<Product> products = new()
        {
            new Product { Id = Guid.NewGuid(), Title = "Luffy Action Figure", Price = 25, CreatedDate = DateTimeOffset.UtcNow },
            new Product { Id = Guid.NewGuid(), Title = "Zoro shirt", Price = 15, CreatedDate = DateTimeOffset.UtcNow },
            new Product { Id = Guid.NewGuid(), Title = "Trafalgar Law hat", Price = 20, CreatedDate = DateTimeOffset.UtcNow },
        };

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await Task.FromResult(products);
        }

        public async Task<Product> GetProductAsync(Guid id)
        {
            return await Task.FromResult(products.Where(p => p.Id == id).SingleOrDefault());
        }

        public async Task CreateProductAsync(Product product)
        {
            products.Add(product);
            await Task.CompletedTask;
        }

        public async Task UpdateProductAsync(Product product)
        {
            var index = products.FindIndex(p => p.Id == product.Id);
            products[index] = product;
            await Task.CompletedTask;
        }

        public async Task DeleteProductAsync(Guid id)
        {
            var index = products.FindIndex(p => p.Id == id);
            products.RemoveAt(index);
            await Task.CompletedTask;
        }
    }
}
