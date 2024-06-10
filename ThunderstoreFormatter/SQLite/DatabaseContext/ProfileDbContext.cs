using Microsoft.EntityFrameworkCore;
using ThunderstoreFormatter.SQLite.Model;

namespace ThunderstoreFormatter.SQLite.DatabaseContext;

public class ProfileDbContext : DbContext
{
    public DbSet<Profile> Profiles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite("Data Source=profiles.db");
}