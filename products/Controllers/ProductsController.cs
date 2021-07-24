using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using products.Dtos;
using products.Entities;
using products.Repositories;
using Microsoft.Extensions.Logging;

namespace products.Controllers
{
    [ApiController]
    [Route("products")]
    public class ProductsController: ControllerBase
    {
        private readonly IProductsRepository repository;
        private readonly ILogger<ProductsController> logger;

        public ProductsController(IProductsRepository repository, ILogger<ProductsController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        [HttpGet]
        public  async Task<IEnumerable<ProductDto>> GetProductsAsync()
        {
            var products = (await repository.GetProductsAsync()).Select(product => product.AsDto());
            
            logger.LogInformation($"{DateTime.UtcNow.ToString("hh:mm:ss")}: Retrieved {products.Count()} products ");

            return products;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProductAsync(Guid id)
        {
            var product = await repository.GetProductAsync(id);
            if(product is null)
            {
                return NotFound();
            }
            return product.AsDto();
        }

        [HttpPost]
        public async Task<ActionResult<ProductDto>> CreateProductAsync(CreateProductDto productDto)
        {
            Product p = new Product()
            {
                Id = Guid.NewGuid(),
                Title = productDto.Title,
                Price = productDto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };

            await repository.CreateProductAsync(p);

            return CreatedAtAction(nameof(GetProductAsync), new { id = p.Id }, p.AsDto());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProductAsync(Guid id, UpdateProductDto productDto)
        {
            var existingP = await repository.GetProductAsync(id);
            if (existingP is null)
            {
                return NotFound();
            }

            Product updatedP = existingP with
            {
                Title = productDto.Title,
                Price = productDto.Price
            };

            await repository.UpdateProductAsync(updatedP);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProductAsync(Guid id)
        {
            var existingP = await repository.GetProductAsync(id);
            if (existingP is null)
            {
                return NotFound();
            }

            await repository.DeleteProductAsync(id);
            return NoContent();
        }
    }
}
