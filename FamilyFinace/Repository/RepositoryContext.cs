using FamilyFinace.Interfaces;
using FamilyFinace.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

            modelBuilder.Entity<Store>().HasKey(s => s.Id);
            modelBuilder.Entity<Store>().Property(s => s.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Store>().HasIndex(s => s.Name).IsUnique();
            modelBuilder.Entity<Store>().Property(s => s.IsRemoved).HasDefaultValue(false);

            modelBuilder.Entity<Cost>().HasKey(s => s.Id);
            modelBuilder.Entity<Cost>().Property(s => s.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Cost>().HasIndex(c => c.CostCategoryId);
            modelBuilder.Entity<Cost>().HasIndex(c => c.PayTypeId);
            modelBuilder.Entity<Cost>().HasIndex(c => c.StoreId);
            modelBuilder.Entity<Cost>().Property(s => s.Date).HasColumnType("date");
            modelBuilder.Entity<Cost>().Property(s => s.Date).HasDefaultValue(DateTime.Now);

            modelBuilder.Entity<Income>().HasKey(s => s.Id);
            modelBuilder.Entity<Income>().Property(s => s.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Income>().HasIndex(c => c.PayTypeId);
            modelBuilder.Entity<Income>().Property(s => s.Date).HasColumnType("date");
            modelBuilder.Entity<Income>().Property(s => s.Date).HasDefaultValue(DateTime.Now);
        }
    }
}
