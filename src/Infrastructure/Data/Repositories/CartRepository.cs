using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using MongoDB.Driver;

namespace Infrastructure.Data.Repositories;

public class CartRepository : BaseRepository<Cart>, ICartRepository
{
    public CartRepository(MongoDbContext context) : base(context.Carts)
    {
    }

    public async Task<Cart> GetByCustomerAsync(string customerProfileId)
    {
        return await _collection.Find(x => x.CustomerProfileId == customerProfileId).FirstOrDefaultAsync();
    }

    public async Task<Cart> GetByCustomerWithItemsAsync(string customerProfileId)
    {
        return await _collection
            .Aggregate()
            .Match(x => x.CustomerProfileId == customerProfileId)
            .Lookup<Cart, CartItem, Cart>(
                foreignCollection: context.CartItems,
                localField: x => x.Id,
                foreignField: x => x.CartId,
                @as: x => x.Items
            )
            .FirstOrDefaultAsync();
    }
}
