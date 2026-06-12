using Microsoft.EntityFrameworkCore;

namespace Source.Data
{
    public class ApplicationDbContext : DbContext
    {
        // Constructor
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // DbSet Properties (Add a DbSet for each entity/model in the application here)
        // Example: public DbSet<ExampleEntity> Examples { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure entity relationships, indexes, unique constraints, default values, etc.
            // Example:
            // modelBuilder.Entity<ExampleEntity>(entity =>
            // {
            //     entity.HasIndex(e => e.Property).IsUnique();
            //     entity.HasOne(e => e.RelatedEntity)
            //           .WithMany(r => r.ExampleEntities)
            //           .HasForeignKey(e => e.RelatedEntityId);
            // });
        }
    }
}