using System;
using Xunit;
using Moq;
using System.Threading.Tasks;
using products.Repositories;
using products.Controllers;
using products.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using products.Dtos;

namespace ProductsService.Tests
{
    public class ProductsControllerTests
    {
        private readonly Mock<IProductsRepository> repositoryStub = new Mock<IProductsRepository>();
        private readonly Mock<ILogger<ProductsController>> loggerStub = new Mock<ILogger<ProductsController>>();

        private readonly Random rand = new();

        [Fact]
        public async Task GetProductAsync_WithUnexistingProduct_NotFound()
        {
            // Arrange 
            repositoryStub.Setup(repo => repo.GetProductAsync(It.IsAny<Guid>())).ReturnsAsync((Product)null);

            var controller = new ProductsController(repositoryStub.Object, loggerStub.Object);

            // Act
            var result = await controller.GetProductAsync(Guid.NewGuid());

            // Assert
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetProductAsync_WithExistingProduct_ExpectedItem()
        {
            // Arrange 
            var expectedItem = CreateRandomProduct();

            repositoryStub.Setup(repo => repo.GetProductAsync(It.IsAny<Guid>())).ReturnsAsync(expectedItem);

            var controller = new ProductsController(repositoryStub.Object, loggerStub.Object);

            // Act
            var result = await controller.GetProductAsync(Guid.NewGuid());

            // Assert
            result.Value.Should().BeEquivalentTo(expectedItem, options => options.ComparingByMembers<Product>());
        }

        [Fact]
        public async Task GetProductsAsync_WithExistingProducts_ReturnsAllItems()
        {
            // Arrange 
            var expectedItems = new[]{CreateRandomProduct(), CreateRandomProduct(), CreateRandomProduct()};

            repositoryStub.Setup(repo => repo.GetProductsAsync()).ReturnsAsync(expectedItems);

            var controller = new ProductsController(repositoryStub.Object, loggerStub.Object);

            // Act
            var actualProducts = await controller.GetProductsAsync();

            // Assert
            actualProducts.Should().BeEquivalentTo(expectedItems, options => options.ComparingByMembers<Product>());
        }

        [Fact]
        public async Task CreateProductAsync_WithProductToCreate_ReturnsCreatedProduct()
        {
            // Arrange 
            var productToCreate = new CreateProductDto{
                Title = Guid.NewGuid().ToString(),
                Price = rand.Next(1000)
            };

            var controller = new ProductsController(repositoryStub.Object, loggerStub.Object);

            // Act
            var result = await controller.CreateProductAsync(productToCreate);

            // Assert
            var createdItem = (result.Result as CreatedAtActionResult).Value as ProductDto;
            createdItem.Should().BeEquivalentTo(productToCreate, options => options.ComparingByMembers<ProductDto>().ExcludingMissingMembers());
            createdItem.Id.Should().NotBeEmpty();
            createdItem.CreatedDate.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
            
        }

        [Fact]
        public async Task UpdateProductAsync_WithExistingProduct_ReturnsNoContent()
        {
            // Arrange 
            Product existingProduct = CreateRandomProduct();
            repositoryStub.Setup(repo => repo.GetProductAsync(It.IsAny<Guid>())).ReturnsAsync(existingProduct);

            var productId = existingProduct.Id;
            var productToUpdate = new UpdateProductDto() {
                Title = Guid.NewGuid().ToString(),
                Price = existingProduct.Price + 10
            };

            var controller = new ProductsController(repositoryStub.Object, loggerStub.Object);

            // Act
            var result = await controller.UpdateProductAsync(productId, productToUpdate);

            // Assert
            result.Should().BeOfType<NoContentResult>();
           
        }

        [Fact]
        public async Task DeleteProductAsync_WithExistingProduct_ReturnsNoContent()
        {
            // Arrange 
            Product existingProduct = CreateRandomProduct();
            repositoryStub.Setup(repo => repo.GetProductAsync(It.IsAny<Guid>())).ReturnsAsync(existingProduct);

            var controller = new ProductsController(repositoryStub.Object, loggerStub.Object);

            // Act
            var result = await controller.DeleteProductAsync(existingProduct.Id);

            // Assert
            result.Should().BeOfType<NoContentResult>();
           
        }

        private Product CreateRandomProduct() {
            return new() {
                Id = Guid.NewGuid(),
                Title = Guid.NewGuid().ToString(),
                Price = rand.Next(1000),
                CreatedDate = DateTimeOffset.UtcNow
            };
        }
    }
}
