using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Pmat_PI.Models
{
    public partial class treinoContext : DbContext
    {
        public treinoContext()
        {
        }

        public treinoContext(DbContextOptions<treinoContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<Competicao> Competicaos { get; set; }
        public virtual DbSet<Treino> Treinos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=localhost;Database=pmate2-demo;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Latin1_General_CI_AS");

            modelBuilder.Entity<AspNetUser>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(80)
                    .HasDefaultValueSql("(N'')");

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<Competicao>(entity =>
            {
                entity.ToTable("Competicao", "pmate");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DataFim).HasColumnType("datetime");

                entity.Property(e => e.DataInicio).HasColumnType("datetime");

                entity.Property(e => e.Etiqueta)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Nome).IsRequired();
            });

            modelBuilder.Entity<Treino>(entity =>
            {
                entity.ToTable("Treino", "pmate");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DataCriacao).HasColumnType("datetime");

                entity.Property(e => e.Estilo).HasMaxLength(25);

                entity.Property(e => e.IdAuthor).HasMaxLength(450);

                entity.Property(e => e.NomeProva)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Plataforma).HasColumnName("plataforma");

                entity.Property(e => e.Url)
                    .HasMaxLength(250)
                    .HasColumnName("URL");

                entity.HasOne(d => d.IdAuthorNavigation)
                    .WithMany(p => p.Treinos)
                    .HasForeignKey(d => d.IdAuthor)
                    .HasConstraintName("FK__Treino__IdAuthor__3118447E");

                entity.HasOne(d => d.IdCompeticaoNavigation)
                    .WithMany(p => p.Treinos)
                    .HasForeignKey(d => d.IdCompeticao)
                    .HasConstraintName("FK__Treino__IdCompet__30242045");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
