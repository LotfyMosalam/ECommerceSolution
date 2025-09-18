using ECommerce.Infrastructure.Repositories;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using ECommerce.Core.Entities;
using Xunit;

public class ProductRepositoryTests
{
    [Fact]
    public async Task AddAsync_ShouldAddProduct()
    {
        var options = new DbContextOptionsBuilder<ECommerceDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDB")
            .Options;

        using var context = new ECommerceDbContext(options);
        var repo = new ProductRepository(context);

        var product = new Product { Name = "Test", Category = "Cat", ProductCode = "P01", Price = 10, MinimumQuantity = 1, DiscountRate = 0.1M };

        await repo.AddAsync(product);
        var products = await repo.GetAllAsync();

        Assert.Single(products);
        Assert.Equal("Test", products.First().Name);
    }
}
