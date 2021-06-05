-- TRANSACTION TEMPLATE 
BEGIN TRANSACTION;
SET IDENTITY_INSERT pmate.NewTable ON

INSERT INTO pmate.NewTable(new_attrs)
SELECT oldAttrs
FROM dbo.OldTable

SET IDENTITY_INSERT pmate.NewTable OFF
COMMIT;

-- SubProvas
BEGIN TRANSACTION;

INSERT INTO pmate.SubProvas(IdProvaPai,IdProvaFilho)
SELECT  distinct refidcompeticao_pai, refidcompeticao_filho
FROM  [pmate-Equamat2000].dbo.tblsubcompeticoes
JOIN pmate.Prova on pmate.Prova.id = [pmate-Equamat2000].dbo.tblsubcompeticoes.ref

DELETE FROM pmate.SubProvas
where IdProvaPai=IdProvaFilho

COMMIT;



------------------------------------ LOCATIONS RELATED ------------------------------



-- Paises
BEGIN TRANSACTION;
SET IDENTITY_INSERT pmate.Pais ON

INSERT INTO pmate.Pais(id,nome)
SELECT IdPais,NomePais
FROM [pmate].dbo.tblpais

INSERT INTO pmate.Pais (id,nome) VALUES (4,'Outros Países');

SET IDENTITY_INSERT pmate.Pais OFF
COMMIT;

select * from  [pmate].dbo.tblpais
select * FROM pmate.Pais order by id
-- select * FROM [pmate-Equamat2000].[pmate].[Pais] order by id
GO



--Distritos 
BEGIN TRANSACTION;
SET IDENTITY_INSERT pmate.Distrito ON

INSERT INTO pmate.Distrito(id,nome)
SELECT  IdDistrito, NomeDistrito
FROM [pmate].dbo.tbldistritos

UPDATE pmate.Distrito
SET pais = 1
WHERE id < 21;

UPDATE pmate.Distrito
SET pais = 4
WHERE id > 20;

SET IDENTITY_INSERT pmate.Distrito OFF
COMMIT;

select * from [pmate].dbo.tbldistritos
select * FROM pmate.Distrito order by id
GO





-- Concelhos
BEGIN TRANSACTION;
SET IDENTITY_INSERT pmate.Concelho ON

-- INSERT feito pelo Management Studio: Database -> rightclick -> Tasks -> Import Flat File - > Choose File -> "Magic" Done 

-- DELETE DUPE CONCELHO (BECAUSE THEY ARE SO SMART)
 UPDATE [pmate].dbo.tblescolas
 SET refidconcelho = 303
 WHERE refidconcelho = 313

 DELETE FROM [pmate].dbo.tblconcelhos
 WHERE idConcelho=313

 -- FIX CONCELHO NAME 
UPDATE [pmate].dbo.tblconcelhos
 SET NomeConcelho = 'Torre de Moncorvo'
 WHERE NomeConcelho = 'Torres de Moncorvo'

 ----- 2 & 3 -----
INSERT INTO pmate.Concelho(id,nome,distrito)
SELECT  distinct idConcelho,NomeConcelho,pmate.Distrito.id
FROM ([pmate].dbo.tblconcelhos LEFT JOIN [pmate].dbo.DistritoConcelhoFreguesia ON NomeConcelho=concelho COLLATE Latin1_general_CI_AI)
				   LEFT JOIN pmate.Distrito ON pmate.distrito.nome=[pmate].dbo.DistritoConcelhoFreguesia.distrito 
		
SET IDENTITY_INSERT pmate.Concelho OFF
COMMIT;




-- Freguesias -> Estrategia igual ao dos Concelhos
BEGIN TRANSACTION;
--select nomeFreguesia, count(nomeFreguesia) from tblfreguesias
--group by nomeFreguesia
--having count(nomeFreguesia) >1

 UPDATE [pmate].dbo.tblescolas
 SET refIdFreguesia = 107
 WHERE refIdFreguesia = 148

  UPDATE [pmate].dbo.tblescolas
 SET refIdFreguesia = 332
 WHERE refIdFreguesia = 333

 DELETE FROM [pmate].dbo.tblfreguesias
 WHERE idFreguesia= 148

 DELETE FROM [pmate].dbo.tblfreguesias
 WHERE idFreguesia= 333

 
 INSERT INTO pmate.Freguesia(nome,concelho)
