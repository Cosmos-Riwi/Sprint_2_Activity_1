using Microsoft.EntityFrameworkCore;
using Veterinaria.Models;

namespace Veterinaria.Data;

public class VeterinariaContext : DbContext
{
    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Mascota> Mascotas => Set<Mascota>();
    public DbSet<Veterinario> Veterinarios => Set<Veterinario>();
    public DbSet<Atencion> Atenciones => Set<Atencion>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var connectionString = "server=168.119.183.3;database=JuanManuel_veterinary_system;user=root;password=g0tIFJEQsKHm5$34Pxu1;port=3307;";
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>().ToTable("Clientes");
        modelBuilder.Entity<Mascota>().ToTable("Mascotas");
        modelBuilder.Entity<Veterinario>().ToTable("Veterinarios");
        modelBuilder.Entity<Atencion>().ToTable("Atenciones");

        modelBuilder.Entity<Cliente>()
            .HasMany(c => c.Mascotas)
            .WithOne(m => m.Cliente!)
            .HasForeignKey(m => m.ClienteId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Mascota>()
            .HasMany(m => m.Atenciones)
            .WithOne(a => a.Mascota!)
            .HasForeignKey(a => a.MascotaId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Veterinario>()
            .HasMany(v => v.Atenciones)
            .WithOne(a => a.Veterinario!)
            .HasForeignKey(a => a.VeterinarioId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}


