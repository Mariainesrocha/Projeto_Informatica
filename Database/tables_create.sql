CREATE SCHEMA pmate;
GO


-- Commands to add tables to Net Core App's Context
--XXX     Scaffold-DbContext "Server=localhost;Database=pmate2-demo;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models
--XXX     -context 'treinoContext' 
--XXX     -Tables AspNetUsers,Treino, Competicao


------------------------------------ LOCATIONS RELATED -----------------------------------

CREATE TABLE pmate.Pais( 
	id int IDENTITY(1,1) PRIMARY KEY, 
	nome VARCHAR(50) UNIQUE,
);

CREATE TABLE pmate.Distrito( 
	id int IDENTITY(1,1) PRIMARY KEY, 
	nome VARCHAR(50),
	pais int ,

	FOREIGN KEY (pais) REFERENCES pmate.Pais(id),
	CONSTRAINT pais_distrito_unico UNIQUE (nome,pais),

);

CREATE TABLE pmate.Concelho(
	id int IDENTITY(1,1) PRIMARY KEY, 
	nome VARCHAR(50),
	distrito  int,

	FOREIGN KEY (distrito) REFERENCES pmate.Distrito(id),
	CONSTRAINT distrito_concelho_unico UNIQUE (nome,distrito),
);

CREATE TABLE pmate.Freguesia(
	id int IDENTITY(1,1) PRIMARY KEY, 
	nome VARCHAR(200),
	concelho int,

	FOREIGN KEY (concelho) REFERENCES pmate.Concelho(id),
	CONSTRAINT concelho_freguesia_unico UNIQUE (nome,concelho),
);






------------------------------------ USER RELATED -----------------------------------

CREATE TABLE pmate.Users(
    id nvarchar(450) ,
    Nome nvarchar(100) ,
    --Username nvarchar(30) , tratado pelo ASP USERS
    --Password nvarchar(20) , tratado pelo ASP USERS
    --QuestionPass nvarchar(100) , tratado pelo ASP USERS
    --AnswerPass nvarchar(100) , tratado pelo ASP USERS
    --IdEscola int , tratado pelo UserRoleEscola
    DataDeRegisto datetime ,
    --IdTipoUser int NOT NULL, role?
    Morada nvarchar(100) ,
    CodPostal nvarchar(4) ,
    ExtensaoCodPostal nvarchar(3) ,
    Localidade nvarchar(30) ,
    idsexo int NOT NULL,
    idNEE int NOT NULL,
    iduser_pk int ,
    editado bit NOT NULL,
    obs nvarchar(max) ,
    -- Guid uniqueidentifier , --so usado em UMA LINHA !!!
    -- StatusEmail bit , --so usado em UMA LINHA !!!
    -- UU nvarchar(100) , --so usado em 49 linhas !!!
    rgpd_comunicacao bit , --5335
    rgpd_estatistica bit , --5335
    rgpd_data datetime , --4335
    validade_pessoal datetime , --4335
    validade_estatistica datetime , --4335

    PRIMARY KEY(id),
    FOREIGN KEY(id) REFERENCES dbo.AspNetUsers(Id)
);




CREATE TABLE pmate.UserContactoTipo(     -- OLD tbldicionariocontacto
	id int IDENTITY(1,1) PRIMARY KEY,  -- OLD idDicionario
	tipo nvarchar(50) UNIQUE,          -- OLD Descricao
);




CREATE TABLE pmate.UserContacto(      -- OLD tblcontacto
	id int IDENTITY(1,1) PRIMARY KEY, -- OLD idContacto
	idUser int NOT NULL,
	idTipo int NOT NULL,              -- OLD: idDic
	Descricao nvarchar(255) NOT NULL,
	--iduser_pk int NOT NULL, WHY iduser again?
);






------------------------------------ SCHOOL RELATED -----------------------------------

CREATE TABLE pmate.TipoEscola(
    id_tipo_escola int IDENTITY(1,1) NOT NULL,
    TipoEscola char(10) ,
    DescricaoTipoEscola char(100) ,
    PRIMARY KEY(id_tipo_escola)
);