SELECT  distinct freguesia,pmate.Concelho.id as concelho
FROM  [pmate].dbo.DistritoConcelhoFreguesia LEFT JOIN pmate.Concelho ON pmate.concelho.nome=[pmate].dbo.DistritoConcelhoFreguesia.concelho  COLLATE Latin1_general_CI_AI
COMMIT;
GO









------------------------------------ USER RELATED -----------------------------------

-- Select from tblUsers and AspNetUsers --
SELECT *
from [pmate2-demo].dbo.AspNetUsers
where UserName='AS'
------------------------------------------
-- Delete users from AspNetUsers --
Delete from [pmate2-demo].dbo.AspNetUsers
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


-- AspNetRoles 
-- Insert é Automatico ao correr a App: 1.ADMIN, 2.PROFESSOR E 3.ALUNO

--AspNetUserRoles ADMINS
BEGIN TRANSACTION;

INSERT INTO dbo.AspNetUserRoles(UserId,RoleId)
SELECT distinct IdUser, (Select Id from AspNetRoles where NormalizedName='ADMIN') 
FROM [pmate].dbo.tblAdm 

COMMIT;


--AspNetUserRoles PROFESSORES
BEGIN TRANSACTION;

INSERT INTO dbo.AspNetUserRoles(UserId,RoleId)
SELECT distinct IdUser, (Select Id from AspNetRoles where NormalizedName='PROFESSOR') 
FROM [pmate].dbo.tblprofessores  JOIN AspNetUsers ON IdUser=Id

COMMIT;


--AspNetUserRoles ALUNOS
BEGIN TRANSACTION;

INSERT INTO dbo.AspNetUserRoles(UserId,RoleId)
SELECT distinct  IdUser, (Select Id from AspNetRoles where NormalizedName='ALUNO') 
FROM [pmate].dbo.tblalunos JOIN AspNetUsers ON IdUser=Id

COMMIT;


-- After Doing External Authentication 
INSERT INTO [pmate2-demo].dbo.AspNetUserRoles(Userid,RoleId) 
VALUES ((Select Id from [pmate2-demo].dbo.AspNetUsers where Email='fabiospar99@gmail.com'),(Select Id from [pmate2-demo].dbo.AspNetRoles where NormalizedName='ADMIN'))
------------------------------------------------------------------------

-- UserContactoTipo
BEGIN TRANSACTION;

INSERT INTO pmate.UserContactoTipo(id,tipo) 
SELECT idDicionario, Descricao
FROM dbo.tbldicionariocontacto

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
--CicloEnsino
BEGIN TRANSACTION;

INSERT INTO pmate.CicloEnsino(id,Descritivo,Abreviatura)
SELECT CicloEnsinoID,Descritivo,Abreviatura 
FROM [pmate-Equamat2000].dbo.cicloensino

COMMIT;

--TipoEscola
BEGIN TRANSACTION;
SET IDENTITY_INSERT pmate.TipoEscola ON

INSERT INTO pmate.TipoEscola(id_tipo_escola, TipoEscola, DescricaoTipoEscola)
SELECT IdTipoEscola, TipoEscola, DescricaoTipoEscola
FROM [pmate].dbo.tbltipoescola

SET IDENTITY_INSERT pmate.TipoEscola OFF
COMMIT;



--Escola
BEGIN TRANSACTION;

UPDATE [pmate-Equamat2000].dbo.tblescolas 
SET refidconcelho=NULL
WHERE refidconcelho < 0

UPDATE [pmate-Equamat2000].dbo.tblescolas
SET refidconcelho=NULL
WHERE  refidconcelho > 314

SET IDENTITY_INSERT pmate.Escola ON

INSERT INTO pmate.Escola(id,IdTipoEscola,NomeEscola,Morada,CodigoPostal,ExtensaoCodPostal,Localidade,Telefone,Fax,Email,Website,Idconcelho,estado,COD_DGEEC,COD_DGPGF,ENSINOS,GRUPONATUREZA,LATITUDE,LONGITUDE)
SELECT IdEscola,IdTipoEscola, NomeEscola, Morada, CodigoPostal, ExtensaoCodPostal, Localidade, Telefone, Fax, Email, Http, refidconcelho, estado, COD_DGEEC, COD_DGPGF, ENSINOS, GRUPONATUREZA, LATITUDE, LONGITUDE
FROM [pmate-Equamat2000].dbo.tblescolas

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

