using FamilyFinace.Interfaces;
using FamilyFinace.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace FamilyFinace.Repository
{
    public class RepositoryContext : DbContext
    {
        public RepositoryContext(DbContextOptions<RepositoryContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<CostCategory> CostCategory { get; set; }
        public DbSet<PayType> PayType { get; set; }
        public DbSet<Store> Store { get; set; }
        public DbSet<Cost> Cost { get; set; }
        public DbSet<Income> Income { get; set; }
        public DbSet<Plan> Plan { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CostCategory>().HasKey(c => c.Id);
            modelBuilder.Entity<CostCategory>().Property(c => c.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<CostCategory>().HasIndex(c => new { c.CategoryName, c.SubCategoryName }).IsUnique();
            modelBuilder.Entity<CostCategory>().Property(c => c.IsRemoved).HasDefaultValue(false);

            modelBuilder.Entity<PayType>().HasKey(p => p.Id);
            modelBuilder.Entity<PayType>().Property(p => p.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<PayType>().HasIndex(p => p.Name).IsUnique();
            modelBuilder.Entity<PayType>().Property(p => p.IsRemoved).HasDefaultValue(false);
            modelBuilder.Entity<PayType>().HasData(new PayType { Id = 1, Name = "Наличность", IsRemoved = false });
            modelBuilder.Entity<PayType>().HasData(new PayType { Id = 2, Name = "Карта", IsRemoved = false });
            modelBuilder.Entity<PayType>().HasData(new PayType { Id = 3, Name = "Payme", IsRemoved = false });
            modelBuilder.Entity<PayType>().HasData(new PayType { Id = 4, Name = "Click", IsRemoved = false });
            modelBuilder.Entity<PayType>().HasData(new PayType { Id = 5, Name = "Apelsin", IsRemoved = false });

            modelBuilder.Entity<Store>().HasKey(s => s.Id);
            modelBuilder.Entity<Store>().Property(s => s.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Store>().HasIndex(s => s.Name).IsUnique();
            modelBuilder.Entity<Store>().Property(s => s.IsRemoved).HasDefaultValue(false);
            modelBuilder.Entity<Store>().HasData(
                new Models.Store { Id = 1, Name = "Artyemka plyus", IsRemoved = false },
                new Models.Store { Id = 2, Name = "Korzinka.uz", IsRemoved = false },
                new Models.Store { Id = 3, Name = "Makro", IsRemoved = false },
                new Models.Store { Id = 4, Name = "Green Apple", IsRemoved = false },
                new Models.Store { Id = 5, Name = "Magnit", IsRemoved = false },
                new Models.Store { Id = 6, Name = "Tegen", IsRemoved = false },
                new Models.Store { Id = 7, Name = "Full Market", IsRemoved = false },
                new Models.Store { Id = 8, Name = "Аптека", IsRemoved = false });


            modelBuilder.Entity<Cost>().HasKey(c => c.Id);
            modelBuilder.Entity<Cost>().Property(c => c.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Cost>().HasIndex(c => c.CostCategoryId);
            modelBuilder.Entity<Cost>().HasIndex(c => c.PayTypeId);
            modelBuilder.Entity<Cost>().HasIndex(c => c.StoreId);
            modelBuilder.Entity<Cost>().Property(c => c.Date).HasColumnType("date");
            modelBuilder.Entity<Cost>().Property(c => c.Date).HasDefaultValue(DateTime.Now);
            modelBuilder.Entity<Cost>().Property(c => c.Amount).HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Income>().HasKey(i => i.Id);
            modelBuilder.Entity<Income>().Property(i => i.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Income>().HasIndex(i => i.PayTypeId);
            modelBuilder.Entity<Income>().Property(i => i.Date).HasColumnType("date");
            modelBuilder.Entity<Income>().Property(i => i.Date).HasDefaultValue(DateTime.Now);
            modelBuilder.Entity<Income>().Property(i => i.Amount).HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Plan>().HasKey(p => p.Id);
            modelBuilder.Entity<Plan>().Property(p => p.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Plan>().HasIndex(p => p.Month);
            modelBuilder.Entity<Plan>().HasIndex(p => p.Year);
            modelBuilder.Entity<Plan>().HasIndex(p => p.CategoryId);
            modelBuilder.Entity<Plan>().Property(p => p.Amount).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Plan>().HasIndex(p => new { p.Month, p.Year, p.CategoryId }).IsUnique();
        }
    }
}
