using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

// import the Entities (database models representing structure of tables in database)
using FinalProject.Data.Entities;
using Microsoft.Identity.Client;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FinalProject.Data.Repositories;

// The Context is How EntityFramework communicates with the database
// We define DbSet properties for each table in the database
public class DatabaseContext : DbContext
{
    // authentication store
    public DbSet<User> Users { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<PastEventImage> PastEventImages { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<ForgotPassword> ForgotPasswords { get; set; }
    public DbSet<Calendar> Calendars { get; set; }
    public DbSet<County> Counties { get; set; }
    public DbSet<EventLike> EventLikes { get; set; }
   // public DbSet<ToggleLikeResult> ToggleLikeResults { get; set; }

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }

    // Configure the context with logging - remove in production
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // remove in production 
        //optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information).EnableSensitiveDataLogging();
        optionsBuilder.UseSqlite("Filename=events.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Calendar>().Property(c => c.Id).ValueGeneratedOnAdd();
    }

    //public static DbContextOptionsBuilder<DatabaseContext> OptionsBuilder => new();

    // Convenience method to recreate the database thus ensuring the new database takes 
    // account of any changes to Models or DatabaseContext. ONLY to be used in development
    public void Initialise()
    {
        Database.EnsureDeleted();
        Database.EnsureCreated();
    }

}

