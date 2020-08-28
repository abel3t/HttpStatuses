using HttpStatuses.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace HttpStatuses.Entities
{
  public class StatusContext : DbContext
  {  
    public StatusContext(DbContextOptions<StatusContext> options) : base(options) {}
    public DbSet<Status> Statues { get; set; }
    
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
      => optionsBuilder.UseNpgsql("Host=localhost;Database=postgres;Username=abel3t;Password=3Yu0r1lXc7l5q8i4E4x8R7h");
  }
}