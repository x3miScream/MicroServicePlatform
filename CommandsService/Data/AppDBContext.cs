using CommandsService.Models;
using Microsoft.EntityFrameworkCore;

namespace CommandsService.Data
{
    public class AppDBContext: DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> opt) : base(opt)
        {

        }

        public DbSet<Platform> Platforms {get;set;} = default!;
        public DbSet<Commands> Commands {get;set;} = default!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Platform>()
                .HasMany(x => x.Commands)
                .WithOne(x => x.Plaatform!)
                .HasForeignKey(x => x.PlatformId);

            modelBuilder
                .Entity<Commands>()
                .HasOne(x => x.Plaatform)
                .WithMany(x => x.Commands)
                .HasForeignKey(x => x.PlatformId);
        }
    }
}