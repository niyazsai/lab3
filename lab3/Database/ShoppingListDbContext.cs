using Microsoft.EntityFrameworkCore;

namespace lab3
{
    public class ShoppingListDbContext : DbContext
    {
        public DbSet<ShoppingList> ShoppingLists { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<History> Histories { get; set; }
        public DbSet<HistoryEntry> HistoryEntries { get; set; }

        public ShoppingListDbContext(DbContextOptions<ShoppingListDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=shoppinglist.db");
        }
    }
}
