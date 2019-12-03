using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TravelAgency.Models;
using Microsoft.EntityFrameworkCore.SqlServer;
using TravelAgency.Migrations;

namespace TravelAgency.DAL
{
    class TravelAgencyDbContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<Employee> Employees { get; set; }

        public TravelAgencyDbContext()
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=DESKTOP-CAQ3GC5\SQLEXPRESS;Initial Catalog=TravelAgency;Integrated Security=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Trip>()
            .Property(e => e.Services)
            .HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries));
            modelBuilder.Entity<ClientTrip>()
                .HasKey(ct => new {ct.ClientId, ct.TripId});
            modelBuilder.Entity<ClientTrip>()
                .HasOne(ct => ct.Client)
                .WithMany(c => c.ClientTrips)
                .HasForeignKey(ct => ct.ClientId);
            modelBuilder.Entity<ClientTrip>()
                .HasOne(ct => ct.Trip)
                .WithMany(t => t.ClientTrips)
                .HasForeignKey(ct => ct.TripId);

        }
    }
}
