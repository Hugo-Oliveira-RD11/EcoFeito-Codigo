using Domain.Entities;

namespace Domain.Interfaces;

public interface IStoreRepository : IRepository<Store>
{
    Task<Store> GetBySellerAsync(string sellerProfileId);
}