CREATE TABLE pmate.Escola(
    id int IDENTITY(1,1) NOT NULL,
    IdTipoEscola int NOT NULL,
    NomeEscola nvarchar(100) NOT NULL,
    -- IdDistrito int NOT NULL,
	-- IdPais int NOT NULL,
    Morada char(100) ,
    CodigoPostal nvarchar(4) ,
    ExtensaoCodPostal nvarchar(3) ,
    Localidade nvarchar(50) ,
    Telefone nchar(9) ,
    Fax nvarchar(9) ,
    Email nvarchar(255) ,
    Website nvarchar(255) , -- OLD Http
    Idconcelho int ,
    IdFreguesia int , --s� usado em 72 linhas das 23902, remover?
    estado bit,
    ENSINOS nvarchar(30) ,
    LATITUDE nvarchar(30) ,
    LONGITUDE nvarchar(30) ,
	GRUPONATUREZA nvarchar(30) ,     -- usado 2650 em 23902
	COD_DGEEC int ,                  -- usado 2647 em 23902
	COD_DGPGF int ,                  -- usado 2647 em 23902

    PRIMARY KEY(id),
    FOREIGN KEY(IdTipoEscola) REFERENCES pmate.TipoEscola(id_tipo_escola),
    --FOREIGN KEY(IdFreguesia) REFERENCES dbo.Freguesias(id_freguesia),
    FOREIGN KEY(IdConcelho) REFERENCES pmate.Concelho(id),
    --FOREIGN KEY(IdDistrito) REFERENCES dbo.Distritos(id_distrito),
    --FOREIGN KEY(IdPais) REFERENCES dbo.Paises(id_pais)

);

CREATE TABLE pmate.EscolaInfoExtra( -- ATTRS da escola potencialmente inuteis.Se forem necessários ficaram numa tabela extra
	IdEscola int PRIMARY KEY , 
	DataInscricao nvarchar(30) ,     -- usado 210 linhas em 23902, remover?
	NIF nvarchar(9) ,                -- usado 664 em 23902
	icon nvarchar(max) ,             -- usado 479 em 23902
	PreInscEquaMat int ,             -- usado 121 em 23902	
	UltimaActualizacao nvarchar(30), -- usado 997 em 23902

	FOREIGN KEY(IdEscola) REFERENCES pmate.Escola(id)
)


CREATE TABLE pmate.AnoLetivo(
	AnoLetivo nvarchar(10) PRIMARY KEY,
	Inicio datetime NOT NULL,
	Fim datetime NOT NULL
);

CREATE TABLE pmate.AnoEscolar( -- OLD tblDicAnoEscolaridade
	id int PRIMARY KEY,
	Ano char(50) NOT NULL
);


CREATE TABLE pmate.Projeto( -- OLD dbo.tbldicprojectos 
	id int NOT NULL,
	descricao TEXT,
	URL nvarchar(100),
	PRIMARY KEY(id),
);

CREATE TABLE pmate.UserEscola (
	id int IDENTITY(1,1) PRIMARY KEY, 
	IdUser nvarchar(450) NOT NULL,
	IdEscola int NOT NULL,
	IdResponsavel int,
	IdAnoEscolar int,
	AnoLetivo  nvarchar(10),
	idProjeto int,
	data_ datetime

	FOREIGN KEY(IdUser) REFERENCES dbo.AspNetUsers(Id),
	FOREIGN KEY(IdEscola) REFERENCES pmate.Escola(id),
	FOREIGN KEY(IdResponsavel) REFERENCES pmate.UserEscola(id),
	FOREIGN KEY(IdAnoEscolar) REFERENCES pmate.AnoEscolar(id),
	FOREIGN KEY(AnoLetivo) REFERENCES pmate.AnoLetivo(AnoLetivo),
	FOREIGN KEY(idProjeto) REFERENCES pmate.Projeto(id),
	CONSTRAINT user_escola_projeto_anolet_escolar UNIQUE (IdUser,IdProjeto,IdEscola,AnoLetivo,IdAnoEscolar)
);


CREATE TABLE pmate.UserEscolaHistorico (
	id int IDENTITY(1,1) PRIMARY KEY, 
	IdUser nvarchar(450) NOT NULL,
	IdEscola int NOT NULL,
	IdResponsavel int,
	IdAnoEscolar int,
	AnoLetivo  nvarchar(10),
	idProjeto int,
	data_ datetime

	FOREIGN KEY(IdUser) REFERENCES dbo.AspNetUsers(Id),
	FOREIGN KEY(IdEscola) REFERENCES pmate.Escola(id),
	FOREIGN KEY(IdResponsavel) REFERENCES pmate.UserEscola(id),
	FOREIGN KEY(IdAnoEscolar) REFERENCES pmate.AnoEscolar(id),
	FOREIGN KEY(AnoLetivo) REFERENCES pmate.AnoLetivo(AnoLetivo),
	FOREIGN KEY(idProjeto) REFERENCES pmate.Projeto(id),
);







