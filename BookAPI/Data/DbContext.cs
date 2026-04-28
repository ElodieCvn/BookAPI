using BookApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookApi.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Book> Books => Set<Book>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(b => b.Id);

            entity.Property(b => b.Title)
                  .IsRequired()
                  .HasMaxLength(200);

            entity.Property(b => b.Author)
                  .IsRequired()
                  .HasMaxLength(150);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);

            entity.HasIndex(u => u.Username).IsUnique();

            entity.Property(u => u.Username)
                  .IsRequired()
                  .HasMaxLength(50);
        });
    }
}
