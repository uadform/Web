using ItemStore.WebApi.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace ItemStore.WebApi.Contexts
{
    public class DataContext : DbContext
    {
        public DbSet<Item> Todos { get; set; }

        public DataContext(DbContextOptions<DataContext>
            options) : base(options)
        {

        }
        public DbSet<Item> Items { get; set; }
    }
}
