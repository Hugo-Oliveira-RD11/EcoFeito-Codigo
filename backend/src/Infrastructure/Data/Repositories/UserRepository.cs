using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using MongoDB.Driver;

namespace Infrastructure.Data.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(MongoDbContext context) : base(context.Users)
    {
    }

    public async Task<User> GetByEmailAsync(string email)
    {
        return await _collection.Find(x => x.Email == email).FirstOrDefaultAsync();
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _collection.Find(x => x.Email == email).AnyAsync();
    }
}
