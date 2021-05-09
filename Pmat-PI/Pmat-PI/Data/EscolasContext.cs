using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Pmat_PI.Models
{
    public partial class EscolasContext : DbContext
    {
        public EscolasContext()
        {
        }

        public EscolasContext(DbContextOptions<EscolasContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Concelho> Concelhos { get; set; }
        public virtual DbSet<Distrito> Distritos { get; set; }
        public virtual DbSet<Escola> Escolas { get; set; }
        public virtual DbSet<Freguesia> Freguesia { get; set; }
        public virtual DbSet<Pais> Pais { get; set; }
        public virtual DbSet<TipoEscola> TipoEscolas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=localhost;Database=pmate2-demo;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Latin1_General_CI_AS");

            modelBuilder.Entity<Concelho>(entity =>
            {
                entity.ToTable("Concelho", "pmate");

                entity.HasIndex(e => new { e.Nome, e.Distrito }, "distrito_concelho_unico")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Distrito).HasColumnName("distrito");

                entity.Property(e => e.Nome)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nome");

                entity.HasOne(d => d.DistritoNavigation)
                    .WithMany(p => p.Concelhos)
                    .HasForeignKey(d => d.Distrito)
                    .HasConstraintName("FK__Concelho__distri__2704CA5F");
            });

            modelBuilder.Entity<Distrito>(entity =>
            {
                entity.ToTable("Distrito", "pmate");

                entity.HasIndex(e => new { e.Nome, e.Pais }, "pais_distrito_unico")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Nome)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nome");

                entity.Property(e => e.Pais).HasColumnName("pais");

                entity.HasOne(d => d.PaisNavigation)
                    .WithMany(p => p.Distritos)
                    .HasForeignKey(d => d.Pais)
                    .HasConstraintName("FK__Distrito__pais__2334397B");
            });

            modelBuilder.Entity<Escola>(entity =>
            {
                entity.ToTable("Escola", "pmate");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CodDgeec).HasColumnName("COD_DGEEC");

                entity.Property(e => e.CodDgpgf).HasColumnName("COD_DGPGF");

                entity.Property(e => e.CodigoPostal).HasMaxLength(4);

                entity.Property(e => e.Email).HasMaxLength(255);

                entity.Property(e => e.Ensinos)
                    .HasMaxLength(30)
                    .HasColumnName("ENSINOS");

                entity.Property(e => e.Estado).HasColumnName("estado");

                entity.Property(e => e.ExtensaoCodPostal).HasMaxLength(3);

                entity.Property(e => e.Fax).HasMaxLength(9);

                entity.Property(e => e.Gruponatureza)
                    .HasMaxLength(30)
                    .HasColumnName("GRUPONATUREZA");

                entity.Property(e => e.Latitude)
                    .HasMaxLength(30)
                    .HasColumnName("LATITUDE");

                entity.Property(e => e.Localidade).HasMaxLength(50);

                entity.Property(e => e.Longitude)
                    .HasMaxLength(30)
                    .HasColumnName("LONGITUDE");

                entity.Property(e => e.Morada)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.NomeEscola)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Telefone)
                    .HasMaxLength(9)
                    .IsFixedLength(true);

                entity.Property(e => e.Website).HasMaxLength(255);

                entity.HasOne(d => d.IdTipoEscolaNavigation)
                    .WithMany(p => p.Escolas)
                    .HasForeignKey(d => d.IdTipoEscola)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Escola__IdTipoEs__11D4A34F");

                entity.HasOne(d => d.IdconcelhoNavigation)
                    .WithMany(p => p.Escolas)
                    .HasForeignKey(d => d.Idconcelho)
                    .HasConstraintName("FK__Escola__Idconcel__12C8C788");
            });

            modelBuilder.Entity<Freguesia>(entity =>
            {
                entity.ToTable("Freguesia", "pmate");

                entity.HasIndex(e => new { e.Nome, e.Concelho }, "concelho_freguesia_unico")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Concelho).HasColumnName("concelho");

                entity.Property(e => e.Nome)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nome");

                entity.HasOne(d => d.ConcelhoNavigation)
                    .WithMany(p => p.Freguesia)
                    .HasForeignKey(d => d.Concelho)
                    .HasConstraintName("FK__freguesia__conce__2AD55B43");
            });

            modelBuilder.Entity<Pais>(entity =>
            {
                entity.ToTable("Pais", "pmate");

                entity.HasIndex(e => e.Nome, "UQ__Pais__6F71C0DCE7A7A1EB")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Nome)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nome");
            });

            modelBuilder.Entity<TipoEscola>(entity =>
            {
                entity.HasKey(e => e.IdTipoEscola)
                    .HasName("PK__TipoEsco__1B703B82342D89C1");

                entity.ToTable("TipoEscola", "pmate");

                entity.Property(e => e.IdTipoEscola).HasColumnName("id_tipo_escola");

                entity.Property(e => e.DescricaoTipoEscola)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.TipoEscola1)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("TipoEscola")
                    .IsFixedLength(true);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
