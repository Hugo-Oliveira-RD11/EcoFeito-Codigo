using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using MongoDB.Driver;

namespace Infrastructure.Data.Repositories;

public class CustomerProfileRepository : BaseRepository<CustomerProfile>, ICustomerProfileRepository
{
    public CustomerProfileRepository(MongoDbContext context) : base(context.CustomerProfiles)
    {
    }

    public async Task<CustomerProfile> GetByUserIdAsync(string userId)
    {
        return await _collection.Find(x => x.UserId == userId).FirstOrDefaultAsync();
    }
}
