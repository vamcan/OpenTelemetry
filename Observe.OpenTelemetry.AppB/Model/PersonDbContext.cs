using Microsoft.EntityFrameworkCore;

namespace Observe.OpenTelemetry.AppB.Model;

public class PersonDbContext : DbContext
{
    public DbSet<Person> People { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=.;Initial Catalog=ObserveTest;User Id=sa;Password=123;TrustServerCertificate=True");
    }
}