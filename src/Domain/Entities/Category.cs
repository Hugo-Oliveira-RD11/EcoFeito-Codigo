namespace Domain.Entities;

public class Category : BaseEntity
{
    public string Name { get; private set; }
    public string? Description { get; private set; }

    // Navigation properties
    public ICollection<Product> Products { get; private set; } = new List<Product>();

    private Category() { }

    public Category(string name, string? description = null)
    {
        Name = name;
        Description = description;
    }

    public void UpdateCategory(string name, string? description)
    {
        Name = name;
        Description = description;
        UpdateTimestamp();
    }
}
