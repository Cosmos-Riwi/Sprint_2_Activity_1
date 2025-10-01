using Microsoft.EntityFrameworkCore;
using Sprint_2_Activity_1.Models;

namespace Sprint_2_Activity_1.Data
{
    public class VeterinaryDbContext : DbContext
    {
        public VeterinaryDbContext(DbContextOptions<VeterinaryDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseMySql(
                "Server=168.119.183.3;Database=veterinaria_san_miguel_jeronimo;Uid=root;Pwd=g0tIFJEQsKHm5$34Pxu1;Port=3307;",
                new MySqlServerVersion(new Version(8, 0, 0)),
                mySqlOptions => mySqlOptions.EnableRetryOnFailure());

        // DbSets for each entity
        public DbSet<Client> Clients { get; set; }
        public DbSet<Pet> Pets { get; set; }
        public DbSet<MedicalHistory> MedicalHistories { get; set; }
        public DbSet<Veterinarian> Veterinarians { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relación 1:N (Un cliente tiene muchas mascotas)
            modelBuilder.Entity<Client>()
                .HasMany(c => c.Pets)
                .WithOne(p => p.Client)
                .HasForeignKey(p => p.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación 1:1 (Una mascota tiene un historial médico)
            modelBuilder.Entity<Pet>()
                .HasOne(p => p.History)
                .WithOne(h => h.Pet)
                .HasForeignKey<MedicalHistory>(h => h.PetId);

            // Relación N:N (Muchos veterinarios atienden muchas mascotas)
            modelBuilder.Entity<Pet>()
                .HasMany(p => p.Veterinarians)
                .WithMany(v => v.Pets);

            // Pet -> Appointments (One-to-Many)
            modelBuilder.Entity<Pet>()
                .HasMany(p => p.Appointments)
                .WithOne(a => a.Pet)
                .HasForeignKey(a => a.PetId)
                .OnDelete(DeleteBehavior.Cascade);

            // Veterinarian -> Appointments (One-to-Many)
            modelBuilder.Entity<Veterinarian>()
                .HasMany(v => v.Appointments)
                .WithOne(a => a.Veterinarian)
                .HasForeignKey(a => a.VeterinarianId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure date properties
            modelBuilder.Entity<Client>()
                .Property(c => c.RegistrationDate)
                .HasColumnType("datetime");

            modelBuilder.Entity<Pet>()
                .Property(p => p.RegistrationDate)
                .HasColumnType("datetime");

            modelBuilder.Entity<MedicalHistory>()
                .Property(m => m.LastUpdated)
                .HasColumnType("datetime");

            modelBuilder.Entity<Veterinarian>()
                .Property(v => v.HireDate)
                .HasColumnType("datetime");

            modelBuilder.Entity<Appointment>()
                .Property(a => a.RegistrationDate)
                .HasColumnType("datetime");

            // Configure decimal precision for cost
            modelBuilder.Entity<Appointment>()
                .Property(a => a.Cost)
                .HasColumnType("decimal(10,2)");
        }
    }
}
