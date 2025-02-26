using Microsoft.EntityFrameworkCore;

namespace GardenHero.Models.Database
{
    public class GardenDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Plant> Plants { get; set; }
        public DbSet<GoodNeighbor> GoodNeighbors { get; set; }
        public DbSet<BadNeighbor> BadNeighbors { get; set; }
        public DbSet<Bed> Beds { get; set; }
        public DbSet<Planting> Plantings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=garden.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasMany(c => c.Plants).WithOne(p => p.Category).HasForeignKey(p => p.CategoryId);

            modelBuilder.Entity<GoodNeighbor>().HasKey(g => new { g.Plant1Id, g.Plant2Id });
            modelBuilder.Entity<BadNeighbor>().HasKey(b => new { b.Plant1Id, b.Plant2Id });

            modelBuilder.Entity<Planting>().HasOne(p => p.Plant).WithMany().HasForeignKey(p => p.PlantId);
            modelBuilder.Entity<Planting>().HasOne(p => p.Bed).WithMany(b => b.Plantings).HasForeignKey(p => p.BedId);
        }
    }
}
