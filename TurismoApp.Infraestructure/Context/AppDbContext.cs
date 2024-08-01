using Microsoft.EntityFrameworkCore;
using TurismoApp.Infraestructure.Entities;

namespace TurismoApp.Infraestructure.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Departamento> Departamentos { get; set; }
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Ciudad> Ciudades { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}