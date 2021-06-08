using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Pmat_PI.Migrations
{
    public partial class Go : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "pmate");

            migrationBuilder.CreateTable(
                name: "AnoEscolar",
                schema: "pmate",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ano = table.Column<string>(type: "char(50)", unicode: false, fixedLength: true, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnoEscolar", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "AnoLetivo",
                schema: "pmate",
                columns: table => new
                {
                    AnoLetivo = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Inicio = table.Column<DateTime>(type: "datetime", nullable: false),
                    Fim = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__AnoLetiv__A3590E90F7460D67", x => x.AnoLetivo);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false, defaultValueSql: "(N'')"),
                    Roles = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CicloEnsino",
                schema: "pmate",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    Descritivo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Abreviatura = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CicloEnsino", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Competicao",
                schema: "pmate",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataInicio = table.Column<DateTime>(type: "datetime", nullable: false),
                    DataFim = table.Column<DateTime>(type: "datetime", nullable: true),
                    Etiqueta = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Competicao", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Modelo",
                schema: "pmate",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modelo", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Pais",
                schema: "pmate",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pais", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Projeto",
                schema: "pmate",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    descricao = table.Column<string>(type: "text", nullable: true),
                    URL = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projeto", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "TipoEscola",
                schema: "pmate",
                columns: table => new
                {
                    id_tipo_escola = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TipoEscola = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: true),
                    DescricaoTipoEscola = table.Column<string>(type: "char(100)", unicode: false, fixedLength: true, maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TipoEsco__1B703B82342D89C1", x => x.id_tipo_escola);
                });

            migrationBuilder.CreateTable(
                name: "UserContacto",
                schema: "pmate",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idUser = table.Column<int>(type: "int", nullable: false),
                    idTipo = table.Column<int>(type: "int", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserContacto", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "UserContactoTipo",
                schema: "pmate",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tipo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserContactoTipo", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Treino",
                schema: "pmate",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdAuthor = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    NomeProva = table.Column<string>(type: "char(60)", unicode: false, fixedLength: true, maxLength: 60, nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    MaxEscolas = table.Column<int>(type: "int", nullable: true),
                    MaxTentJogo = table.Column<int>(type: "int", nullable: true),
                    TempoTotalJogo = table.Column<int>(type: "int", nullable: true),
                    NumNiveis = table.Column<int>(type: "int", nullable: false),
                    VidasPorNivel = table.Column<int>(type: "int", nullable: true),
                    NumElemsEquipa = table.Column<int>(type: "int", nullable: true),
                    Calculadora = table.Column<bool>(type: "bit", nullable: true),
                    Estilo = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    URL = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    TreinoVisivel = table.Column<bool>(type: "bit", nullable: false),
                    RefIdCicloEnsino = table.Column<int>(type: "int", nullable: true),
                    plataforma = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Treino", x => x.id);
                    table.ForeignKey(
                        name: "FK__Treino__IdAuthor__2E3BD7D3",
                        column: x => x.IdAuthor,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Treino__RefIdCic__5FD33367",
                        column: x => x.RefIdCicloEnsino,
                        principalSchema: "pmate",
                        principalTable: "CicloEnsino",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Prova",
                schema: "pmate",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdAuthor = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    IdCompeticao = table.Column<int>(type: "int", nullable: true),
                    NomeProva = table.Column<string>(type: "char(60)", unicode: false, fixedLength: true, maxLength: 60, nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    MaxEscolas = table.Column<int>(type: "int", nullable: true),
                    MaxTentJogo = table.Column<int>(type: "int", nullable: true),
                    TempoTotalJogo = table.Column<int>(type: "int", nullable: true),
                    NumNiveis = table.Column<int>(type: "int", nullable: false),
                    VidasPorNivel = table.Column<int>(type: "int", nullable: true),
                    NumElemsEquipa = table.Column<int>(type: "int", nullable: true),
                    Calculadora = table.Column<bool>(type: "bit", nullable: true),
                    DataInscFinal = table.Column<DateTime>(type: "datetime", nullable: true),
                    DataProva = table.Column<DateTime>(type: "datetime", nullable: true),
                    InicioPreInscricao = table.Column<DateTime>(type: "datetime", nullable: true),
                    FimPreInscricao = table.Column<DateTime>(type: "datetime", nullable: true),
                    InicioInscricaoEquipas = table.Column<DateTime>(type: "datetime", nullable: true),
                    FimInscricaoEquipas = table.Column<DateTime>(type: "datetime", nullable: true),
                    FimProva = table.Column<DateTime>(type: "datetime", nullable: true),
                    Estilo = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    URL = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    TreinoVisivel = table.Column<bool>(type: "bit", nullable: false),
                    RefIdCicloEnsino = table.Column<int>(type: "int", nullable: true),
                    plataforma = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prova", x => x.id);
                    table.ForeignKey(
                        name: "FK__Prova__IdAuthor__1C1D2798",
                        column: x => x.IdAuthor,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Prova__IdCompeti__1B29035F",
                        column: x => x.IdCompeticao,
                        principalSchema: "pmate",
                        principalTable: "Competicao",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Prova__RefIdCicl__60C757A0",
                        column: x => x.RefIdCicloEnsino,
                        principalSchema: "pmate",
                        principalTable: "CicloEnsino",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ModeloNovo",
                schema: "pmate",
                columns: table => new
                {
                    IdModel = table.Column<int>(type: "int", nullable: false),
                    Question = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Id_ModeLevel = table.Column<int>(type: "int", nullable: false),
                    IdModelType = table.Column<int>(type: "int", nullable: false),
                    Id_Tree = table.Column<int>(type: "int", nullable: false),
                    AnswersNumber = table.Column<short>(type: "smallint", nullable: true),
                    Id_Cycle = table.Column<int>(type: "int", nullable: true),
                    Id_ModelVersion = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: true),
                    XML = table.Column<string>(type: "xml", nullable: true),
                    Id_User = table.Column<int>(type: "int", nullable: true),
                    Obs = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ModeloNo__C2F00099C89400FE", x => x.IdModel);
                    table.ForeignKey(
                        name: "FK__ModeloNov__IdMod__35DCF99B",
                        column: x => x.IdModel,
                        principalSchema: "pmate",
                        principalTable: "Modelo",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ModeloVelho",
                schema: "pmate",
                columns: table => new
                {
                    IdModel = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Objectives = table.Column<string>(type: "ntext", nullable: true),
                    Question = table.Column<string>(type: "ntext", nullable: true),
                    Solution = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Restrictions = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Obs = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NumeroRespostas = table.Column<int>(type: "int", nullable: true),
                    DataModificado = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModificadoPor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Letras = table.Column<string>(type: "nchar(30)", fixedLength: true, maxLength: 30, nullable: true),
                    Tipo = table.Column<int>(type: "int", nullable: true),
                    cCicloEnsino = table.Column<int>(type: "int", nullable: true),
                    cNivelDificuldade = table.Column<int>(type: "int", nullable: true),
                    cContador = table.Column<int>(type: "int", nullable: true),
                    cResponsavel = table.Column<int>(type: "int", nullable: true),
                    cDataElaboracao = table.Column<DateTime>(type: "datetime", nullable: true),
                    cInformacaoAdicional = table.Column<string>(type: "ntext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ModeloVe__C2F00099BE541047", x => x.IdModel);
                    table.ForeignKey(
                        name: "FK__ModeloVel__IdMod__38B96646",
                        column: x => x.IdModel,
                        principalSchema: "pmate",
                        principalTable: "Modelo",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Distrito",
                schema: "pmate",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    pais = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Distrito", x => x.id);
                    table.ForeignKey(
                        name: "FK__Distrito__pais__2334397B",
                        column: x => x.pais,
                        principalSchema: "pmate",
                        principalTable: "Pais",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TreinoEnunciado",
                schema: "pmate",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUser = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    IdTreino = table.Column<int>(type: "int", nullable: false),
                    _Data = table.Column<DateTime>(type: "datetime", nullable: false),
                    _Status = table.Column<int>(type: "int", nullable: true),
                    ultimoNivel = table.Column<int>(type: "int", nullable: true),
                    tempo = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreinoEnunciado", x => x.id);
                    table.ForeignKey(
                        name: "FK__TreinoEnu__IdTre__536D5C82",
                        column: x => x.IdTreino,
                        principalSchema: "pmate",
                        principalTable: "Treino",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__TreinoEnu__IdUse__546180BB",
                        column: x => x.IdUser,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TreinoModelos",
                schema: "pmate",
                columns: table => new
                {
                    IdTreino = table.Column<int>(type: "int", nullable: false),
                    IdModelo = table.Column<int>(type: "int", nullable: false),
                    Nivel = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TreinoMo__482F0D450B00C229", x => new { x.IdTreino, x.IdModelo, x.Nivel });
                    table.ForeignKey(
                        name: "FK__TreinoMod__IdMod__405A880E",
                        column: x => x.IdModelo,
                        principalSchema: "pmate",
                        principalTable: "Modelo",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__TreinoMod__IdTre__3F6663D5",
                        column: x => x.IdTreino,
                        principalSchema: "pmate",
                        principalTable: "Treino",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProvaModelos",
                schema: "pmate",
                columns: table => new
                {
                    IdProva = table.Column<int>(type: "int", nullable: false),
                    IdModelo = table.Column<int>(type: "int", nullable: false),
                    Nivel = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ProvaMod__6059BF9DF6B3D3C7", x => new { x.IdProva, x.IdModelo, x.Nivel });
                    table.ForeignKey(
                        name: "FK__ProvaMode__IdMod__3C89F72A",
                        column: x => x.IdModelo,
                        principalSchema: "pmate",
                        principalTable: "Modelo",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__ProvaMode__IdPro__3B95D2F1",
                        column: x => x.IdProva,
                        principalSchema: "pmate",
                        principalTable: "Prova",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SubProvas",
                schema: "pmate",
                columns: table => new
                {
                    IdProvaPai = table.Column<int>(type: "int", nullable: false),
                    IdProvaFilho = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SubProva__89F3B2720053DDFF", x => new { x.IdProvaPai, x.IdProvaFilho });
                    table.ForeignKey(
                        name: "FK__SubProvas__IdPro__6F1576F7",
                        column: x => x.IdProvaPai,
                        principalSchema: "pmate",
                        principalTable: "Prova",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__SubProvas__IdPro__70099B30",
                        column: x => x.IdProvaFilho,
                        principalSchema: "pmate",
                        principalTable: "Prova",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Concelho",
                schema: "pmate",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    distrito = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Concelho", x => x.id);
                    table.ForeignKey(
                        name: "FK__Concelho__distri__2704CA5F",
                        column: x => x.distrito,
                        principalSchema: "pmate",
                        principalTable: "Distrito",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Escola",
                schema: "pmate",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdTipoEscola = table.Column<int>(type: "int", nullable: false),
                    NomeEscola = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Morada = table.Column<string>(type: "char(100)", unicode: false, fixedLength: true, maxLength: 100, nullable: true),
                    CodigoPostal = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    ExtensaoCodPostal = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    Localidade = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Telefone = table.Column<string>(type: "nchar(9)", fixedLength: true, maxLength: 9, nullable: true),
                    Fax = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Website = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Idconcelho = table.Column<int>(type: "int", nullable: true),
                    IdFreguesia = table.Column<int>(type: "int", nullable: true),
                    estado = table.Column<bool>(type: "bit", nullable: true),
                    ENSINOS = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    LATITUDE = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    LONGITUDE = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    GRUPONATUREZA = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    COD_DGEEC = table.Column<int>(type: "int", nullable: true),
                    COD_DGPGF = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Escola", x => x.id);
                    table.ForeignKey(
                        name: "FK__Escola__Idconcel__12C8C788",
                        column: x => x.Idconcelho,
                        principalSchema: "pmate",
                        principalTable: "Concelho",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Escola__IdTipoEs__11D4A34F",
                        column: x => x.IdTipoEscola,
                        principalSchema: "pmate",
                        principalTable: "TipoEscola",
                        principalColumn: "id_tipo_escola",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Freguesia",
                schema: "pmate",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    concelho = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Freguesia", x => x.id);
                    table.ForeignKey(
                        name: "FK__freguesia__conce__2AD55B43",
                        column: x => x.concelho,
                        principalSchema: "pmate",
                        principalTable: "Concelho",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Equipa",
                schema: "pmate",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "date", nullable: true),
                    IdEscola = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipa", x => x.id);
                    table.ForeignKey(
                        name: "FK__Equipa__IdEscola__269AB60B",
                        column: x => x.IdEscola,
                        principalSchema: "pmate",
                        principalTable: "Escola",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProvaEscolas",
                schema: "pmate",
                columns: table => new
                {
                    IdEscola = table.Column<int>(type: "int", nullable: false),
                    IdProva = table.Column<int>(type: "int", nullable: false),
                    IdUserEscola = table.Column<int>(type: "int", nullable: true),
                    DataRegisto = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false),
                    EscolaOrganizadora = table.Column<int>(type: "int", nullable: true),
                    AnoLetivo = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ProvaEsc__EB11F89C4EC72277", x => new { x.IdEscola, x.IdProva });
                    table.ForeignKey(
                        name: "FK__ProvaEsco__AnoLe__23BE4960",
                        column: x => x.AnoLetivo,
                        principalSchema: "pmate",
                        principalTable: "AnoLetivo",
                        principalColumn: "AnoLetivo",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__ProvaEsco__Escol__22CA2527",
                        column: x => x.EscolaOrganizadora,
                        principalSchema: "pmate",
                        principalTable: "Escola",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__ProvaEsco__IdEsc__20E1DCB5",
                        column: x => x.IdEscola,
                        principalSchema: "pmate",
                        principalTable: "Escola",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__ProvaEsco__IdPro__21D600EE",
                        column: x => x.IdProva,
                        principalSchema: "pmate",
                        principalTable: "Prova",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserEscola",
                schema: "pmate",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUser = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdEscola = table.Column<int>(type: "int", nullable: false),
                    IdResponsavel = table.Column<int>(type: "int", nullable: true),
                    IdAnoEscolar = table.Column<int>(type: "int", nullable: true),
                    AnoLetivo = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    idProjeto = table.Column<int>(type: "int", nullable: true),
                    data_ = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserEscola", x => x.id);
                    table.ForeignKey(
                        name: "FK__UserEscol__AnoLe__090A5324",
                        column: x => x.AnoLetivo,
                        principalSchema: "pmate",
                        principalTable: "AnoLetivo",
                        principalColumn: "AnoLetivo",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__UserEscol__data___0539C240",
                        column: x => x.IdUser,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__UserEscol__IdAno__08162EEB",
                        column: x => x.IdAnoEscolar,
                        principalSchema: "pmate",
                        principalTable: "AnoEscolar",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__UserEscol__IdEsc__062DE679",
                        column: x => x.IdEscola,
                        principalSchema: "pmate",
                        principalTable: "Escola",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__UserEscol__idPro__09FE775D",
                        column: x => x.idProjeto,
                        principalSchema: "pmate",
                        principalTable: "Projeto",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__UserEscol__IdRes__07220AB2",
                        column: x => x.IdResponsavel,
                        principalSchema: "pmate",
                        principalTable: "UserEscola",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EquipaAlunos",
                schema: "pmate",
                columns: table => new
                {
                    IdAlunoEquipa = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdEquipa = table.Column<int>(type: "int", nullable: true),
                    IdUser = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__EquipaAl__B3D1C4C9422057E6", x => x.IdAlunoEquipa);
                    table.ForeignKey(
                        name: "FK__EquipaAlu__IdEqu__2A6B46EF",
                        column: x => x.IdEquipa,
                        principalSchema: "pmate",
                        principalTable: "Equipa",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__EquipaAlu__IdUse__2B5F6B28",
                        column: x => x.IdUser,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EquipaProva",
                schema: "pmate",
                columns: table => new
                {
                    IdEquipa = table.Column<int>(type: "int", nullable: false),
                    IdProva = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__EquipaPr__BEE35229EDBE1B17", x => new { x.IdProva, x.IdEquipa });
                    table.ForeignKey(
                        name: "FK__EquipaPro__IdEqu__442B18F2",
                        column: x => x.IdEquipa,
                        principalSchema: "pmate",
                        principalTable: "Equipa",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__EquipaPro__IdPro__451F3D2B",
                        column: x => x.IdProva,
                        principalSchema: "pmate",
                        principalTable: "Prova",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProvaEquipaEnunciado",
                schema: "pmate",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdEquipa = table.Column<int>(type: "int", nullable: false),
                    IdProva = table.Column<int>(type: "int", nullable: false),
                    _Data = table.Column<DateTime>(type: "datetime", nullable: false),
                    _Status = table.Column<int>(type: "int", nullable: true),
                    ultimoNivel = table.Column<int>(type: "int", nullable: true),
                    tempo = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProvaEquipaEnunciado", x => x.id);
                    table.ForeignKey(
                        name: "FK__ProvaEqui__IdEqu__48EFCE0F",
                        column: x => x.IdEquipa,
                        principalSchema: "pmate",
                        principalTable: "Equipa",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__ProvaEqui__IdPro__47FBA9D6",
                        column: x => x.IdProva,
                        principalSchema: "pmate",
                        principalTable: "Prova",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserEscolaHistorico",
                schema: "pmate",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUser = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    IdEscola = table.Column<int>(type: "int", nullable: false),
                    IdResponsavel = table.Column<int>(type: "int", nullable: true),
                    IdAnoEscolar = table.Column<int>(type: "int", nullable: true),
                    AnoLetivo = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    idProjeto = table.Column<int>(type: "int", nullable: true),
                    data_ = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserEscolaHistorico", x => x.id);
                    table.ForeignKey(
                        name: "FK__UserEscol__AnoLe__10AB74EC",
                        column: x => x.AnoLetivo,
                        principalSchema: "pmate",
                        principalTable: "AnoLetivo",
                        principalColumn: "AnoLetivo",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__UserEscol__data___0CDAE408",
                        column: x => x.IdUser,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__UserEscol__IdAno__0FB750B3",
                        column: x => x.IdAnoEscolar,
                        principalSchema: "pmate",
                        principalTable: "AnoEscolar",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__UserEscol__IdEsc__0DCF0841",
                        column: x => x.IdEscola,
                        principalSchema: "pmate",
                        principalTable: "Escola",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__UserEscol__idPro__119F9925",
                        column: x => x.idProjeto,
                        principalSchema: "pmate",
                        principalTable: "Projeto",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__UserEscol__IdRes__0EC32C7A",
                        column: x => x.IdResponsavel,
                        principalSchema: "pmate",
                        principalTable: "UserEscola",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "([NormalizedUserName] IS NOT NULL)");

            migrationBuilder.CreateIndex(
                name: "distrito_concelho_unico",
                schema: "pmate",
                table: "Concelho",
                columns: new[] { "nome", "distrito" },
                unique: true,
                filter: "[nome] IS NOT NULL AND [distrito] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Concelho_distrito",
                schema: "pmate",
                table: "Concelho",
                column: "distrito");

            migrationBuilder.CreateIndex(
                name: "IX_Distrito_pais",
                schema: "pmate",
                table: "Distrito",
                column: "pais");

            migrationBuilder.CreateIndex(
                name: "pais_distrito_unico",
                schema: "pmate",
                table: "Distrito",
                columns: new[] { "nome", "pais" },
                unique: true,
                filter: "[nome] IS NOT NULL AND [pais] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Equipa_IdEscola",
                schema: "pmate",
                table: "Equipa",
                column: "IdEscola");

            migrationBuilder.CreateIndex(
                name: "equipa_user",
                schema: "pmate",
                table: "EquipaAlunos",
                columns: new[] { "IdEquipa", "IdUser" },
                unique: true,
                filter: "[IdEquipa] IS NOT NULL AND [IdUser] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_EquipaAlunos_IdUser",
                schema: "pmate",
                table: "EquipaAlunos",
                column: "IdUser");

            migrationBuilder.CreateIndex(
                name: "IX_EquipaProva_IdEquipa",
                schema: "pmate",
                table: "EquipaProva",
                column: "IdEquipa");

            migrationBuilder.CreateIndex(
                name: "IX_Escola_Idconcelho",
                schema: "pmate",
                table: "Escola",
                column: "Idconcelho");

            migrationBuilder.CreateIndex(
                name: "IX_Escola_IdTipoEscola",
                schema: "pmate",
                table: "Escola",
                column: "IdTipoEscola");

            migrationBuilder.CreateIndex(
                name: "concelho_freguesia_unico",
                schema: "pmate",
                table: "Freguesia",
                columns: new[] { "nome", "concelho" },
                unique: true,
                filter: "[nome] IS NOT NULL AND [concelho] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Freguesia_concelho",
                schema: "pmate",
                table: "Freguesia",
                column: "concelho");

            migrationBuilder.CreateIndex(
                name: "UQ__Pais__6F71C0DCE7A7A1EB",
                schema: "pmate",
                table: "Pais",
                column: "nome",
                unique: true,
                filter: "[nome] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Prova_IdAuthor",
                schema: "pmate",
                table: "Prova",
                column: "IdAuthor");

            migrationBuilder.CreateIndex(
                name: "IX_Prova_IdCompeticao",
                schema: "pmate",
                table: "Prova",
                column: "IdCompeticao");

            migrationBuilder.CreateIndex(
                name: "IX_Prova_RefIdCicloEnsino",
                schema: "pmate",
                table: "Prova",
                column: "RefIdCicloEnsino");

            migrationBuilder.CreateIndex(
                name: "IX_ProvaEquipaEnunciado_IdEquipa",
                schema: "pmate",
                table: "ProvaEquipaEnunciado",
                column: "IdEquipa");

            migrationBuilder.CreateIndex(
                name: "IX_ProvaEquipaEnunciado_IdProva",
                schema: "pmate",
                table: "ProvaEquipaEnunciado",
                column: "IdProva");

            migrationBuilder.CreateIndex(
                name: "IX_ProvaEscolas_AnoLetivo",
                schema: "pmate",
                table: "ProvaEscolas",
                column: "AnoLetivo");

            migrationBuilder.CreateIndex(
                name: "IX_ProvaEscolas_EscolaOrganizadora",
                schema: "pmate",
                table: "ProvaEscolas",
                column: "EscolaOrganizadora");

            migrationBuilder.CreateIndex(
                name: "IX_ProvaEscolas_IdProva",
                schema: "pmate",
                table: "ProvaEscolas",
                column: "IdProva");

            migrationBuilder.CreateIndex(
                name: "IX_ProvaModelos_IdModelo",
                schema: "pmate",
                table: "ProvaModelos",
                column: "IdModelo");

            migrationBuilder.CreateIndex(
                name: "IX_SubProvas_IdProvaFilho",
                schema: "pmate",
                table: "SubProvas",
                column: "IdProvaFilho");

            migrationBuilder.CreateIndex(
                name: "IX_Treino_IdAuthor",
                schema: "pmate",
                table: "Treino",
                column: "IdAuthor");

            migrationBuilder.CreateIndex(
                name: "IX_Treino_RefIdCicloEnsino",
                schema: "pmate",
                table: "Treino",
                column: "RefIdCicloEnsino");

            migrationBuilder.CreateIndex(
                name: "IX_TreinoEnunciado_IdTreino",
                schema: "pmate",
                table: "TreinoEnunciado",
                column: "IdTreino");

            migrationBuilder.CreateIndex(
                name: "IX_TreinoEnunciado_IdUser",
                schema: "pmate",
                table: "TreinoEnunciado",
                column: "IdUser");

            migrationBuilder.CreateIndex(
                name: "IX_TreinoModelos_IdModelo",
                schema: "pmate",
                table: "TreinoModelos",
                column: "IdModelo");

            migrationBuilder.CreateIndex(
                name: "UQ__UserCont__E7F956496233548D",
                schema: "pmate",
                table: "UserContactoTipo",
                column: "tipo",
                unique: true,
                filter: "[tipo] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserEscola_AnoLetivo",
                schema: "pmate",
                table: "UserEscola",
                column: "AnoLetivo");

            migrationBuilder.CreateIndex(
                name: "IX_UserEscola_IdAnoEscolar",
                schema: "pmate",
                table: "UserEscola",
                column: "IdAnoEscolar");

            migrationBuilder.CreateIndex(
                name: "IX_UserEscola_IdEscola",
                schema: "pmate",
                table: "UserEscola",
                column: "IdEscola");

            migrationBuilder.CreateIndex(
                name: "IX_UserEscola_idProjeto",
                schema: "pmate",
                table: "UserEscola",
                column: "idProjeto");

            migrationBuilder.CreateIndex(
                name: "IX_UserEscola_IdResponsavel",
                schema: "pmate",
                table: "UserEscola",
                column: "IdResponsavel");

            migrationBuilder.CreateIndex(
                name: "user_escola_projeto_anolet_escolar",
                schema: "pmate",
                table: "UserEscola",
                columns: new[] { "IdUser", "idProjeto", "IdEscola", "AnoLetivo", "IdAnoEscolar" },
                unique: true,
                filter: "[IdUser] IS NOT NULL AND [idProjeto] IS NOT NULL AND [AnoLetivo] IS NOT NULL AND [IdAnoEscolar] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserEscolaHistorico_AnoLetivo",
                schema: "pmate",
                table: "UserEscolaHistorico",
                column: "AnoLetivo");

            migrationBuilder.CreateIndex(
                name: "IX_UserEscolaHistorico_IdAnoEscolar",
                schema: "pmate",
                table: "UserEscolaHistorico",
                column: "IdAnoEscolar");

            migrationBuilder.CreateIndex(
                name: "IX_UserEscolaHistorico_IdEscola",
                schema: "pmate",
                table: "UserEscolaHistorico",
                column: "IdEscola");

            migrationBuilder.CreateIndex(
                name: "IX_UserEscolaHistorico_idProjeto",
                schema: "pmate",
                table: "UserEscolaHistorico",
                column: "idProjeto");

            migrationBuilder.CreateIndex(
                name: "IX_UserEscolaHistorico_IdResponsavel",
                schema: "pmate",
                table: "UserEscolaHistorico",
                column: "IdResponsavel");

            migrationBuilder.CreateIndex(
                name: "IX_UserEscolaHistorico_IdUser",
                schema: "pmate",
                table: "UserEscolaHistorico",
                column: "IdUser");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EquipaAlunos",
                schema: "pmate");

            migrationBuilder.DropTable(
                name: "EquipaProva",
                schema: "pmate");

            migrationBuilder.DropTable(
                name: "Freguesia",
                schema: "pmate");

            migrationBuilder.DropTable(
                name: "ModeloNovo",
                schema: "pmate");

            migrationBuilder.DropTable(
                name: "ModeloVelho",
                schema: "pmate");

            migrationBuilder.DropTable(
                name: "ProvaEquipaEnunciado",
                schema: "pmate");

            migrationBuilder.DropTable(
                name: "ProvaEscolas",
                schema: "pmate");

            migrationBuilder.DropTable(
                name: "ProvaModelos",
                schema: "pmate");

            migrationBuilder.DropTable(
                name: "SubProvas",
                schema: "pmate");

            migrationBuilder.DropTable(
                name: "TreinoEnunciado",
                schema: "pmate");

            migrationBuilder.DropTable(
                name: "TreinoModelos",
                schema: "pmate");

            migrationBuilder.DropTable(
                name: "UserContacto",
                schema: "pmate");

            migrationBuilder.DropTable(
                name: "UserContactoTipo",
                schema: "pmate");

            migrationBuilder.DropTable(
                name: "UserEscolaHistorico",
                schema: "pmate");

            migrationBuilder.DropTable(
                name: "Equipa",
                schema: "pmate");

            migrationBuilder.DropTable(
                name: "Prova",
                schema: "pmate");

            migrationBuilder.DropTable(
                name: "Modelo",
                schema: "pmate");

            migrationBuilder.DropTable(
                name: "Treino",
                schema: "pmate");

            migrationBuilder.DropTable(
                name: "UserEscola",
                schema: "pmate");

            migrationBuilder.DropTable(
                name: "Competicao",
                schema: "pmate");

            migrationBuilder.DropTable(
                name: "CicloEnsino",
                schema: "pmate");

            migrationBuilder.DropTable(
                name: "AnoLetivo",
                schema: "pmate");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "AnoEscolar",
                schema: "pmate");

            migrationBuilder.DropTable(
                name: "Escola",
                schema: "pmate");

            migrationBuilder.DropTable(
                name: "Projeto",
                schema: "pmate");

            migrationBuilder.DropTable(
                name: "Concelho",
                schema: "pmate");

            migrationBuilder.DropTable(
                name: "TipoEscola",
                schema: "pmate");

            migrationBuilder.DropTable(
                name: "Distrito",
                schema: "pmate");

            migrationBuilder.DropTable(
                name: "Pais",
                schema: "pmate");
        }
    }
}
