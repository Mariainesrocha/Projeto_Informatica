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

        public virtual DbSet<AnoEscolar> AnoEscolars { get; set; }
        public virtual DbSet<AnoLetivo> AnoLetivos { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<Concelho> Concelhos { get; set; }
        public virtual DbSet<Distrito> Distritos { get; set; }
        public virtual DbSet<Escola> Escolas { get; set; }
        public virtual DbSet<Freguesia> Freguesia { get; set; }
        public virtual DbSet<Pais> Pais { get; set; }
        public virtual DbSet<Projeto> Projetos { get; set; }
        public virtual DbSet<TipoEscola> TipoEscolas { get; set; }
        public virtual DbSet<UserEscola> UserEscolas { get; set; }
        public virtual DbSet<UserEscolaHistorico> UserEscolaHistoricos { get; set; }

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

            modelBuilder.Entity<AnoEscolar>(entity =>
            {
                entity.ToTable("AnoEscolar", "pmate");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Ano)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<AnoLetivo>(entity =>
            {
                entity.HasKey(e => e.AnoLetivo1)
                    .HasName("PK__AnoLetiv__A3590E90F7460D67");

                entity.ToTable("AnoLetivo", "pmate");

                entity.Property(e => e.AnoLetivo1)
                    .HasMaxLength(10)
                    .HasColumnName("AnoLetivo");

                entity.Property(e => e.Fim).HasColumnType("datetime");

                entity.Property(e => e.Inicio).HasColumnType("datetime");
            });

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

            modelBuilder.Entity<Projeto>(entity =>
            {
                entity.ToTable("Projeto", "pmate");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();//.UseIdentityColumn(seed: 0, increment: 1);

                entity.Property(e => e.Descricao)
                    .HasColumnType("text")
                    .HasColumnName("descricao");

                entity.Property(e => e.Url)
                    .HasMaxLength(100)
                    .HasColumnName("URL");
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

            modelBuilder.Entity<UserEscola>(entity =>
            {
                entity.ToTable("UserEscola", "pmate");

                entity.HasIndex(e => new { e.IdUser, e.IdProjeto, e.IdEscola, e.AnoLetivo, e.IdAnoEscolar }, "user_escola_projeto_anolet_escolar")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AnoLetivo).HasMaxLength(10);

                entity.Property(e => e.Data)
                    .HasColumnType("datetime")
                    .HasColumnName("data_");

                entity.Property(e => e.IdProjeto).HasColumnName("idProjeto");

                entity.Property(e => e.IdUser).IsRequired();

                entity.HasOne(d => d.AnoLetivoNavigation)
                    .WithMany(p => p.UserEscolas)
                    .HasForeignKey(d => d.AnoLetivo)
                    .HasConstraintName("FK__UserEscol__AnoLe__5772F790");

                entity.HasOne(d => d.IdAnoEscolarNavigation)
                    .WithMany(p => p.UserEscolas)
                    .HasForeignKey(d => d.IdAnoEscolar)
                    .HasConstraintName("FK__UserEscol__IdAno__567ED357");

                entity.HasOne(d => d.IdEscolaNavigation)
                    .WithMany(p => p.UserEscolas)
                    .HasForeignKey(d => d.IdEscola)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserEscol__IdEsc__54968AE5");

                entity.HasOne(d => d.IdProjetoNavigation)
                    .WithMany(p => p.UserEscolas)
                    .HasForeignKey(d => d.IdProjeto)
                    .HasConstraintName("FK__UserEscol__idPro__58671BC9");

                entity.HasOne(d => d.IdResponsavelNavigation)
                    .WithMany(p => p.InverseIdResponsavelNavigation)
                    .HasForeignKey(d => d.IdResponsavel)
                    .HasConstraintName("FK__UserEscol__IdRes__558AAF1E");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.UserEscolas)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserEscol__data___53A266AC");
            });

            modelBuilder.Entity<UserEscolaHistorico>(entity =>
            {
                entity.ToTable("UserEscolaHistorico", "pmate");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AnoLetivo).HasMaxLength(10);

                entity.Property(e => e.Data)
                    .HasColumnType("datetime")
                    .HasColumnName("data_");

                entity.Property(e => e.IdProjeto).HasColumnName("idProjeto");

                entity.Property(e => e.IdUser)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.AnoLetivoNavigation)
                    .WithMany(p => p.UserEscolaHistoricos)
                    .HasForeignKey(d => d.AnoLetivo)
                    .HasConstraintName("FK__UserEscol__AnoLe__61F08603");

                entity.HasOne(d => d.IdAnoEscolarNavigation)
                    .WithMany(p => p.UserEscolaHistoricos)
                    .HasForeignKey(d => d.IdAnoEscolar)
                    .HasConstraintName("FK__UserEscol__IdAno__60FC61CA");

                entity.HasOne(d => d.IdEscolaNavigation)
                    .WithMany(p => p.UserEscolaHistoricos)
                    .HasForeignKey(d => d.IdEscola)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserEscol__IdEsc__5F141958");

                entity.HasOne(d => d.IdProjetoNavigation)
                    .WithMany(p => p.UserEscolaHistoricos)
                    .HasForeignKey(d => d.IdProjeto)
                    .HasConstraintName("FK__UserEscol__idPro__62E4AA3C");

                entity.HasOne(d => d.IdResponsavelNavigation)
                    .WithMany(p => p.UserEscolaHistoricos)
                    .HasForeignKey(d => d.IdResponsavel)
                    .HasConstraintName("FK__UserEscol__IdRes__60083D91");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.UserEscolaHistoricos)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserEscol__data___5E1FF51F");
            });
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
