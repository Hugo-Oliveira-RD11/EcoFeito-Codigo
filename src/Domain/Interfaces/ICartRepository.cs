using Domain.Entities;

namespace Domain.Interfaces;

public interface ICartRepository : IRepository<Cart>
{
    Task<Cart> GetByCustomerAsync(string customerProfileId);
}
