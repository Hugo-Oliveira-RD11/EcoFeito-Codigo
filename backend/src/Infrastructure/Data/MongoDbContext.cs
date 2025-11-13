// Infrastructure/Data/MongoDbContext.cs
using Domain.Entities;
using Infrastructure.Config;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Infrastructure.Data;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IOptions<MongoDbSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        _database = client.GetDatabase(settings.Value.DatabaseName);

        // Criar índices
        CreateIndexes();
    }

    public IMongoCollection<User> Users => _database.GetCollection<User>("Users");
    public IMongoCollection<SellerProfile> SellerProfiles => _database.GetCollection<SellerProfile>("SellerProfiles");
    public IMongoCollection<CustomerProfile> CustomerProfiles => _database.GetCollection<CustomerProfile>("CustomerProfiles");
    public IMongoCollection<Store> Stores => _database.GetCollection<Store>("Stores");
    public IMongoCollection<Product> Products => _database.GetCollection<Product>("Products");
    public IMongoCollection<ProductImage> ProductImages => _database.GetCollection<ProductImage>("ProductImages");
    public IMongoCollection<Category> Categories => _database.GetCollection<Category>("Categories");
    public IMongoCollection<Cart> Carts => _database.GetCollection<Cart>("Carts");
    public IMongoCollection<CartItem> CartItems => _database.GetCollection<CartItem>("CartItems");
    public IMongoCollection<Order> Orders => _database.GetCollection<Order>("Orders");
    public IMongoCollection<OrderItem> OrderItems => _database.GetCollection<OrderItem>("OrderItems");

    private void CreateIndexes()
    {
        // Índices para Users
        Users.Indexes.CreateOne(
            new CreateIndexModel<User>(Builders<User>.IndexKeys.Ascending(u => u.Email),
            new CreateIndexOptions { Unique = true }
        );

        // Índices para Products
        Products.Indexes.CreateOne(
            new CreateIndexModel<Product>(Builders<Product>.IndexKeys.Ascending(p => p.SellerProfileId))
        );
        Products.Indexes.CreateOne(
            new CreateIndexModel<Product>(Builders<Product>.IndexKeys.Ascending(p => p.CategoryId))
        );
        Products.Indexes.CreateOne(
            new CreateIndexModel<Product>(Builders<Product>.IndexKeys.Text(p => p.Name).Text(p => p.Description))
        );

        // Índices para Orders
        Orders.Indexes.CreateOne(
            new CreateIndexModel<Order>(Builders<Order>.IndexKeys.Ascending(o => o.CustomerProfileId))
        );

        // Índices para Carts
        Carts.Indexes.CreateOne(
            new CreateIndexModel<Cart>(Builders<Cart>.IndexKeys.Ascending(c => c.CustomerProfileId),
            new CreateIndexOptions { Unique = true }
        );
    }
}
