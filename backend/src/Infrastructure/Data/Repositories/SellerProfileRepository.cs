using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using MongoDB.Driver;

namespace Infrastructure.Data.Repositories;

public class SellerProfileRepository : BaseRepository<SellerProfile>, ISellerProfileRepository
{
    public SellerProfileRepository(MongoDbContext context) : base(context.SellerProfiles)
    {
    }

    public async Task<SellerProfile> GetByUserIdAsync(string userId)
    {
        return await _collection.Find(x => x.UserId == userId).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<SellerProfile>> GetByIdsAsync(IEnumerable<string> ids)
    {
        return await _collection.Find(x => ids.Contains(x.Id)).ToListAsync();
    }
}
