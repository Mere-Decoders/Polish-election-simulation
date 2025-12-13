using Microsoft.EntityFrameworkCore;
using backend.Models.DBModels;

namespace backend.Data;

public class AppDbContext:DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    public DbSet<User> Users { get; set; }
    public DbSet<DataClaim> DataClaims { get; set; }
    public DbSet<MethodClaim> MethodClaims { get; set; }
    public DbSet<SimulationDataDBWrapper> SimulationData { get; set; }
    public DbSet<SimulationMethodDBWrapper> SimulationMethods { get; set; }

    //commented until issue #61 is resolved
    /* public DbSet<SimulationDataDBWrapper> SimulationData { get; set; }*/
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
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

        modelBuilder.Entity<SimulationDataDBWrapper>(entity =>
        {
            entity.ToTable("simulation_data");
            entity.Property(s => s.Id)
                  .HasDefaultValueSql("gen_random_uuid()");
        });

        modelBuilder.Entity<SimulationMethodDBWrapper>(entity =>
        {
            entity.ToTable("simulation_methods");
            entity.Property(s => s.Id)
                  .HasDefaultValueSql("gen_random_uuid()");
        });

        //commented until issue #61 is resolved
        /*   modelBuilder.Entity<SimulationDataDBWrapper>(entity =>
           {
               entity.ToTable("simulation_data");

               entity.HasKey(e => e.Id);

               entity.OwnsOne(e => e.Data, owned =>
               {
                   owned.ToJson();
               });
           });*/

    }

}