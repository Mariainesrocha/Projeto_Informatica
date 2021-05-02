-- TRANSACTION TEMPLATE 
BEGIN TRANSACTION;
SET IDENTITY_INSERT pmate.NewTable ON

INSERT INTO pmate.NewTable(new_attrs)
SELECT oldAttrs
FROM dbo.OldTable

SET IDENTITY_INSERT pmate.NewTable OFF
COMMIT;


---------------------------------------  IMPORTANTE  ----------------------------------------
-- !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!  PLANEAMENTO !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! --

--Tabelas mais importantes para tratar e usar no ASP NET CORE:
    ---> AspNetUsers / AspNetRoles / AspNetUserRoles
    ---> Users (tabela que tem os attrs extra) 
    ---> TipoEscola / Escola / AnoLetivo / AnoEscolaridade / UserRoleEscola
    ---> Prova / Competicao / Modelos / ProvaModelos

-- OBJETIVOS ATUAIS:
    -- INICIAIS
    --> Listar Escolas / Users / Provas / Listar Treinos (Inicialmente sem filtros sabendo fazer 1, os outros é replicar)
    --> Abrir uma Prova 
    --> Criar uma competição
    --> Criar 1 prova dentro ou fora de 1 competição 
    --> Inscrever Escolas nas Provas/Competiçoes (DOUBT - Ao que parece na BD atual ainda não temos a tabela de EscolaProva -> Escolas inscritas numa prova..!?)

    -- MAIS AVANÇADOS
    --> Filtrar Provas por Tema e outros 
    --> Filtrar Users por Roles
    --> Abrir o Perfil de um User

-- EM STANDBY - Podemos acrescentar isto + tarde :
    --> tbls de localizações (pais, distrito etc) 
    --> tbls com  + detalhes relativos á prova (Enunciados, Niveis,  respostas.. etc) 
    --> tbls com Equipas que partipam nas provas

--------------------------------------- END/IMPORTANTE ----------------------------------------




------------------------------------ LOCATIONS RELATED ------------------------------



-- Paises
BEGIN TRANSACTION;
SET IDENTITY_INSERT pmate.Pais ON

INSERT INTO pmate.Pais(id,nome)
SELECT IdPais,NomePais
FROM dbo.tblpais

INSERT INTO pmate.Pais (id,nome) VALUES (4,'Outros Países');

SET IDENTITY_INSERT pmate.Pais OFF
COMMIT;

select * from tblpais
select * FROM pmate.Pais order by id
-- select * FROM [pmate-Equamat2000].[pmate].[Pais] order by id
GO



--Distritos 
BEGIN TRANSACTION;
SET IDENTITY_INSERT pmate.Distrito ON

INSERT INTO pmate.Distrito(id,nome)
SELECT  IdDistrito, NomeDistrito
FROM dbo.tbldistritos

UPDATE pmate.Distrito
SET pais = 1
WHERE id < 21;

UPDATE pmate.Distrito
SET pais = 4
WHERE id > 20;

SET IDENTITY_INSERT pmate.Distrito OFF
COMMIT;

select * from tbldistritos
select * FROM pmate.Distrito order by id
GO



-- Concelhos
-- 1.Bulk insert na temporary_table com as cenas do csv que tem DISTRITO - CONCELHO - FREGUESIA
-- 2.Fazer um Join pelos nomes dos concelhos:  temp_table com tblconcelhos.. Teremos : ID, Distrito, Concelho , FREGUESIA 
-- 3.Fazer INSERT na tabela pmate.Concelhos. id -> ID , nome -> Concelho, Distrito(FK) -> Distrito
BEGIN TRANSACTION;
SET IDENTITY_INSERT pmate.Concelho ON

 ----- 1 -----
CREATE TABLE ConcelhosTemp(
   code1 varchar(300),
   distrito varchar(300),
   code2 varchar(300),
   concelho varchar(300),
   code3 varchar(300),
   freguesia varchar(300),
   code4 varchar(300),
   -- some_weird_type varchar(300)
);

BULK INSERT ConcelhosTemp
    FROM 'C:\Users\Lenovo\Desktop\PI\DistritosConcelhosFreguesias_CAOP2013_Populacao_Censos2011.csv'
    WITH
    (
	CODEPAGE = 'OEM',
	DATAFILETYPE ='char',
	FIRSTROW = 2,          --Ignorar cabeçalho do file
    FIELDTERMINATOR = ';',  --CSV field delimiter
    ROWTERMINATOR = ';;',   --Use to shift the control to next row
    ERRORFILE = 'C:\Users\Lenovo\Desktop\PI\BulkInsertErrors.csv',
    TABLOCK )

 ----- 2 & 3 -----
