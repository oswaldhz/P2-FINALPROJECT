using System;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "8.0.5");

            modelBuilder.Entity("Domain.Entities.Equipo", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("TEXT");

                b.Property<string>("Descripcion")
                    .HasMaxLength(500)
                    .HasColumnType("TEXT");

                b.Property<string>("Nombre")
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnType("TEXT");

                b.HasKey("Id");

                b.ToTable("Equipos", (string)null);
            });

            modelBuilder.Entity("Domain.Entities.Reserva", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("TEXT");

                b.Property<string>("Estado")
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnType("TEXT");

                b.Property<DateTime>("FechaFin")
                    .HasColumnType("TEXT");

                b.Property<DateTime>("FechaInicio")
                    .HasColumnType("TEXT");

                b.Property<string>("Notas")
                    .HasMaxLength(1000)
                    .HasColumnType("TEXT");

                b.Property<Guid>("EquipoId")
                    .HasColumnType("TEXT");

                b.Property<Guid>("UsuarioId")
                    .HasColumnType("TEXT");

                b.HasKey("Id");

                b.HasIndex("EquipoId");

                b.HasIndex("UsuarioId");

                b.ToTable("Reservas", (string)null);
            });

            modelBuilder.Entity("Domain.Entities.Software", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("TEXT");

                b.Property<string>("Nombre")
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnType("TEXT");

                b.Property<string>("Version")
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnType("TEXT");

                b.HasKey("Id");

                b.ToTable("Softwares", (string)null);
            });

            modelBuilder.Entity("Domain.Entities.Usuario", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("TEXT");

                b.Property<string>("Email")
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnType("TEXT");

                b.Property<string>("Nombre")
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnType("TEXT");

                b.Property<string>("PasswordHash")
                    .IsRequired()
                    .HasColumnType("TEXT");

                b.Property<string>("Rol")
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnType("TEXT");

                b.HasKey("Id");

                b.HasIndex("Email")
                    .IsUnique();

                b.ToTable("Usuarios", (string)null);
            });

            modelBuilder.Entity("EquipoSoftware", b =>
            {
                b.Property<Guid>("EquipoId")
                    .HasColumnType("TEXT");

                b.Property<Guid>("SoftwareId")
                    .HasColumnType("TEXT");

                b.HasKey("EquipoId", "SoftwareId");

                b.HasIndex("SoftwareId");

                b.ToTable("EquiposSoftwares", (string)null);
            });

            modelBuilder.Entity("Domain.Entities.Reserva", b =>
            {
                b.HasOne("Domain.Entities.Equipo", "Equipo")
                    .WithMany("Reservas")
                    .HasForeignKey("EquipoId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasOne("Domain.Entities.Usuario", "Usuario")
                    .WithMany("Reservas")
                    .HasForeignKey("UsuarioId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Navigation("Equipo");

                b.Navigation("Usuario");
            });

            modelBuilder.Entity("EquipoSoftware", b =>
            {
                b.HasOne("Domain.Entities.Equipo", null)
                    .WithMany()
                    .HasForeignKey("EquipoId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasOne("Domain.Entities.Software", null)
                    .WithMany()
                    .HasForeignKey("SoftwareId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

            modelBuilder.Entity("Domain.Entities.Equipo", b =>
            {
                b.Navigation("Reservas");

                b.Navigation("Softwares");
            });

            modelBuilder.Entity("Domain.Entities.Usuario", b =>
            {
                b.Navigation("Reservas");
            });
        }
    }
}
