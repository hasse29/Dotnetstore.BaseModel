using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Dotnetstore.BaseModel.Tests.Configurations;

internal static class ConfigurationTestHelper
{
    public static IEntityType BuildEntityType<TEntity>(IEntityTypeConfiguration<TEntity> configuration)
        where TEntity : class
    {
        var options = new DbContextOptionsBuilder<TestDbContext<TEntity>>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new TestDbContext<TEntity>(options, configuration);
        return context.Model.FindEntityType(typeof(TEntity))
               ?? throw new InvalidOperationException($"{typeof(TEntity).Name} was not added to the model.");
    }

    public static IProperty GetProperty(this IEntityType entityType, string name) =>
        entityType.FindProperty(name)
        ?? throw new InvalidOperationException($"Property {name} was not found on {entityType.Name}.");

    public static bool HasIndexOn(this IEntityType entityType, string propertyName) =>
        entityType.GetIndexes().Any(x => x.Properties.Count == 1 && x.Properties[0].Name == propertyName);

    private sealed class TestDbContext<TEntity>(
        DbContextOptions<TestDbContext<TEntity>> options,
        IEntityTypeConfiguration<TEntity> configuration) : DbContext(options)
        where TEntity : class
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder) =>
            modelBuilder.ApplyConfiguration(configuration);
    }
}