------------------------------------ MODELS  RELATED -----------------------------------

CREATE TABLE pmate.Modelo(
    id int PRIMARY KEY,
    Tipo nvarchar(10),
);

CREATE TABLE pmate.ModeloNovo(
	IdModel int PRIMARY KEY,
	Question nvarchar(max) NULL,
	Id_ModeLevel int NOT NULL,
	IdModelType int NOT NULL,
	Id_Tree int NOT NULL,
	AnswersNumber smallint NULL,
	Id_Cycle int NULL,
	Id_ModelVersion int NULL,
	Status bit NULL,
	XML xml NULL,
	Id_User int NULL,
	Obs nvarchar(100) NULL,

	FOREIGN KEY (IdModel) REFERENCES pmate.Modelo(id),
); 

CREATE TABLE pmate.ModeloVelho(
	IdModel int PRIMARY KEY, -- OLD: ModelId
	Name nvarchar(15) NULL,
	Objectives ntext NULL,
	Question ntext NULL,
	Solution nvarchar(255) NULL,
	Restrictions nvarchar(255) NULL,
	Obs nvarchar(50) NULL,
	NumeroRespostas int NULL,
	DataModificado datetime NULL,
	ModificadoPor nvarchar(50) NULL,
	Letras nchar(30) NULL,
	Tipo int NULL,
	cCicloEnsino int NULL,
	cNivelDificuldade int NULL,
	cContador int NULL,
	cResponsavel int NULL,
	cDataElaboracao datetime NULL,
	cInformacaoAdicional ntext NULL,

	FOREIGN KEY (IdModel) REFERENCES pmate.Modelo(id),
);


CREATE TABLE pmate.ProvaModelos(
	IdProva int NOT NULL, --OLD: refIdComp
	IdModelo int NOT NULL, --OLD: refModelId
	Nivel int NOT NULL,

	PRIMARY KEY (IdProva,IdModelo,Nivel),

	FOREIGN KEY (IdProva) REFERENCES pmate.Prova(id),
	FOREIGN KEY (IdModelo) REFERENCES pmate.Modelo(id),
);

CREATE TABLE pmate.TreinoModelos(
	IdTreino int NOT NULL, --OLD: refIdComp
	IdModelo int NOT NULL, --OLD: refModelId
	Nivel int NOT NULL,

	PRIMARY KEY (IdTreino,IdModelo,Nivel),

	FOREIGN KEY (IdTreino) REFERENCES pmate.Treino(id),
	FOREIGN KEY (IdModelo) REFERENCES pmate.Modelo(id),
);




------------------------------------ EXAMS  RELATED -----------------------------------

CREATE TABLE pmate.Competicao(        -- OLD dbo.tblcompeticaoEvento
	id int IDENTITY(1,1) PRIMARY KEY, -- OLD IdCompeticaoEvento
	Nome nvarchar(max) NOT NULL, 	  -- OLD NomeEvento
	DataInicio datetime NOT NULL,
	DataFim datetime NULL, 			  -- OLD Datafim
	Etiqueta nvarchar(50) NOT NULL
) 

