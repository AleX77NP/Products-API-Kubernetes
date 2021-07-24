using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using products.Entities;

namespace products.Repositories
{
    public class MongoDbProductsRepository : IProductsRepository
    {
        private const string databaseName = "shop";
        private const string collectionName = "products";
        private readonly IMongoCollection<Product> productsCollection;
        private readonly FilterDefinitionBuilder<Product> filterBuilder = Builders<Product>.Filter;

        public MongoDbProductsRepository(IMongoClient mongoClient)
        {
            IMongoDatabase database = mongoClient.GetDatabase(databaseName);
            productsCollection = database.GetCollection<Product>("products");
        }
        public async Task CreateProductAsync(Product product)
        {
            await productsCollection.InsertOneAsync(product);
        }

        public async Task DeleteProductAsync(Guid id)
        {
            var filter = filterBuilder.Eq(product => product.Id, id);
            await productsCollection.DeleteOneAsync(filter);
        }

        public async Task<Product> GetProductAsync(Guid id)
        {
            var filter = filterBuilder.Eq(product => product.Id, id);
            return await productsCollection.Find(filter).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await productsCollection.Find(new BsonDocument()).ToListAsync();
        }

        public async Task UpdateProductAsync(Product product)
        {
            var filter = filterBuilder.Eq(existingProduct => existingProduct.Id, product.Id);
            await productsCollection.ReplaceOneAsync(filter, product);
        }
    }
}