INSERT INTO [pmate2-demo].pmate.AnoLetivo(AnoLetivo,Inicio,Fim)
SELECT AnoLectivo, Inicio, Fim
FROM [pmate-Equamat2000].dbo.dicanolectivo

COMMIT;



--AnoEscolar
BEGIN TRANSACTION;
SET IDENTITY_INSERT pmate.AnoEscolar ON

INSERT INTO pmate.AnoEscolar(id,Ano)
SELECT idanoescolariedade, descricao
FROM dbo.tblDicAnoEscolaridade

SET IDENTITY_INSERT pmate.AnoEscolar OFF
COMMIT;


--------------------------- Projeto
BEGIN TRANSACTION;

INSERT INTO pmate.Projeto(id, descricao, URL) 
SELECT idprojecto,descricao,URL
FROM [pmate-Equamat2000].dbo.tbldicprojectos

COMMIT;


---------------------------  User-Escola
BEGIN TRANSACTION;
SET IDENTITY_INSERT pmate.UserEscola OFF

INSERT INTO pmate.UserEscola(IdUser, IdEscola,AnoLetivo, idProjeto, IdAnoEscolar, data_) 
SELECT distinct [pmate-Equamat2000].dbo.tbluserescola.RefIdUser, RefIdEscola, RefAnoLectivo,Projecto,AnoEscolaridade,MaxDate
FROM [pmate-Equamat2000].dbo.tbluserescola 
JOIN dbo.AspNetUsers ON CAST(RefIdUser AS nvarchar) = Id  
JOIN pmate.Escola ON RefIdEscola= pmate.Escola.id
JOIN pmate.Projeto ON Projecto= pmate.Projeto.id
JOIN pmate.AnoEscolar ON AnoEscolaridade=pmate.AnoEscolar.id
JOIN pmate.AnoLetivo ON RefAnoLectivo=pmate.AnoLetivo.AnoLetivo
JOIN (
    select RefIdUser, max(Data) as MaxDate
    from  [pmate-Equamat2000].dbo.tbluserescola
    group by RefIdUser
) tm ON [pmate-Equamat2000].dbo.tbluserescola.RefIdUser = tm.RefIdUser and [pmate-Equamat2000].dbo.tbluserescola.Data = tm.MaxDate

COMMIT;



-------------- User-Escola Historico
BEGIN TRANSACTION;
SET IDENTITY_INSERT pmate.UserEscolaHistorico OFF

INSERT INTO pmate.UserEscolaHistorico(IdUser, IdEscola,AnoLetivo, idProjeto, IdAnoEscolar, data_) 
SELECT distinct [pmate-Equamat2000].dbo.tbluserescola.RefIdUser, RefIdEscola, RefAnoLectivo,Projecto,AnoEscolaridade,data
FROM [pmate-Equamat2000].dbo.tbluserescola 
JOIN dbo.AspNetUsers ON CAST(RefIdUser AS nvarchar) = Id  
JOIN pmate.Escola ON RefIdEscola= pmate.Escola.id
JOIN pmate.Projeto ON Projecto= pmate.Projeto.id
JOIN pmate.AnoEscolar ON AnoEscolaridade=pmate.AnoEscolar.id
JOIN pmate.AnoLetivo ON RefAnoLectivo=pmate.AnoLetivo.AnoLetivo

COMMIT;

-- NOTA: NO TRIGGER DO HISTORICO FAZER VERIFICACAO DE EXISTENCIA DESSE REGISTO






------------------------------------ MODELS  RELATED ---------------------------------

--Modelo
BEGIN TRANSACTION;

INSERT INTO pmate.Modelo(id,Tipo)
SELECT IdModel, 'NEW'
FROM [pmate-Equamat2000].dbo.Model

INSERT INTO pmate.Modelo(id,Tipo)
SELECT ModelID, 'OLD'
FROM [pmate-Equamat2000].dbo.ModelsMD

COMMIT;


--ModeloNovo
BEGIN TRANSACTION;

INSERT INTO pmate.ModeloNovo(IdModel,Question,Id_ModeLevel,IdModelType,Id_Tree,AnswersNumber,Id_Cycle,Id_ModelVersion,Status,XML,Id_User,Obs)
SELECT IdModel,Question,Id_ModeLevel,IdModelType,Id_Tree,AnswersNumber,Id_Cycle,Id_ModelVersion,Status,XML,Id_User,Obs
FROM [pmate-Equamat2000].dbo.Model

