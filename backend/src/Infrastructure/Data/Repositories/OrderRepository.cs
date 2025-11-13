using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using MongoDB.Driver;

namespace Infrastructure.Data.Repositories;

public class OrderRepository : BaseRepository<Order>, IOrderRepository
{
    public OrderRepository(MongoDbContext context) : base(context.Orders)
    {
    }

    public async Task<IEnumerable<Order>> GetByCustomerAsync(string customerProfileId)
    {
        return await _collection.Find(x => x.CustomerProfileId == customerProfileId).ToListAsync();
    }

    public async Task<IEnumerable<Order>> GetBySellerAsync(string sellerProfileId)
    {
        // Buscar pedidos que contenham produtos do vendedor
        var orders = await _collection
            .Aggregate()
            .Lookup<Order, OrderItem, Order>(
                foreignCollection: context.OrderItems,
                localField: x => x.Id,
                foreignField: x => x.OrderId,
                @as: x => x.Items
            )
            .Match(x => x.Items.Any(item => item.Product.SellerProfileId == sellerProfileId))
            .ToListAsync();

        return orders;
    }

    public async Task<Order> GetByIdWithItemsAsync(string orderId)
    {
        return await _collection
            .Aggregate()
            .Match(x => x.Id == orderId)
            .Lookup<Order, OrderItem, Order>(
                foreignCollection: context.OrderItems,
                localField: x => x.Id,
                foreignField: x => x.OrderId,
                @as: x => x.Items
            )
            .FirstOrDefaultAsync();
    }
}
