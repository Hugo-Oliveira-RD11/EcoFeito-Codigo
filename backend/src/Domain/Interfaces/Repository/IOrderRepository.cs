using Domain.Entities;

namespace Domain.Interfaces;

public interface IOrderRepository : IRepository<Order>
{
    Task<IEnumerable<Order>> GetByCustomerAsync(string customerProfileId);
    Task<IEnumerable<Order>> GetBySellerAsync(string sellerProfileId);
}