COMMIT;



--ModeloVelho
BEGIN TRANSACTION;

INSERT INTO pmate.ModeloVelho(IdModel,Name,Objectives,Question,Solution ,Restrictions ,Obs,NumeroRespostas,DataModificado ,ModificadoPor,Letras,Tipo,
cCicloEnsino,cNivelDificuldade,cContador,cResponsavel,cDataElaboracao,cInformacaoAdicional)

SELECT ModelId,Name,Objectives,Question,Solution ,Restrictions ,Obs,NumeroRespostas,DataModificado ,ModificadoPor,Letras,Tipo,
cCicloEnsino,cNivelDificuldade,cContador,cResponsavel,cDataElaboracao,cInformacaoAdicional
FROM [pmate-Equamat2000].dbo.ModelsMD

COMMIT;


--ProvaModelos
BEGIN TRANSACTION;

INSERT INTO pmate.ProvaModelos(IdProva,IdModelo,Nivel)
SELECT refIdComp,refModelId,Nivel
FROM [pmate-Equamat2000].dbo.tblcompmodel
join pmate.Prova on id=refIdComp

COMMIT;


--TreinoModelos
BEGIN TRANSACTION;

INSERT INTO pmate.TreinoModelos(IdTreino,IdModelo,Nivel)
SELECT refIdComp,refModelId,Nivel
FROM [pmate-Equamat2000].dbo.tblcompmodel
join pmate.Treino on id=refIdComp

COMMIT;

------------------------------------ EXAMS  RELATED -----------------------------------
--CicloEnsino
BEGIN TRANSACTION;

INSERT INTO pmate.CicloEnsino(id,Descritivo,Abreviatura)
SELECT CicloEnsinoID,Descritivo,Abreviatura 
FROM [pmate-Equamat2000].dbo.cicloensino

COMMIT;

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
FROM [pmate-Equamat2000].dbo.tblcompeticaoEvento

SET IDENTITY_INSERT pmate.Competicao OFF
COMMIT;


--Prova
BEGIN TRANSACTION;
SET IDENTITY_INSERT pmate.Prova ON

INSERT INTO pmate.Prova(id,IdAuthor, IdCompeticao, NomeProva, DataCriacao, MaxEscolas, MaxTentJogo, TempoTotalJogo, NumNiveis, VidasPorNivel, NumElemsEquipa, Calculadora, DataInscFinal, DataProva, InicioPreInscricao, FimPreInscricao, InicioInscricaoEquipas, FimInscricaoEquipas, FimProva, Estilo, URL, TreinoVisivel, IdCicloEnsino, plataforma)
SELECT IdCompeticao, IdUser, idCompeticaoEvento, NomeCompeticao, DataCriacao, NumaMaxEqEscolas, MaxTentJogo, TempoTotalJogo, NumTotNiveis, VidasPorNivel, NumEltosEquipa, Calculadora, DataInscFinal, DiaHoraCompeticao, InicioPreInscricao, FimPreInscricao, InicioInscricaoEquipas, FimInscricaoEquipas, NULL, Estilo, URL, TreinoVisivel, RefIdCicloEnsino, plataforma
FROM [pmate-Equamat2000].dbo.tblcompeticao
where NomeCompeticao not like '%treino%'

SET IDENTITY_INSERT pmate.Prova OFF
COMMIT;

-- SubProvas
BEGIN TRANSACTION;

INSERT INTO pmate.SubProvas(IdProvaPai,IdProvaFilho)          
SELECT  distinct refidcompeticao_pai, refidcompeticao_filho
FROM  [pmate-Equamat2000].dbo.tblsubcompeticoes
JOIN pmate.Prova on pmate.Prova.id = [pmate-Equamat2000].dbo.tblsubcompeticoes.ref

DELETE FROM pmate.SubProvas
where IdProvaPai=IdProvaFilho

COMMIT;


--ProvaEscolas
BEGIN TRANSACTION;

