using Microsoft.EntityFrameworkCore;
using TurismoApp.Infraestructure.Entities;

namespace TurismoApp.Infraestructure.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Ciudad> Ciudades { get; set; }
        public DbSet<Recorrido> Recorridos { get; set; }
        public DbSet<ClienteRecorrido> ClienteRecorridos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Recorrido>()
                .HasOne(r => r.CiudadOrigen)
                .WithMany()
                .HasForeignKey(r => r.CiudadOrigenId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Recorrido>()
                .HasOne(r => r.CiudadDestino)
                .WithMany()
                .HasForeignKey(r => r.CiudadDestinoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ClienteRecorrido>()
                .HasKey(cr => new { cr.ClienteId, cr.RecorridoId });

            modelBuilder.Entity<ClienteRecorrido>()
                .HasOne(cr => cr.Cliente)
                .WithMany(c => c.ClienteRecorridos)
                .HasForeignKey(cr => cr.ClienteId);

            modelBuilder.Entity<ClienteRecorrido>()
                .HasOne(cr => cr.Recorrido)
                .WithMany(r => r.ClienteRecorridos)
                .HasForeignKey(cr => cr.RecorridoId);
        }
    }
}
