using Microsoft.EntityFrameworkCore;

namespace codingtest;

public class DBContext : DbContext
{
    public DBContext(DbContextOptions<DBContext> options): base(options)
    {
    }

    public DbSet<ProductModel> Products { get; set; } = null!;
}