INSERT INTO pmate.Concelho(id,nome,distrito)
SELECT  idConcelho,NomeConcelho,distrito
FROM tblconcelhos INNER JOIN ConcelhosTemp ON NomeConcelho=concelho;

SET IDENTITY_INSERT pmate.Concelho OFF
COMMIT;

select * from ConcelhosTemp;
select * from pmate.concelho;
select * from tblconcelhos;



-- Freguesias -> Estrategia igual ao dos Concelhos
BEGIN TRANSACTION;
SET IDENTITY_INSERT pmate.Freguesia ON

SET IDENTITY_INSERT pmate.Freguesia OFF
COMMIT;
GO









------------------------------------ USER RELATED -----------------------------------

-- Select from tblUsers and AspNetUsers --
Select * 
from [pmate-Equamat2000].dbo.tblUsers
where Login=':)'

SELECT *
from [pmate2-demo].dbo.AspNetUsers
where UserName='AS'
------------------------------------------

-- Delete users from AspNetUsers --
Delete
from [pmate2-demo].dbo.AspNetUsers
where Id is not NULL
-----------------------------------



    
-- Fix tblUsers before Migration --
Update [pmate].dbo.tblUsers
set Nome = 'Empty'
where Nome is null
-----------------------------------

------------------------------------------------

-- Scalar-valued Function to Convert Sha512 to Base64 --
USE [pmate2-demo]
GO

