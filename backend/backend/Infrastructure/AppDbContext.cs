using Microsoft.EntityFrameworkCore;
using backend.Infrastructure.Entities;

namespace backend.Infrastructure;

public class AppDbContext:DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    public DbSet<UserEntity> Users { get; set; }
    public DbSet<DataClaim> DataClaims { get; set; }
    public DbSet<MethodClaim> MethodClaims { get; set; }
    public DbSet<SimulationDataEntity> SimulationDataStore { get; set; }
    public DbSet<SimulationMethodEntity> SimulationMethodsStore { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserEntity>(entity =>
        {
            entity.ToTable("users");
            entity.Property(u => u.Id)
                  .HasDefaultValueSql("gen_random_uuid()");
        });

        modelBuilder.Entity<DataClaim>(entity =>
        {
            entity.ToTable("data_claims");
            entity.HasKey(e => new { e.UserId, e.DataId });
        });

        modelBuilder.Entity<MethodClaim>(entity =>
        {
            entity.ToTable("method_claims");
            entity.HasKey(e => new { e.UserId, e.MethodId });
        });

        modelBuilder.Entity<SimulationDataEntity>(entity =>
        {
            entity.ToTable("simulation_data");
            entity.HasKey(e => e.Id);
            entity.Property(s => s.Id)
                .ValueGeneratedNever();
        });

        modelBuilder.Entity<SimulationMethodEntity>(entity =>
        {
            entity.ToTable("simulation_methods");
            entity.HasKey(e => e.Id);
            entity.Property(s => s.Id)
                .ValueGeneratedNever();
        });

    }

}