using Microsoft.EntityFrameworkCore;
using ThunderstoreFormatter.SQLite.Model;

namespace ThunderstoreFormatter.SQLite.DatabaseContext;

public class ModDbContext : DbContext
{
    public DbSet<Mod> Mods { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=mods.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Mod>()
            .Property(m => m.Categories)
            .HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries));
    }
}