using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using MongoDB.Driver;

namespace Infrastructure.Data.Repositories;

public class ProductRepository : BaseRepository<Product>, IProductRepository
{
    public ProductRepository(MongoDbContext context) : base(context.Products)
    {
    }

    public async Task<IEnumerable<Product>> GetBySellerAsync(string sellerProfileId)
    {
        return await _collection.Find(x => x.SellerProfileId == sellerProfileId && x.IsActive).ToListAsync();
    }

    public async Task<IEnumerable<Product>> SearchAsync(string searchTerm, string? categoryId = null)
    {
        var builder = Builders<Product>.Filter;
        var filter = builder.Eq(x => x.IsActive, true);

        if (!string.IsNullOrEmpty(searchTerm))
        {
            var searchFilter = builder.Or(
                builder.Regex(x => x.Name, new MongoDB.Bson.BsonRegularExpression(searchTerm, "i")),
                builder.Regex(x => x.Description, new MongoDB.Bson.BsonRegularExpression(searchTerm, "i"))
            );
            filter = builder.And(filter, searchFilter);
        }

        if (!string.IsNullOrEmpty(categoryId))
        {
            var categoryFilter = builder.Eq(x => x.CategoryId, categoryId);
            filter = builder.And(filter, categoryFilter);
        }

        return await _collection.Find(filter).ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetActiveProductsAsync()
    {
        return await _collection.Find(x => x.IsActive).ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetByIdsAsync(IEnumerable<string> ids)
    {
        return await _collection.Find(x => ids.Contains(x.Id) && x.IsActive).ToListAsync();
    }
}