CREATE TABLE pmate.Prova(
	id int IDENTITY(1,1) PRIMARY KEY, -- OLD IdCompeticao
	IdAuthor nvarchar(450) NULL, 				  -- OLD IdUser
	-- IdModoProva int NULL           -- OLD IdModoCompeticao & De onde vem este ID?
	IdCompeticao int NULL,            -- OLD idCompeticaoEvento

	NomeProva char(60) NULL,          -- OLD NomeCompeticao
	DataCriacao datetime NULL,
	MaxEscolas int NULL,              -- OLD NumaMaxEqEscolas
	MaxTentJogo int NULL,
	TempoTotalJogo int NULL,
	NumNiveis int NOT NULL,           -- OLD NumTotNiveis
	VidasPorNivel int NULL,
	NumElemsEquipa int NULL,          -- OLD NumEltosEquipa
	Calculadora bit NULL,
	-- VisualizarProva bit NULL, TODAS AS ROWS  a NULL -- Nunca foi usado.

	DataInscFinal datetime NULL,
	DataProva datetime NULL,          -- OLD DiaHoraCompeticao
	InicioPreInscricao datetime NULL,
	FimPreInscricao datetime NULL,
	InicioInscricaoEquipas datetime NULL,
	FimInscricaoEquipas datetime NULL,
	FimProva datetime NULL,

	Estilo nvarchar(25) NULL,
	URL nvarchar(250) NULL,
	TreinoVisivel bit NOT NULL,
	RefIdCicloEnsino int NULL,
	plataforma int NULL, 
	-- IconName nvarchar(25) NULL, ESTA NULL EM 1870/2018 ROWS

	FOREIGN KEY(IdCompeticao) REFERENCES pmate.Competicao(id),
	FOREIGN KEY(IdAuthor)     REFERENCES dbo.AspNetUsers(id),

	-- Possivelmente adicionar : IdCategoria int, FOREIGN KEY(IdCategoria) REFERENCES pmate.Categoria(id),

);

