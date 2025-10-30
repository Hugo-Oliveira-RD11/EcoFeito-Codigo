using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using MongoDB.Driver;

namespace Infrastructure.Data.Repositories;

public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
{
    public CategoryRepository(MongoDbContext context) : base(context.Categories)
    {
    }

    public async Task<Category> GetByNameAsync(string name)
    {
        return await _collection.Find(x => x.Name == name).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Category>> GetByIdsAsync(IEnumerable<string> ids)
    {
        return await _collection.Find(x => ids.Contains(x.Id)).ToListAsync();
    }
}
