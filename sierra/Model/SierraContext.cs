using Microsoft.EntityFrameworkCore;
using sierra.Model.Entities;

public class SierraContext : DbContext
{
    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<Order> Orders { get; set; }
    public virtual DbSet<Customer> Customers { get; set; }

    public string _connectionString { get; }

    public SierraContext(string connectionString)
    {
      _connectionString = connectionString;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
      => options.UseSqlServer(_connectionString);
}