/****** Object:  UserDefinedFunction [dbo].[encodebase64]    Script Date: 01/05/2021 21:18:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[encodebase64](@hashedValue VARBINARY(8000)) 
RETURNS NVARCHAR(MAX)
BEGIN
	Declare @converted NVARCHAR(MAX)

	Select @converted = CAST(N'' AS XML).value(
          'xs:base64Binary(xs:hexBinary(sql:variable("@hashedValue")))'
        , 'NVARCHAR(MAX)'
    )
 Return @converted
END 
GO
----------------------------------------------------------

-- Migrate Users from tblUsers to AspNetUsers --
BEGIN TRANSACTION;

INSERT INTO [pmate2-demo].dbo.AspNetUsers(Id,UserName,PasswordHash, EmailConfirmed, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount, Age, Name, Roles, UserId) 
SELECT IdUser,Login, [pmate2-demo].dbo.encodebase64(HASHBYTES('SHA2_512',cast(Password as varchar(max)))), 0, 0, 0, 0, 0, 0, Left(Nome,80), 0, 0
FROM [pmate].dbo.tblUsers

COMMIT;



-- Fix AspNetUsers after Migration --
Update [pmate2-demo].dbo.AspNetUsers
Set EmailConfirmed=1, NormalizedUserName = Upper(UserName), SecurityStamp = 'abc'
-------------------------------------


--AspNetUsers 
--NOTA: Devido á encprytion de Passwords automatica do ASPNET CORE Talvez esta transaction não sejam aplicavel
--NOTA: Falta confirmar os nomes dos attrs desta tbl do NetCore
BEGIN TRANSACTION;
SET IDENTITY_INSERT pmate.AspNetUsers ON

INSERT INTO pmate.AspNetUsers(IdUser,Username,Password, QuestionPass,AnswerPass) 
SELECT IdUser,Username,Password, QuestionPass,AnswerPass
FROM dbo.tblUsers   

SET IDENTITY_INSERT pmate.AspNetUsers OFF
COMMIT;




-- Users
BEGIN TRANSACTION;
SET IDENTITY_INSERT pmate.Users ON

INSERT INTO pmate.Users(IdUser,Nome,DataDeRegisto,Morada,CodPostal,ExtensaoCodPostal,Localidade,idsexo,idNEE,iduser_pk,editado,obs,rgpd_comunicacao,rgpd_estatistica,rgpd_data,validade_pessoal,validade_estatistica) 
SELECT IdUser,Nome,DataDeRegisto,Morada,CodPostal,ExtensaoCodPostal,Localidade,idsexo,idNEE,iduser_pk,editado,obs,rgpd_comunicacao,rgpd_estatistica,rgpd_data,validade_pessoal,validade_estatistica
FROM dbo.tblUsers   

SET IDENTITY_INSERT pmate.Users OFF
COMMIT;




-- UserContactoTipo
BEGIN TRANSACTION;
SET IDENTITY_INSERT pmate.UserContactoTipo ON

INSERT INTO pmate.UserContactoTipo(id,tipo) 
SELECT idDicionario, Descricao
FROM dbo.tbldicionariocontacto

SET IDENTITY_INSERT pmate.UserContactoTipo OFF
COMMIT;



-- UserContacto
BEGIN TRANSACTION;
SET IDENTITY_INSERT pmate.UserContacto ON

INSERT INTO pmate.UserContacto(id,idUser,idTipo,Descricao) 
SELECT idContacto,idUser,idDic,Descricao
FROM dbo.tblcontacto   

SET IDENTITY_INSERT pmate.UserContacto OFF
COMMIT;






------------------------------------ SCHOOL RELATED ---------------------------------

--TipoEscola
BEGIN TRANSACTION;
SET IDENTITY_INSERT pmate.TipoEscola ON

INSERT INTO pmate.TipoEscola(id_tipo_escola, TipoEscola, DescricaoTipoEscola)
SELECT IdTipoEscola, TipoEscola, DescricaoTipoEscola
FROM dbo.tbltipoescola

SET IDENTITY_INSERT pmate.TipoEscola OFF
COMMIT;



--Escola
BEGIN TRANSACTION;
SET IDENTITY_INSERT pmate.Escola ON

INSERT INTO pmate.Escola(id,IdTipoEscola,NomeEscola,Morada,CodigoPostal,ExtensaoCodPostal,Localidade,Telefone,Fax,Email,Website,Idconcelho,IdFreguesia,estado,COD_DGEEC,COD_DGPGF,ENSINOS,GRUPONATUREZA,LATITUDE,LONGITUDE)
SELECT IdEscola,IdTipoEscola, NomeEscola, Morada, CodigoPostal, ExtensaoCodPostal, Localidade, Telefone, Fax, Email, Http, refidconcelho, refIdFreguesia, estado, COD_DGEEC, COD_DGPGF, ENSINOS, GRUPONATUREZA, LATITUDE, LONGITUDE
FROM dbo.tblescolas

SET IDENTITY_INSERT pmate.Escola OFF
COMMIT;



--EscolaInfoExtra - > pode ser eventualmente necessário  caso os attrs quase n utilizados/inuteis sejam realmente pra manter
BEGIN TRANSACTION;
INSERT INTO pmate.EscolaInfoExtra(new_attrs)
SELECT oldAttrs  -- NOT DONE YET
FROM dbo.tblescolas
COMMIT;



--AnoLetivo
BEGIN TRANSACTION;
SET IDENTITY_INSERT pmate.AnoLetivo ON

INSERT INTO pmate.AnoLetivo(AnoLetivo,Inicio,Fim)
SELECT AnoLectivo, Inicio, Fim
FROM dbo.dicanolectivo

SET IDENTITY_INSERT pmate.AnoLetivo OFF
COMMIT;



--AnoEscolar
BEGIN TRANSACTION;
SET IDENTITY_INSERT pmate.AnoEscolar ON

INSERT INTO pmate.AnoEscolar(id,Ano)
SELECT idanoescolariedade, descri��o
FROM dbo.tblDicAnoEscolaridade

SET IDENTITY_INSERT pmate.AnoEscolar OFF
COMMIT;




-- AspNetRoles 
-- NOTA : Fazer Insert das ROLES : 1.ADMIN, 2.PROFESSOR E 3.ALUNO



--AspNetUserRoles -> ADMINS
--NOTA: Falta confirmar os nomes dos attrs desta tbl do NetCore
BEGIN TRANSACTION;
SET IDENTITY_INSERT pmate.AspNetUserRole ON

ALTER TABLE dbo.tblAdm 
ADD _role int;

UPDATE dbo.tblAdm
SET _role = 1 -- should be admin's role id 
WHERE _role is NULL

INSERT INTO pmate.AspNetUserRole(IdUser,_role)
SELECT IdUser, _role
FROM dbo.tblAdm

SET IDENTITY_INSERT pmate.AspNetUserRole OFF
COMMIT;



--AspNetUserRoles -> PROFESSORES
BEGIN TRANSACTION;
SET IDENTITY_INSERT pmate.AspNetUserRole ON

ALTER TABLE dbo.tblprofessores 
ADD _role int;

UPDATE dbo.tblprofessores 
SET _role = 2 -- should be professor's role id 
WHERE _role is NULL

INSERT INTO pmate.AspNetUserRole(IdUser,_role)
SELECT IdUser, _role
FROM dbo.tblprofessores 

SET IDENTITY_INSERT pmate.AspNetUserRole OFF
COMMIT;



--AspNetUserRoles -> ALUNOS
BEGIN TRANSACTION;
SET IDENTITY_INSERT pmate.AspNetUserRole ON

ALTER TABLE dbo.tblalunos 
ADD _role int;

UPDATE dbo.tblalunos
SET _role = 3 -- should be aluno's role id 
WHERE _role is NULL

INSERT INTO pmate.AspNetUserRole(IdUser,_role)
SELECT IdUser, _role
FROM dbo.tblalunos

SET IDENTITY_INSERT pmate.AspNetUserRole OFF
COMMIT;



-- UserRoleEscola
BEGIN TRANSACTION;

INSERT INTO pmate.UserRoleEscola(IdUser,IdRole,IdEscola,IdAnoEscolar,AnoLetivo)
SELECT IdUser, IdRole,RefIdEscola,AnoEscolaridade,RefAnoLectivo
FROM (pmate.AspNetUserRoles 
    INNER JOIN dbo.tblUsers ON pmate.AspNetUserRoles.IdUser = dbo.tblUsers.IdUser)
    INNER JOIN  (select RefIdUser, max(Data),RefAnoLectivo,AnoEscolaridade from tbluserescola group by RefIdUser) ON IdUser=RefIdUser -- max->para guardar apenas a escola mais recente... as restantes podem ser guardadas na outra tabela de historico mais tarde


COMMIT;



INSERT INTO pmate.Concelho(id,nome,distrito)
SELECT  idConcelho,NomeConcelho,distrito
FROM tblconcelhos INNER JOIN ConcelhosTemp ON NomeConcelho=concelho;

------------------------------------ MODELS  RELATED ---------------------------------

--Modelo
BEGIN TRANSACTION;
SET IDENTITY_INSERT pmate.Modelo ON

INSERT INTO pmate.Modelo(id, refCicloEnsinoID, Expr1, SubTema, Objectivo_Principal, Objectivo_Secundario, Nivel_Dificuldade, contador)
SELECT Name, refCicloEnsinoID, Expr1, SubTema, [Objectivo Principal], [Objectivo Secund�rio], [N�vel Dificuldade], contador
FROM dbo.rv_CodigoModelo

SET IDENTITY_INSERT pmate.Modelo OFF
COMMIT;








------------------------------------ EXAMS  RELATED -----------------------------------

--Categoria
BEGIN TRANSACTION;
SET IDENTITY_INSERT pmate.Categoria ON

INSERT INTO pmate.Categoria(id, nome, TempoTotalJogo, NumTotNiveis, Estilo, RefIdCicloEnsino, IconName)
SELECT competicaoBaseId, designacao, TempoTotalJogo, NumTotNiveis, Estilo, RefIdCicloEnsino, IconName
FROM dbo.tblCompeticaoBase

SET IDENTITY_INSERT pmate.Categoria OFF
COMMIT;



--Competicao
BEGIN TRANSACTION;
SET IDENTITY_INSERT pmate.Competicao ON

INSERT INTO pmate.Competicao(id, Nome, DataInicio, DataFim, Etiqueta)
SELECT IdCompeticaoEvento, NomeEvento, DataInicio, Datafim, Etiqueta
FROM dbo.tblcompeticaoEvento

SET IDENTITY_INSERT pmate.Competicao OFF
COMMIT;



--Prova
BEGIN TRANSACTION;
SET IDENTITY_INSERT pmate.Prova ON

INSERT INTO pmate.Prova(id,IdAuthor, IdCompeticao, NomeProva, DataCriacao, MaxEscolas, MaxTentJogo, TempoTotalJogo, NumNiveis, VidasPorNivel, NumElemsEquipa, Calculadora, DataInscFinal, DataProva, InicioPreInscricao, FimPreInscricao, InicioInscricaoEquipas, FimInscricaoEquipas, FimProva, Estilo, URL, TreinoVisivel, RefIdCicloEnsino, plataforma)
SELECT IdCompeticao, IdUser, idCompeticaoEvento, NomeCompeticao, DataCriacao, NumaMaxEqEscolas, MaxTentJogo, TempoTotalJogo, NumTotNiveis, VidasPorNivel, NumEltosEquipa, Calculadora, DataInscFinal, DiaHoraCompeticao, InicioPreInscricao, FimPreInscricao, InicioInscricaoEquipas, FimInscricaoEquipas, FimProva, Estilo, URL, TreinoVisivel, RefIdCicloEnsino, plataforma
FROM dbo.tblcompeticao

SET IDENTITY_INSERT pmate.Prova OFF
COMMIT;


------------------------------------ TRAINNING_TESTS  RELATED -----------------------------------