INSERT INTO pmate.ProvaEscolas(IdEscola,IdProva,IdUserEscola,DataRegisto,EscolaOrganizadora,AnoLetivo)			 
SELECT distinct [pmate-Equamat2000].dbo.tblinsccompescola.idEscola,[pmate-Equamat2000].dbo.tblinsccompescola.IdCompeticao, idUserEscola,MaxDate,refescolaorganizadora, refanolectivo
FROM [pmate-Equamat2000].dbo.tblinsccompescola
join pmate.Prova on pmate.Prova.id=[pmate-Equamat2000].dbo.tblinsccompescola.IdCompeticao --Avoid deleted Provas
join pmate.Escola on pmate.Escola.id=[pmate-Equamat2000].dbo.tblinsccompescola.IdEscola   --Avoid deleted Escolas
JOIN (                                                                                    --Avoid duplicated registries with different insert dates
    select idEscola,IdCompeticao, max(DataRegisto) as MaxDate
    from  [pmate-Equamat2000].dbo.tblinsccompescola
    group by idEscola,IdCompeticao
) tm ON [pmate-Equamat2000].dbo.tblinsccompescola.idEscola = tm.idEscola
        and [pmate-Equamat2000].dbo.tblinsccompescola.IdCompeticao = tm.IdCompeticao
        and [pmate-Equamat2000].dbo.tblinsccompescola.DataRegisto = tm.MaxDate
COMMIT


-- Equipa 
BEGIN TRANSACTION;
SET IDENTITY_INSERT pmate.Equipa ON

INSERT INTO pmate.Equipa(id,Nome,DataCriacao,IdEscola)
SELECT IdEquipa,NomeEquipa,dataI,IdEscola
FROM [pmate-Equamat2000].dbo.tblEquipas
join pmate.Escola on IdEscola=pmate.Escola.id -- Avoid deleted schools

SET IDENTITY_INSERT pmate.Equipa OFF
COMMIT;


-- EquipaProva
BEGIN TRANSACTION;

INSERT INTO pmate.EquipaProva(IdEquipa,IdProva)
SELECT distinct IdEquipa,[pmate-Equamat2000].dbo.tblequipascomp.IdCompeticao
FROM [pmate-Equamat2000].dbo.tblequipascomp
join pmate.Equipa on pmate.Equipa.id=IdEquipa -- Avoid deleted equipas
join pmate.Prova on pmate.Prova.id=[pmate-Equamat2000].dbo.tblequipascomp.IdCompeticao --Avoid deleted Provas

COMMIT;



-- EquipaAlunos
BEGIN TRANSACTION;

INSERT INTO pmate.EquipaAlunos(IdEquipa,IdUser)
SELECT distinct IdEquipa,IdUser
FROM [pmate-Equamat2000].dbo.tblalunosequipas
join pmate.Equipa on pmate.Equipa.id=IdEquipa -- Avoid deleted equipas
join dbo.AspNetUsers on dbo.AspNetUsers.id= cast(IdUser as nvarchar) -- Avoid deleted users

COMMIT;



--ProvaEquipaEnunciado
BEGIN TRANSACTION;
SET IDENTITY_INSERT pmate.ProvaEquipaEnunciado ON

INSERT INTO pmate.ProvaEquipaEnunciado(id,IdProva,IdEquipa,_Data,_Status,ultimoNivel,tempo)
SELECT distinct [pmate-Equamat2000].dbo.tblJogoGeradoAnterior.IdJogo,MaxIdComp, IdEquipa,Data,Status,MaxNivel,tempo
FROM [pmate-Equamat2000].dbo.tblJogoGeradoAnterior 
join pmate.Prova on pmate.Prova.id=IdCOmpet --Buscar Apenas Provas
join  pmate.Equipa on  pmate.Equipa.id=[pmate-Equamat2000].dbo.tblJogoGeradoAnterior.IdEquipa --Buscar o id da equipa
JOIN (                                                                                    --Avoid duplicated IdJogo
    select distinct IdJogo, max( IdCOmpet) as MaxIdComp, max(ultimoNivel) as MaxNivel
    from  [pmate-Equamat2000].dbo.tblJogoGeradoAnterior
    group by IdJogo
) tm ON  [pmate-Equamat2000].dbo.tblJogoGeradoAnterior.IdJogo = tm.IdJogo 
		and [pmate-Equamat2000].dbo.tblJogoGeradoAnterior.IdCOmpet = tm.MaxIdComp
		and [pmate-Equamat2000].dbo.tblJogoGeradoAnterior.ultimoNivel = tm.MaxNivel


