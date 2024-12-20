using Microsoft.EntityFrameworkCore;

// import the Entities (database models representing structure of tables in database)
using FinalProject.Data.Entities;

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
    public DbSet<Registration> Registrations { get; set; }

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }

    // Configure the context with logging - remove in production
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // remove in production 
        optionsBuilder.UseSqlite("Filename=events.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Calendar>().Property(c => c.Id).ValueGeneratedOnAdd();
        modelBuilder.Entity<Registration>()
            .HasOne(r => r.Event)
            .WithMany(e => e.Registrations)
            .HasForeignKey(r => r.EventId);
        modelBuilder.Entity<Registration>()
            .HasOne(r => r.User)
            .WithMany(u => u.Registrations)
            .HasForeignKey(r => r.UserId);
    }

    public static DbContextOptionsBuilder<DatabaseContext> OptionsBuilder => new();

    // Convenience method to recreate the database thus ensuring the new database takes 
    // account of any changes to Models or DatabaseContext. ONLY to be used in development
    public void Initialise()
    {
        Database.EnsureDeleted();
        Database.EnsureCreated();
    }
}