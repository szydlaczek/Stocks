using CompanyApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace CompanyApi.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Stock> Stock { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>().HasKey(pk => pk.CompanyId);

            var navigation = modelBuilder.Entity<Company>()
                .Metadata.FindNavigation(nameof(Company.Stocks));

            navigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            modelBuilder.Entity<Company>()
                .Property(n => n.Name)
                .HasMaxLength(128)
                .IsRequired();

            modelBuilder.Entity<Company>()
                .Property(n => n.Isin)
                .HasMaxLength(10)
                .IsRequired();

            modelBuilder.Entity<Stock>()
                .HasKey(pk => pk.StockId);

            modelBuilder.Entity<Stock>()
                .HasOne(c => c.Company)
                .WithMany(s => s.Stocks)
                .HasForeignKey(pk => pk.CompanyId);
        }

        private IEnumerable<object> GetCompanies()
        {
            List<object> companies = new List<object>();
            for (int i = 1; i <= 20; i++)
            {
                companies.Add(new { CompanyId = i, Name = $"Company {i}", Isin = $"Isin {i}" });
            }
            return companies;
        }

        private IEnumerable<object> GetStocks()
        {
            List<object> stocks = new List<object>();
            int stockId = 1;
            for (int i = 1; i <= 20; i++)
            {
                for (int j = 1; j <= 20000; j++)
                {
                    stocks.Add(new
                    {
                        StockId = stockId,
                        Value = RandomNumberBetween(5.00, 99.99),
                        Time = GetRandomDate(DateTime.Now.AddDays(-5000), DateTime.Now),
                        CompanyId = i
                    });
                    stockId++;
                }
            }

            return stocks;
        }

        private static readonly Random random = new Random();

        private static double RandomNumberBetween(double minValue, double maxValue)
        {
            var next = random.NextDouble();

            return minValue + (next * (maxValue - minValue));
        }

        public static DateTime GetRandomDate(DateTime from, DateTime to)
        {
            var range = to - from;

            var randTimeSpan = new TimeSpan((long)(random.NextDouble() * range.Ticks));

            return from + randTimeSpan;
        }
    }
}