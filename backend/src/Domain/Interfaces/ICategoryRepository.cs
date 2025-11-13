using Domain.Entities;

namespace Domain.Interfaces;

public interface ICategoryRepository :  IRepository<Category>
{
  Task<Category> GetByNameAsync(string requestName);
  Task<List<Category>> GetByIdsAsync(IEnumerable<string> categoryIds);
}