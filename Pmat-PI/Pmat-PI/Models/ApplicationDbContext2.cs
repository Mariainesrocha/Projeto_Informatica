using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Pmat_PI.Models
{
    public partial class ApplicationDbContext2 : DbContext
    {
        public ApplicationDbContext2()
        {
        }

        public ApplicationDbContext2(DbContextOptions<ApplicationDbContext2> options)
            : base(options)
        {
        }

        public virtual DbSet<AnoLetivo> AnoLetivos { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<Competicao> Competicaos { get; set; }
        public virtual DbSet<Concelho> Concelhos { get; set; }
        public virtual DbSet<Distrito> Distritos { get; set; }
        public virtual DbSet<Equipa> Equipas { get; set; }
        public virtual DbSet<EquipaAluno> EquipaAlunos { get; set; }
        public virtual DbSet<EquipaProva> EquipaProvas { get; set; }
        public virtual DbSet<Escola> Escolas { get; set; }
        public virtual DbSet<Freguesium> Freguesia { get; set; }
        public virtual DbSet<Modelo> Modelos { get; set; }
        public virtual DbSet<ModeloNovo> ModeloNovos { get; set; }
        public virtual DbSet<ModeloVelho> ModeloVelhos { get; set; }
        public virtual DbSet<Pai> Pais { get; set; }
        public virtual DbSet<Prova> Provas { get; set; }
        public virtual DbSet<ProvaEquipaEnunciado> ProvaEquipaEnunciados { get; set; }
        public virtual DbSet<ProvaEscola> ProvaEscolas { get; set; }
        public virtual DbSet<ProvaModelo> ProvaModelos { get; set; }
        public virtual DbSet<TipoEscola> TipoEscolas { get; set; }
        public virtual DbSet<Treino> Treinos { get; set; }
        public virtual DbSet<TreinoEnunciado> TreinoEnunciados { get; set; }
        public virtual DbSet<TreinoModelo> TreinoModelos { get; set; }
        public virtual DbSet<UserContacto> UserContactos { get; set; }
        public virtual DbSet<UserContactoTipo> UserContactoTipos { get; set; }

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

            modelBuilder.Entity<AnoLetivo>(entity =>
            {
                entity.HasKey(e => e.AnoLetivo1)
                    .HasName("PK__AnoLetiv__A3590E904309D7BE");

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
                    .HasConstraintName("FK__Concelho__distri__44FF419A");
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
                    .HasConstraintName("FK__Distrito__pais__412EB0B6");
            });

            modelBuilder.Entity<Equipa>(entity =>
            {
                entity.ToTable("Equipa", "pmate");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DataCriacao).HasColumnType("date");

                entity.Property(e => e.Nome)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nome");

                entity.HasOne(d => d.IdEscolaNavigation)
                    .WithMany(p => p.Equipas)
                    .HasForeignKey(d => d.IdEscola)
                    .HasConstraintName("FK__Equipa__IdEscola__607251E5");
            });

            modelBuilder.Entity<EquipaAluno>(entity =>
            {
                entity.HasKey(e => e.IdAlunoEquipa)
                    .HasName("PK__EquipaAl__B3D1C4C91FFFF038");

                entity.ToTable("EquipaAlunos", "pmate");

                entity.HasIndex(e => new { e.IdEquipa, e.IdUser }, "equipa_user")
                    .IsUnique();

                entity.HasOne(d => d.IdEquipaNavigation)
                    .WithMany(p => p.EquipaAlunos)
                    .HasForeignKey(d => d.IdEquipa)
                    .HasConstraintName("FK__EquipaAlu__IdEqu__7EF6D905");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.EquipaAlunos)
                    .HasForeignKey(d => d.IdUser)
                    .HasConstraintName("FK__EquipaAlu__IdUse__7FEAFD3E");
            });

            modelBuilder.Entity<EquipaProva>(entity =>
            {
                entity.HasKey(e => new { e.IdProva, e.IdEquipa })
                    .HasName("PK__EquipaPr__BEE352297E39CAAE");

                entity.ToTable("EquipaProva", "pmate");

                entity.HasOne(d => d.IdEquipaNavigation)
                    .WithMany(p => p.EquipaProvas)
                    .HasForeignKey(d => d.IdEquipa)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EquipaPro__IdEqu__634EBE90");

                entity.HasOne(d => d.IdProvaNavigation)
                    .WithMany(p => p.EquipaProvas)
                    .HasForeignKey(d => d.IdProva)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EquipaPro__IdPro__6442E2C9");
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
                    .HasConstraintName("FK__Escola__IdTipoEs__2DE6D218");

                entity.HasOne(d => d.IdconcelhoNavigation)
                    .WithMany(p => p.Escolas)
                    .HasForeignKey(d => d.Idconcelho)
                    .HasConstraintName("FK__Escola__Idconcel__2EDAF651");
            });

            modelBuilder.Entity<Freguesium>(entity =>
            {
                entity.ToTable("Freguesia", "pmate");

                entity.HasIndex(e => new { e.Nome, e.Concelho }, "concelho_freguesia_unico")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Concelho).HasColumnName("concelho");

                entity.Property(e => e.Nome)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("nome");

                entity.HasOne(d => d.ConcelhoNavigation)
                    .WithMany(p => p.Freguesia)
                    .HasForeignKey(d => d.Concelho)
                    .HasConstraintName("FK__Freguesia__conce__367C1819");
            });

            modelBuilder.Entity<Modelo>(entity =>
            {
                entity.ToTable("Modelo", "pmate");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Tipo).HasMaxLength(10);
            });

            modelBuilder.Entity<ModeloNovo>(entity =>
            {
                entity.HasKey(e => e.IdModel)
                    .HasName("PK__ModeloNo__C2F00099F86A3A2F");

                entity.ToTable("ModeloNovo", "pmate");

                entity.Property(e => e.IdModel).ValueGeneratedNever();

                entity.Property(e => e.IdCycle).HasColumnName("Id_Cycle");

                entity.Property(e => e.IdModeLevel).HasColumnName("Id_ModeLevel");

                entity.Property(e => e.IdModelVersion).HasColumnName("Id_ModelVersion");

                entity.Property(e => e.IdTree).HasColumnName("Id_Tree");

                entity.Property(e => e.IdUser).HasColumnName("Id_User");

                entity.Property(e => e.Obs).HasMaxLength(100);

                entity.Property(e => e.Xml)
                    .HasColumnType("xml")
                    .HasColumnName("XML");

                entity.HasOne(d => d.IdModelNavigation)
                    .WithOne(p => p.ModeloNovo)
                    .HasForeignKey<ModeloNovo>(d => d.IdModel)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ModeloNov__IdMod__2610A626");
            });

            modelBuilder.Entity<ModeloVelho>(entity =>
            {
                entity.HasKey(e => e.IdModel)
                    .HasName("PK__ModeloVe__C2F000993B2B7DEB");

                entity.ToTable("ModeloVelho", "pmate");

                entity.Property(e => e.IdModel).ValueGeneratedNever();

                entity.Property(e => e.CCicloEnsino).HasColumnName("cCicloEnsino");

                entity.Property(e => e.CContador).HasColumnName("cContador");

                entity.Property(e => e.CDataElaboracao)
                    .HasColumnType("datetime")
                    .HasColumnName("cDataElaboracao");

                entity.Property(e => e.CInformacaoAdicional)
                    .HasColumnType("ntext")
                    .HasColumnName("cInformacaoAdicional");

                entity.Property(e => e.CNivelDificuldade).HasColumnName("cNivelDificuldade");

                entity.Property(e => e.CResponsavel).HasColumnName("cResponsavel");

                entity.Property(e => e.DataModificado).HasColumnType("datetime");

                entity.Property(e => e.Letras)
                    .HasMaxLength(30)
                    .IsFixedLength(true);

                entity.Property(e => e.ModificadoPor).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(15);

                entity.Property(e => e.Objectives).HasColumnType("ntext");

                entity.Property(e => e.Obs).HasMaxLength(50);

                entity.Property(e => e.Question).HasColumnType("ntext");

                entity.Property(e => e.Restrictions).HasMaxLength(255);

                entity.Property(e => e.Solution).HasMaxLength(255);

                entity.HasOne(d => d.IdModelNavigation)
                    .WithOne(p => p.ModeloVelho)
                    .HasForeignKey<ModeloVelho>(d => d.IdModel)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ModeloVel__IdMod__28ED12D1");
            });

            modelBuilder.Entity<Pai>(entity =>
            {
                entity.ToTable("Pais", "pmate");

                entity.HasIndex(e => e.Nome, "UQ__Pais__6F71C0DC0F485660")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Nome)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nome");
            });

            modelBuilder.Entity<Prova>(entity =>
            {
                entity.ToTable("Prova", "pmate");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DataCriacao).HasColumnType("datetime");

                entity.Property(e => e.DataInscFinal).HasColumnType("datetime");

                entity.Property(e => e.DataProva).HasColumnType("datetime");

                entity.Property(e => e.Estilo).HasMaxLength(25);

                entity.Property(e => e.FimInscricaoEquipas).HasColumnType("datetime");

                entity.Property(e => e.FimPreInscricao).HasColumnType("datetime");

                entity.Property(e => e.FimProva).HasColumnType("datetime");

                entity.Property(e => e.IdAuthor).HasMaxLength(450);

                entity.Property(e => e.InicioInscricaoEquipas).HasColumnType("datetime");

                entity.Property(e => e.InicioPreInscricao).HasColumnType("datetime");

                entity.Property(e => e.NomeProva)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Plataforma).HasColumnName("plataforma");

                entity.Property(e => e.Url)
                    .HasMaxLength(250)
                    .HasColumnName("URL");

                entity.HasOne(d => d.IdAuthorNavigation)
                    .WithMany(p => p.Provas)
                    .HasForeignKey(d => d.IdAuthor)
                    .HasConstraintName("FK__Prova__IdAuthor__47A6A41B");

                entity.HasOne(d => d.IdCompeticaoNavigation)
                    .WithMany(p => p.Provas)
                    .HasForeignKey(d => d.IdCompeticao)
                    .HasConstraintName("FK__Prova__IdCompeti__46B27FE2");
            });

            modelBuilder.Entity<ProvaEquipaEnunciado>(entity =>
            {
                entity.ToTable("ProvaEquipaEnunciado", "pmate");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Data)
                    .HasColumnType("datetime")
                    .HasColumnName("_Data");

                entity.Property(e => e.Status).HasColumnName("_Status");

                entity.Property(e => e.Tempo)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("tempo")
                    .IsFixedLength(true);

                entity.Property(e => e.UltimoNivel).HasColumnName("ultimoNivel");

                entity.HasOne(d => d.IdEquipaNavigation)
                    .WithMany(p => p.ProvaEquipaEnunciados)
                    .HasForeignKey(d => d.IdEquipa)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ProvaEqui__IdEqu__1A9EF37A");

                entity.HasOne(d => d.IdProvaNavigation)
                    .WithMany(p => p.ProvaEquipaEnunciados)
                    .HasForeignKey(d => d.IdProva)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ProvaEqui__IdPro__19AACF41");
            });

            modelBuilder.Entity<ProvaEscola>(entity =>
            {
                entity.HasKey(e => new { e.IdEscola, e.IdProva })
                    .HasName("PK__ProvaEsc__EB11F89C7578856E");

                entity.ToTable("ProvaEscolas", "pmate");

                entity.Property(e => e.AnoLetivo).HasMaxLength(10);

                entity.Property(e => e.DataRegisto).HasPrecision(3);

                entity.HasOne(d => d.AnoLetivoNavigation)
                    .WithMany(p => p.ProvaEscolas)
                    .HasForeignKey(d => d.AnoLetivo)
                    .HasConstraintName("FK__ProvaEsco__AnoLe__36470DEF");

                entity.HasOne(d => d.EscolaOrganizadoraNavigation)
                    .WithMany(p => p.ProvaEscolaEscolaOrganizadoraNavigations)
                    .HasForeignKey(d => d.EscolaOrganizadora)
                    .HasConstraintName("FK__ProvaEsco__Escol__3552E9B6");

                entity.HasOne(d => d.IdEscolaNavigation)
                    .WithMany(p => p.ProvaEscolaIdEscolaNavigations)
                    .HasForeignKey(d => d.IdEscola)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ProvaEsco__IdEsc__336AA144");

                entity.HasOne(d => d.IdProvaNavigation)
                    .WithMany(p => p.ProvaEscolas)
                    .HasForeignKey(d => d.IdProva)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ProvaEsco__IdPro__345EC57D");
            });

            modelBuilder.Entity<ProvaModelo>(entity =>
            {
                entity.HasKey(e => new { e.IdProva, e.IdModelo, e.Nivel })
                    .HasName("PK__ProvaMod__6059BF9D2EA7EEAD");

                entity.ToTable("ProvaModelos", "pmate");

                entity.HasOne(d => d.IdModeloNavigation)
                    .WithMany(p => p.ProvaModelos)
                    .HasForeignKey(d => d.IdModelo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ProvaMode__IdMod__2CBDA3B5");

                entity.HasOne(d => d.IdProvaNavigation)
                    .WithMany(p => p.ProvaModelos)
                    .HasForeignKey(d => d.IdProva)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ProvaMode__IdPro__2BC97F7C");
            });

            modelBuilder.Entity<TipoEscola>(entity =>
            {
                entity.HasKey(e => e.IdTipoEscola)
                    .HasName("PK__TipoEsco__1B703B8280D738E2");

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
                    .HasConstraintName("FK__Treino__IdAuthor__3F115E1A");
            });

            modelBuilder.Entity<TreinoEnunciado>(entity =>
            {
                entity.ToTable("TreinoEnunciado", "pmate");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Data)
                    .HasColumnType("datetime")
                    .HasColumnName("_Data");

                entity.Property(e => e.IdUser)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.Status).HasColumnName("_Status");

                entity.Property(e => e.Tempo)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("tempo")
                    .IsFixedLength(true);

                entity.Property(e => e.UltimoNivel).HasColumnName("ultimoNivel");

                entity.HasOne(d => d.IdTreinoNavigation)
                    .WithMany(p => p.TreinoEnunciados)
                    .HasForeignKey(d => d.IdTreino)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TreinoEnu__IdTre__1209AD79");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.TreinoEnunciados)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TreinoEnu__IdUse__12FDD1B2");
            });

            modelBuilder.Entity<TreinoModelo>(entity =>
            {
                entity.HasKey(e => new { e.IdTreino, e.IdModelo, e.Nivel })
                    .HasName("PK__TreinoMo__482F0D45B3E63CF8");

                entity.ToTable("TreinoModelos", "pmate");

                entity.HasOne(d => d.IdModeloNavigation)
                    .WithMany(p => p.TreinoModelos)
                    .HasForeignKey(d => d.IdModelo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TreinoMod__IdMod__308E3499");

                entity.HasOne(d => d.IdTreinoNavigation)
                    .WithMany(p => p.TreinoModelos)
                    .HasForeignKey(d => d.IdTreino)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TreinoMod__IdTre__2F9A1060");
            });

            modelBuilder.Entity<UserContacto>(entity =>
            {
                entity.ToTable("UserContacto", "pmate");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Descricao)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.IdTipo).HasColumnName("idTipo");

                entity.Property(e => e.IdUser).HasColumnName("idUser");
            });

            modelBuilder.Entity<UserContactoTipo>(entity =>
            {
                entity.ToTable("UserContactoTipo", "pmate");

                entity.HasIndex(e => e.Tipo, "UQ__UserCont__E7F956490077C509")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Tipo)
                    .HasMaxLength(50)
                    .HasColumnName("tipo");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
