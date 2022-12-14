using Microsoft.EntityFrameworkCore;
using RozetkaFinder.Models.GoodObjects;
using RozetkaFinder.Models.User;

namespace RozetkaFinder.Repository
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<SubscribtionGood> SubscriptionsGood { get; set; }
        public DbSet<SubscriptionMarkdown> SubscriptionMarkdown { get; set; }
        public ApplicationContext()
        {
            Database.EnsureCreated();
        }
        public ApplicationContext(DbContextOptions options)
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=WIN-P3761FAKFQC;DataBase=UserDB;Trusted_Connection=True;");
        }
    }
}
