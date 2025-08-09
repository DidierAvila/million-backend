using Microsoft.EntityFrameworkCore;
using Million.Domain.Entities;

namespace Million.Infrastructure.Data
{
    public class MillionDbContext : DbContext
    {
        public MillionDbContext(DbContextOptions<MillionDbContext> options) : base(options)
        {
        }

        public DbSet<Propietario> Propietarios { get; set; }
        public DbSet<Propiedad> Propiedades { get; set; }
        public DbSet<ImagenPropiedad> ImagenesPropiedad { get; set; }
        public DbSet<TrazabilidadPropiedad> TrazabilidadesPropiedad { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Propietario
            modelBuilder.Entity<Propietario>(entity =>
            {
                entity.HasKey(e => e.IdOwner);
                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(250);
                entity.Property(e => e.FechaNacimiento)
                    .IsRequired();
            });

            // Configure Propiedad
            modelBuilder.Entity<Propiedad>(entity =>
            {
                entity.HasKey(e => e.IdProperty);
                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(250);
                entity.Property(e => e.Direccion)
                    .IsRequired()
                    .HasMaxLength(500);
                entity.Property(e => e.Precio)
                    .IsRequired()
                    .HasColumnType("decimal(18,2)");
                entity.Property(e => e.Impuestos)
                    .IsRequired()
                    .HasColumnType("decimal(18,2)");
                entity.Property(e => e.CodigoInterno)
                    .IsRequired()
                    .HasMaxLength(50);

                // Foreign key relationship
                entity.HasOne(e => e.Propietario)
                    .WithMany(p => p.Propiedades)
                    .HasForeignKey(e => e.IdOwner)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure ImagenPropiedad
            modelBuilder.Entity<ImagenPropiedad>(entity =>
            {
                entity.HasKey(e => e.IdPropertyImage);
                entity.Property(e => e.Archivo)
                    .IsRequired()
                    .HasMaxLength(500);

                // Foreign key relationship
                entity.HasOne(e => e.Propiedad)
                    .WithMany(p => p.ImagenesPropiedad)
                    .HasForeignKey(e => e.IdProperty)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure TrazabilidadPropiedad
            modelBuilder.Entity<TrazabilidadPropiedad>(entity =>
            {
                entity.HasKey(e => e.IdPropertyTrace);
                entity.Property(e => e.FechaVenta)
                    .IsRequired();
                entity.Property(e => e.Valor)
                    .IsRequired()
                    .HasColumnType("decimal(18,2)");

                // Foreign key relationship
                entity.HasOne(e => e.Propiedad)
                    .WithMany(p => p.TrazabilidadesPropiedad)
                    .HasForeignKey(e => e.IdProperty)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}