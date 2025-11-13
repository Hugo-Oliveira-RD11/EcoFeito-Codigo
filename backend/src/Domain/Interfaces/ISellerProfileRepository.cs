using Domain.Entities;

namespace Domain.Interfaces;

public interface ISellerProfileRepository : IRepository<SellerProfile>
{
  Task<List<SellerProfile>> GetByIdsAsync(IEnumerable<string> sellerIds);
}