using Domain.Entities;

namespace Domain.Interfaces;

public interface IProductRepository : IRepository<Product>
{
    Task<IEnumerable<Product>> GetBySellerAsync(string sellerProfileId);
    Task<IEnumerable<Product>> SearchAsync(string searchTerm, string? categoryId = null);
    Task<IEnumerable<Product>> GetActiveProductsAsync();
}
