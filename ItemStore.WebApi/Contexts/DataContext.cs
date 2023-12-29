using ItemStore.WebApi.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace ItemStore.WebApi.Contexts
{
    public class DataContext : DbContext
    {
        public DbSet<Shop> Shops { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Purchase> Purchases { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
    }
}
