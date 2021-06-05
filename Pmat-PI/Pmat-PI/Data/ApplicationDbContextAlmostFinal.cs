using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Pmat_PI.Models
{
    public partial class ApplicationDbContextAlmostFinal : DbContext
    {
        public ApplicationDbContextAlmostFinal()
        {
        }

        public ApplicationDbContextAlmostFinal(DbContextOptions<ApplicationDbContextAlmostFinal> options)
            : base(options)
        {
        }

        public virtual DbSet<AnoEscolar> AnoEscolars { get; set; }
        public virtual DbSet<AnoLetivo> AnoLetivos { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<CicloEnsino> CicloEnsinos { get; set; }
        public virtual DbSet<Competicao> Competicaos { get; set; }
        public virtual DbSet<Concelho> Concelhos { get; set; }
        public virtual DbSet<Distrito> Distritos { get; set; }
        public virtual DbSet<Equipa> Equipas { get; set; }
        public virtual DbSet<EquipaAluno> EquipaAlunos { get; set; }
        public virtual DbSet<EquipaProva> EquipaProvas { get; set; }
        public virtual DbSet<Escola> Escolas { get; set; }
        public virtual DbSet<Freguesia> Freguesia { get; set; }
        public virtual DbSet<Modelo> Modelos { get; set; }
        public virtual DbSet<ModeloNovo> ModeloNovos { get; set; }
        public virtual DbSet<ModeloVelho> ModeloVelhos { get; set; }
        public virtual DbSet<Pais> Pais { get; set; }
        public virtual DbSet<Projeto> Projetos { get; set; }
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
        public virtual DbSet<UserEscola> UserEscolas { get; set; }
        public virtual DbSet<UserEscolaHistorico> UserEscolaHistoricos { get; set; }
        public virtual DbSet<SubProva> SubProvas { get; set; }

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

            modelBuilder.Entity<CicloEnsino>(entity =>
            {
                entity.ToTable("CicloEnsino", "pmate");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Abreviatura)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Descritivo)
                    .IsRequired()
                    .HasMaxLength(100);
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
                    .HasConstraintName("FK__Equipa__IdEscola__269AB60B");
            });

            modelBuilder.Entity<EquipaAluno>(entity =>
            {
                entity.HasKey(e => e.IdAlunoEquipa)
                    .HasName("PK__EquipaAl__B3D1C4C9422057E6");

                entity.ToTable("EquipaAlunos", "pmate");

                entity.HasIndex(e => new { e.IdEquipa, e.IdUser }, "equipa_user")
                    .IsUnique();

                entity.HasOne(d => d.IdEquipaNavigation)
                    .WithMany(p => p.EquipaAlunos)
                    .HasForeignKey(d => d.IdEquipa)
                    .HasConstraintName("FK__EquipaAlu__IdEqu__2A6B46EF");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.EquipaAlunos)
                    .HasForeignKey(d => d.IdUser)
                    .HasConstraintName("FK__EquipaAlu__IdUse__2B5F6B28");
            });

            modelBuilder.Entity<EquipaProva>(entity =>
            {
                entity.HasKey(e => new { e.IdProva, e.IdEquipa })
                    .HasName("PK__EquipaPr__BEE35229EDBE1B17");

                entity.ToTable("EquipaProva", "pmate");

                entity.HasOne(d => d.IdEquipaNavigation)
                    .WithMany(p => p.EquipaProvas)
                    .HasForeignKey(d => d.IdEquipa)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EquipaPro__IdEqu__442B18F2");

                entity.HasOne(d => d.IdProvaNavigation)
                    .WithMany(p => p.EquipaProvas)
                    .HasForeignKey(d => d.IdProva)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EquipaPro__IdPro__451F3D2B");
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
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("nome");

                entity.HasOne(d => d.ConcelhoNavigation)
                    .WithMany(p => p.Freguesia)
                    .HasForeignKey(d => d.Concelho)
                    .HasConstraintName("FK__freguesia__conce__2AD55B43");
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
                    .HasName("PK__ModeloNo__C2F00099C89400FE");

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
                    .HasConstraintName("FK__ModeloNov__IdMod__35DCF99B");
            });

            modelBuilder.Entity<ModeloVelho>(entity =>
            {
                entity.HasKey(e => e.IdModel)
                    .HasName("PK__ModeloVe__C2F00099BE541047");

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
                    .HasConstraintName("FK__ModeloVel__IdMod__38B96646");
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

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Descricao)
                    .HasColumnType("text")
                    .HasColumnName("descricao");

                entity.Property(e => e.Url)
                    .HasMaxLength(100)
                    .HasColumnName("URL");
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
                    .HasConstraintName("FK__Prova__IdAuthor__1C1D2798");

                entity.HasOne(d => d.IdCompeticaoNavigation)
                    .WithMany(p => p.Provas)
                    .HasForeignKey(d => d.IdCompeticao)
                    .HasConstraintName("FK__Prova__IdCompeti__1B29035F");

                entity.HasOne(d => d.RefIdCicloEnsinoNavigation)
                    .WithMany(p => p.Provas)
                    .HasForeignKey(d => d.RefIdCicloEnsino)
                    .HasConstraintName("FK__Prova__RefIdCicl__60C757A0");
            });

            modelBuilder.Entity<SubProva>(entity =>
            {
                entity.HasKey(e => new { e.IdProvaPai, e.IdProvaFilho })
                    .HasName("PK__SubProva__89F3B2720053DDFF");

                entity.ToTable("SubProvas", "pmate");

                entity.HasOne(d => d.IdProvaFilhoNavigation)
                    .WithMany(p => p.SubProvaIdProvaFilhoNavigations)
                    .HasForeignKey(d => d.IdProvaFilho)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SubProvas__IdPro__70099B30");

                entity.HasOne(d => d.IdProvaPaiNavigation)
                    .WithMany(p => p.SubProvaIdProvaPaiNavigations)
                    .HasForeignKey(d => d.IdProvaPai)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SubProvas__IdPro__6F1576F7");
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
                    .HasConstraintName("FK__ProvaEqui__IdEqu__48EFCE0F");

                entity.HasOne(d => d.IdProvaNavigation)
                    .WithMany(p => p.ProvaEquipaEnunciados)
                    .HasForeignKey(d => d.IdProva)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ProvaEqui__IdPro__47FBA9D6");
            });

            modelBuilder.Entity<ProvaEscola>(entity =>
            {
                entity.HasKey(e => new { e.IdEscola, e.IdProva })
                    .HasName("PK__ProvaEsc__EB11F89C4EC72277");

                entity.ToTable("ProvaEscolas", "pmate");

                entity.Property(e => e.AnoLetivo).HasMaxLength(10);

                entity.Property(e => e.DataRegisto).HasPrecision(3);

                entity.HasOne(d => d.AnoLetivoNavigation)
                    .WithMany(p => p.ProvaEscolas)
                    .HasForeignKey(d => d.AnoLetivo)
                    .HasConstraintName("FK__ProvaEsco__AnoLe__23BE4960");

                entity.HasOne(d => d.EscolaOrganizadoraNavigation)
                    .WithMany(p => p.ProvaEscolaEscolaOrganizadoraNavigations)
                    .HasForeignKey(d => d.EscolaOrganizadora)
                    .HasConstraintName("FK__ProvaEsco__Escol__22CA2527");

                entity.HasOne(d => d.IdEscolaNavigation)
                    .WithMany(p => p.ProvaEscolaIdEscolaNavigations)
                    .HasForeignKey(d => d.IdEscola)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ProvaEsco__IdEsc__20E1DCB5");

                entity.HasOne(d => d.IdProvaNavigation)
                    .WithMany(p => p.ProvaEscolas)
                    .HasForeignKey(d => d.IdProva)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ProvaEsco__IdPro__21D600EE");
            });

            modelBuilder.Entity<ProvaModelo>(entity =>
            {
                entity.HasKey(e => new { e.IdProva, e.IdModelo, e.Nivel })
                    .HasName("PK__ProvaMod__6059BF9DF6B3D3C7");

                entity.ToTable("ProvaModelos", "pmate");

                entity.HasOne(d => d.IdModeloNavigation)
                    .WithMany(p => p.ProvaModelos)
                    .HasForeignKey(d => d.IdModelo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ProvaMode__IdMod__3C89F72A");

                entity.HasOne(d => d.IdProvaNavigation)
                    .WithMany(p => p.ProvaModelos)
                    .HasForeignKey(d => d.IdProva)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ProvaMode__IdPro__3B95D2F1");
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
                    .HasConstraintName("FK__Treino__IdAuthor__2E3BD7D3");

                entity.HasOne(d => d.RefIdCicloEnsinoNavigation)
                    .WithMany(p => p.Treinos)
                    .HasForeignKey(d => d.RefIdCicloEnsino)
                    .HasConstraintName("FK__Treino__RefIdCic__5FD33367");
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
                    .HasConstraintName("FK__TreinoEnu__IdTre__536D5C82");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.TreinoEnunciados)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TreinoEnu__IdUse__546180BB");
            });

            modelBuilder.Entity<TreinoModelo>(entity =>
            {
                entity.HasKey(e => new { e.IdTreino, e.IdModelo, e.Nivel })
                    .HasName("PK__TreinoMo__482F0D450B00C229");

                entity.ToTable("TreinoModelos", "pmate");

                entity.HasOne(d => d.IdModeloNavigation)
                    .WithMany(p => p.TreinoModelos)
                    .HasForeignKey(d => d.IdModelo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TreinoMod__IdMod__405A880E");

                entity.HasOne(d => d.IdTreinoNavigation)
                    .WithMany(p => p.TreinoModelos)
                    .HasForeignKey(d => d.IdTreino)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TreinoMod__IdTre__3F6663D5");
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

                entity.HasIndex(e => e.Tipo, "UQ__UserCont__E7F956496233548D")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Tipo)
                    .HasMaxLength(50)
                    .HasColumnName("tipo");
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
                    .HasConstraintName("FK__UserEscol__AnoLe__090A5324");

                entity.HasOne(d => d.IdAnoEscolarNavigation)
                    .WithMany(p => p.UserEscolas)
                    .HasForeignKey(d => d.IdAnoEscolar)
                    .HasConstraintName("FK__UserEscol__IdAno__08162EEB");

                entity.HasOne(d => d.IdEscolaNavigation)
                    .WithMany(p => p.UserEscolas)
                    .HasForeignKey(d => d.IdEscola)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserEscol__IdEsc__062DE679");

                entity.HasOne(d => d.IdProjetoNavigation)
                    .WithMany(p => p.UserEscolas)
                    .HasForeignKey(d => d.IdProjeto)
                    .HasConstraintName("FK__UserEscol__idPro__09FE775D");

                entity.HasOne(d => d.IdResponsavelNavigation)
                    .WithMany(p => p.InverseIdResponsavelNavigation)
                    .HasForeignKey(d => d.IdResponsavel)
                    .HasConstraintName("FK__UserEscol__IdRes__07220AB2");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.UserEscolas)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserEscol__data___0539C240");
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
                    .HasConstraintName("FK__UserEscol__AnoLe__10AB74EC");

                entity.HasOne(d => d.IdAnoEscolarNavigation)
                    .WithMany(p => p.UserEscolaHistoricos)
                    .HasForeignKey(d => d.IdAnoEscolar)
                    .HasConstraintName("FK__UserEscol__IdAno__0FB750B3");

                entity.HasOne(d => d.IdEscolaNavigation)
                    .WithMany(p => p.UserEscolaHistoricos)
                    .HasForeignKey(d => d.IdEscola)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserEscol__IdEsc__0DCF0841");

                entity.HasOne(d => d.IdProjetoNavigation)
                    .WithMany(p => p.UserEscolaHistoricos)
                    .HasForeignKey(d => d.IdProjeto)
                    .HasConstraintName("FK__UserEscol__idPro__119F9925");

                entity.HasOne(d => d.IdResponsavelNavigation)
                    .WithMany(p => p.UserEscolaHistoricos)
                    .HasForeignKey(d => d.IdResponsavel)
                    .HasConstraintName("FK__UserEscol__IdRes__0EC32C7A");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.UserEscolaHistoricos)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserEscol__data___0CDAE408");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
