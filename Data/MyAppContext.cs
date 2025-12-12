using Microsoft.EntityFrameworkCore;
using Training_API.Models;

namespace Training_API.Data

{
    public class MyAppContext : DbContext
    {
        public MyAppContext(DbContextOptions<MyAppContext> option) : base(option)
        {
            
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Address> Addresses { get; set; }
    }
}