CREATE TABLE pmate.Categoria(             -- OLD dbo.tblCompeticaoBase(
	id int IDENTITY(1,1) PRIMARY KEY,     -- OLD competicaoBaseId
	nome nvarchar(50) NOT NULL,           -- OLD designacao
	-- competicaoBasePaiId int NULL,      -- Apenas 15/35 rows não estão Null. -- Pq q CompeticaoBase tem id para um 'BasePai'?
	TempoTotalJogo int NULL,
	NumTotNiveis int NOT NULL,
	-- Calculadora bit NULL,              -- Qual a utilidade de ter isto aqui se isto já é definido em cada Prova individualmente ?
	Estilo nvarchar(25) NULL,
	RefIdCicloEnsino int NULL,
	IconName nvarchar(25) NULL
)
GO



CREATE TABLE [pmate2-demo].pmate.ProvaEscolas(  -- OLD: dbo.tblinsccompescola
	--IdInscricao int IDENTITY(1,1) PRIMARY KEY, -- Old: idInscCompEscola
	IdEscola int NOT NULL, 					   -- Old: idEscola
	IdProva int NOT NULL,                      -- Old: idProva
	IdUserEscola int NULL,                     -- Que user é este e para que serve? -- Old: idUserEscola

	DataRegisto datetime2(3) NOT NULL,
	EscolaOrganizadora int NULL,			   -- OLD: refescolaorganizadora
	AnoLetivo nvarchar(10) NULL, 			   -- OLD: refanolectivo
	

	PRIMARY KEY (idEscola,idProva),

	FOREIGN KEY(idEscola)  REFERENCES  [pmate2-demo].pmate.Escola(id),
	FOREIGN KEY(idProva)   REFERENCES  [pmate2-demo].pmate.Prova(id),
	FOREIGN KEY(EscolaOrganizadora) REFERENCES  [pmate2-demo].pmate.Escola(id),
	FOREIGN KEY(AnoLetivo) REFERENCES  [pmate2-demo].pmate.AnoLetivo(AnoLetivo)
);



CREATE TABLE pmate.Equipa( -- OLD: tblequipas
	id int IDENTITY(1,1) PRIMARY KEY,
	nome VARCHAR(50),
	DataCriacao Date,
	IdEscola int,

	FOREIGN KEY (IdEscola) REFERENCES pmate.Escola(id),
);


CREATE TABLE pmate.EquipaAlunos( -- OLD dbo.tblalunosequipas
	IdAlunoEquipa int  IDENTITY(1,1) PRIMARY KEY , 
	IdEquipa int,
	IdUser nvarchar(450), 
	-- iduser_pk int NOT NULL, provavelmente inutil
	-- idequipa_pk int NOT NULL, provavelmente inutil 

	CONSTRAINT equipa_user UNIQUE(IdEquipa,IdUser),
	FOREIGN KEY(IdEquipa) REFERENCES pmate.Equipa(id),
	FOREIGN KEY(IdUser)  REFERENCES dbo.AspNetUsers(id)
);



CREATE TABLE pmate.ProvaModelos( -- OLD dbo.tblcompmodel 
	idModelo nvarchar(15) NOT NULL,
	idProva int NOT NULL,
	Nivel int NOT NULL,

	PRIMARY KEY(idModelo,idProva),
	FOREIGN KEY(idModelo) REFERENCES pmate.Modelo(id),
	FOREIGN KEY(idProva) REFERENCES pmate.Prova(id)
);


CREATE TABLE pmate.EquipaProva( -- OLD:  tblequipascomp
	IdEquipa int NOT NULL,
	IdProva int NOT NULL,

	PRIMARY KEY(IdProva,IdEquipa),
	FOREIGN KEY (IdEquipa) REFERENCES pmate.Equipa(id),
	FOREIGN KEY(IdProva) REFERENCES pmate.Prova(id),
);

CREATE TABLE pmate.ProvaEquipaEnunciado( -- OLD JogoGerado
	id int IDENTITY(1,1) PRIMARY KEY, -- OLD : IdJogo 
	IdEquipa int NOT NULL,
	IdProva int NOT NULL, -- OLD IdCOmpet

	_Data datetime NOT NULL,
	_Status int ,
	ultimoNivel int,
	tempo char(10),

	FOREIGN KEY(IdProva) REFERENCES pmate.Prova(id),
	FOREIGN KEY(IdEquipa) REFERENCES pmate.Equipa(id),
)

CREATE TABLE pmate.ProvaEqEnunNivel( 
	-- FALTAM AQUI ATTRS!!!!!!!!!!!!!
	id int IDENTITY(1,1) PRIMARY KEY,
	IdEnunciadoEquipa int,

	FOREIGN KEY(IdEnunciadoEquipa) REFERENCES pmate.ProvaEquipaEnunciado(id), 
);

CREATE TABLE pmate.ProvaEqEnunNivelUserResp( 
	 -- FALTAM AQUI ATTRS!!!!!!!!!!!!!!!!
	id int IDENTITY(1,1) PRIMARY KEY,
	IdNivel int,

	FOREIGN KEY(IdEnunciadoEquipaNivel) REFERENCES pmate.ProvaEqEnunNivel(id),
);

CREATE TABLE pmate.ProvaEqEnunNivelResps(  
	-- FALTAM AQUI ATTRS!!!!!!!!!!!!!!!!
	id int IDENTITY(1,1) PRIMARY KEY,
	IdNivel int,

	FOREIGN KEY(IdEnunciadoEquipaNivel) REFERENCES pmate.ProvaEqEnunNivel(id),
);









------------------------------------ TRAINNING_TESTS  RELATED -----------------------------------

CREATE TABLE pmate.Treino(
	id int IDENTITY(1,1) PRIMARY KEY, -- OLD IdCompeticao
	IdAuthor nvarchar(450) NULL, 				  -- OLD IdUser
	-- IdModoProva int NULL           -- OLD IdModoCompeticao & De onde vem este ID?

	NomeProva char(60) NULL,          -- OLD NomeCompeticao
	DataCriacao datetime NULL,
	MaxEscolas int NULL,              -- OLD NumaMaxEqEscolas
	MaxTentJogo int NULL,
	TempoTotalJogo int NULL,
	NumNiveis int NOT NULL,           -- OLD NumTotNiveis
	VidasPorNivel int NULL,
	NumElemsEquipa int NULL,          -- OLD NumEltosEquipa
	Calculadora bit NULL,
	-- VisualizarProva bit NULL, TODAS AS ROWS a NULL -- Nunca foi usado.

	Estilo nvarchar(25) NULL,
	URL nvarchar(250) NULL,
	TreinoVisivel bit NOT NULL,
	RefIdCicloEnsino int NULL,
	plataforma int NULL, 
	-- IconName nvarchar(25) NULL, ESTA NULL EM 1870/2018 ROWS

	FOREIGN KEY(IdAuthor)     REFERENCES dbo.AspNetUsers(id),

	-- Possivelmente adicionar : IdCategoria int, FOREIGN KEY(IdCategoria) REFERENCES pmate.Categoria(id),
);

CREATE TABLE pmate.TreinoModelos( -- OLD dbo.tblcompmodel 
	IdModelo nvarchar(15) NOT NULL,
	IdTreino int NOT NULL,
	Nivel int NOT NULL,
	PRIMARY KEY(idTreino,idModelo),
	FOREIGN KEY(idModelo) REFERENCES pmate.Modelo(id),
	FOREIGN KEY(idTreino) REFERENCES pmate.Treino(id),
);

CREATE TABLE pmate.TreinoEnunciado( -- Antigo JogoGerado PRECISA DE MODIFICA�OES!!!
	id int IDENTITY(1,1) PRIMARY KEY,  -- OLD: IdJogo
	-- IdJogoGeradoAnterior int,       -- Provavelmente inutil. a tabela de jogos 2020_2021 n tem este attr       
	IdUser nvarchar(450) NOT NULL,     -- OLD: IdEquipa - > É para tentar ir buscar o id do aluno que fez o treino. Treinos são indivuais portanto equipa é desnecessário
	IdTreino int NOT NULL,           -- OLD: IdCOmpet
	

	_Data datetime NOT NULL,        -- OLD: Data
	_Status int ,                   -- OLD: Status
	ultimoNivel int,
	tempo char(10),

	FOREIGN KEY(IdTreino) REFERENCES pmate.Treino(id),
	FOREIGN KEY(IdUser) REFERENCES dbo.AspNetUsers(id),
)



CREATE TABLE pmate.TreinoEnunNivel( -- FALTAM AQUI ATTRS!!!!!!!!!!!!!
	id int IDENTITY(1,1) PRIMARY KEY,
	IdEnunciadoEquipa int,

	FOREIGN KEY(IdEnunciadoEquipa) REFERENCES pmate.EnunciadoEquipa(id), 
);

CREATE TABLE pmate.TreinoEnunNivelUserResp(  -- FALTAM AQUI ATTRS!!!!!!!!!!!!!!!!
	id int IDENTITY(1,1) PRIMARY KEY,
	IdEnunciadoEquipaNivel int,

	FOREIGN KEY(IdEnunciadoEquipaNivel) REFERENCES pmate.EnunciadoEquipaNivel(id),
);

CREATE TABLE pmate.TreinoEnunNivelResps(  -- FALTAM AQUI ATTRS!!!!!!!!!!!!!!!!
	id int IDENTITY(1,1) PRIMARY KEY,
	IdEnunciadoEquipaNivel int,

	FOREIGN KEY(IdEnunciadoEquipaNivel) REFERENCES pmate.EnunciadoEquipaNivel(id),
);





-- SELECTS 

-- DROP TABLES 

------------------------------------ EXAMS  RELATED -----------------------------------
DROP TABLE pmate.ProvaEqEnunNivelResps;
DROP TABLE pmate.ProvaEqEnunNivelUserResp;
DROP TABLE pmate.ProvaEqEnunNiveis;
DROP TABLE pmate.ProvaEquipaEnunciado;
DROP TABLE pmate.ProvaModelos;
DROP TABLE pmate.EquipaAlunos;
DROP TABLE pmate.EquipaProva
DROP TABLE pmate.Equipa;
DROP TABLE pmate.ProvaEscolas;
DROP TABLE pmate.Prova
DROP TABLE pmate.Categoria;
DROP TABLE pmate.Competicao;

------------------------------------ TRAINNING_TESTS  RELATED -----------------------------------
DROP TABLE pmate.TreinoEnunNivelResps;
DROP TABLE pmate.TreinoEnunNivelUserResp;
DROP TABLE pmate.TreinoEnunNiveis;
DROP TABLE pmate.TreinoEnunciado;
DROP TABLE pmate.TreinoModelo;
DROP TABLE pmate.Treino;


------------------------------------ MODELS  RELATED -----------------------------------
DROP TABLE pmate.Modelo;

------------------------------------ SCHOOL RELATED -----------------------------------

DROP TABLE pmate.EscolaInfoExtra;
DROP TABLE pmate.Projeto;
DROP TABLE pmate.UserEscolaHistorico;
DROP TABLE pmate.UserEscola;
DROP TABLE pmate.Escola;
DROP TABLE pmate.AnoLetivo;
DROP TABLE pmate.AnoEscolar;
DROP TABLE pmate.TipoEscola;


------------------------------------ USER RELATED -----------------------------------
DROP TABLE pmate.Users;
DROP TABLE pmate.UserContacto;
DROP TABLE pmate.UserContactoTipo;

------------------------------------ LOCATIONS RELATED -----------------------------------
DROP TABLE pmate.Freguesia;
DROP TABLE pmate.Concelho;
DROP TABLE pmate.Distrito;
DROP TABLE pmate.Pais;
