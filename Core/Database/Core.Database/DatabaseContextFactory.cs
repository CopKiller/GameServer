using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Core.Database;

/// <summary>
/// This is used to create a new instance of the Database Context in design time.
/// This is used by the EF Core tools to create migrations.
/// This is used by the EF Core tools to drop and recreate the database.
/// </summary>
public class DatabaseContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
{
    public DatabaseContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .AddUserSecrets(Assembly.GetExecutingAssembly())
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new DatabaseContext(optionsBuilder.Options);
    }
}