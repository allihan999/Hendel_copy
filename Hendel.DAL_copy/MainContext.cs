using Hendel.DAL_copy.Models;
using Microsoft.EntityFrameworkCore;

namespace Hendel.DAL_copy
{
    public class MainContext : DbContext
    {
        //Korzh.EasyQuery.Linq
        public DbSet<User> Users { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Catalog> Catalogs { get; set; }
        public DbSet<Archive> Archives { get; set; }
        public DbSet<AdminInputClass> AdminInputClasses { get; set; }
        public DbSet<MyKorzina> MyKorzinas { get; set; }
        public DbSet<Watch> Watches { get; set; }
        public DbSet<Order> Orders { get; set; }

        public DbSet<BuyProducts> BuyProductsTable { get; set; }

        public MainContext(DbContextOptions<MainContext> options) : base(options) 
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasData(new List<User>()
            {
                new User{Id = 1, Role= Role.Пользователь.ToString(),  Name = "Коля",   Surname = "Тарасов", Email = "kolya@gmial.com",  Password = "111", DoublePassword = "111"},
                new User{Id = 2, Role = Role.Пользователь.ToString(), Name = "Сергей", Surname = "Малеев",  Email = "sergey@gmial.com", Password = "222", DoublePassword = "222"},

                new User{Id = 3, Role= Role.Администратор.ToString(),  Name = "Алихан", Surname = "Исхаджиев", Email = "alihan@gmial.com", Password = "111", DoublePassword = "111"},
                new User{Id = 4, Role = Role.Администратор.ToString(), Name = "Турпал", Surname = "Мамакаев",  Email = "turpal@gmial.com", Password = "222", DoublePassword = "222"}

            });

            modelBuilder.Entity<AdminInputClass>().HasData(new List<AdminInputClass>()
            {
                 new AdminInputClass{Id = 1, Role= Role.Администратор.ToString(),  Name = "Алихан", Surname = "Исхаджиев", Email = "alihan@gmial.com", Password = "111", DoublePassword = "111"},
                 new AdminInputClass{Id = 2, Role = Role.Администратор.ToString(), Name = "Турпал", Surname = "Мамакаев",  Email = "turpal@gmial.com", Password = "222", DoublePassword = "222"}
          
            });

            //modelBuilder.Entity<Catalog>().HasData(new List<Catalog>()
            //{
            //     new Catalog{Id = 1, Name = "Hublots", Surname = "Исхаджиев", Email = "alihan@gmial.com", Password = "111", DoublePassword = "111"},
            //     new Catalog{Id = 2, Name = "Hublots", Surname = "Мамакаев",  Email = "turpal@gmial.com", Password = "222", DoublePassword = "222"}
            //});
        }
    }
}