INSERT INTO pmate.ProvaEquipaEnunciado(id,IdProva,IdEquipa,_Data,_Status,ultimoNivel,tempo)
SELECT  IdJogo,IdCOmpet, IdEquipa,Data,Status,ultimoNivel,tempo
FROM [pmate-Equamat2000].dbo.tblJogoGerado_2020_2021 
join pmate.Prova on pmate.Prova.id=IdCOmpet --Buscar Apenas Provas
join  pmate.Equipa on  pmate.Equipa.id=[pmate-Equamat2000].dbo.tblJogoGerado_2020_2021.IdEquipa --Buscar o id da equipa


SET IDENTITY_INSERT pmate.ProvaEquipaEnunciado OFF
COMMIT;



------------------------------------ TRAINNING_TESTS  RELATED -----------------------------------

--Treino
BEGIN TRANSACTION;
SET IDENTITY_INSERT pmate.Treino ON

INSERT INTO pmate.Treino(id,IdAuthor, NomeProva, DataCriacao, MaxEscolas, MaxTentJogo, TempoTotalJogo, NumNiveis, VidasPorNivel, NumElemsEquipa, Calculadora, Estilo, URL, TreinoVisivel, IdCicloEnsino, plataforma)
SELECT IdCompeticao, IdUser, NomeCompeticao, DataCriacao, NumaMaxEqEscolas, MaxTentJogo, TempoTotalJogo, NumTotNiveis, VidasPorNivel, NumEltosEquipa, Calculadora, Estilo, URL, TreinoVisivel, RefIdCicloEnsino, plataforma
FROM [pmate-Equamat2000].dbo.tblcompeticao
Where NomeCompeticao like '%treino%'

SET IDENTITY_INSERT pmate.Treino OFF
COMMIT;



--EnunciadoTreino
BEGIN TRANSACTION;
SET IDENTITY_INSERT pmate.TreinoEnunciado ON

INSERT INTO pmate.TreinoEnunciado(id,IdTreino,IdUser,_Data,_Status,ultimoNivel,tempo)
SELECT distinct [pmate-Equamat2000].dbo.tblJogoGeradoAnterior.IdJogo,MaxIdComp, IdUser,Data,Status,MaxNivel,tempo
FROM [pmate-Equamat2000].dbo.tblJogoGeradoAnterior 
join pmate.Treino on pmate.Treino.id=IdCOmpet --Buscar Apenas Treinos
join  [pmate-Equamat2000].dbo.tblalunosequipas on  [pmate-Equamat2000].dbo.tblalunosequipas.IdEquipa=[pmate-Equamat2000].dbo.tblJogoGeradoAnterior.IdEquipa --Buscar o id do user da equipa
join dbo.AspNetUsers on dbo.AspNetUsers.id=cast(IdUser as nvarchar) -- Avoid deleted users
JOIN (                                                                                    --Avoid duplicated IdJogo
    select distinct IdJogo, max( IdCOmpet) as MaxIdComp, max(ultimoNivel) as MaxNivel
    from  [pmate-Equamat2000].dbo.tblJogoGeradoAnterior
    group by IdJogo
) tm ON  [pmate-Equamat2000].dbo.tblJogoGeradoAnterior.IdJogo = tm.IdJogo 
		and [pmate-Equamat2000].dbo.tblJogoGeradoAnterior.IdCOmpet = tm.MaxIdComp
		and [pmate-Equamat2000].dbo.tblJogoGeradoAnterior.ultimoNivel = tm.MaxNivel


INSERT INTO pmate.TreinoEnunciado(id,IdTreino,IdUser,_Data,_Status,ultimoNivel,tempo)
SELECT  IdJogo,IdCOmpet, IdUser,Data,Status,ultimoNivel,tempo
FROM [pmate-Equamat2000].dbo.tblJogoGerado_2020_2021 
join pmate.Treino on pmate.Treino.id=IdCOmpet --Buscar Apenas Treinos
join  [pmate-Equamat2000].dbo.tblalunosequipas on  [pmate-Equamat2000].dbo.tblalunosequipas.IdEquipa=[pmate-Equamat2000].dbo.tblJogoGerado_2020_2021.IdEquipa --Buscar o id do user da equipa
join dbo.AspNetUsers on dbo.AspNetUsers.id=cast(IdUser as nvarchar) -- Avoid deleted users


SET IDENTITY_INSERT pmate.TreinoEnunciado OFF
COMMIT;

