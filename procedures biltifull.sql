CREATE PROCEDURE IncluiCliente
	@CPF varchar(14),
	@Nome varchar(50),
	@DataNasc date,
	@Sexo char,
	@Situacao char
AS
BEGIN 
	INSERT INTO Cliente (CPF, Nome, DataNasc, sexo, Situacao)
	VALUES (@CPF, @Nome, @DataNasc, @Sexo, @Situacao)
END

--********************************************

CREATE PROCEDURE ClienteSituacao
	@Situacao char,
	@CPF varchar(14)
AS
BEGIN
	UPDATE Cliente
	SET Situacao = @Situacao
	WHERE CPF = @CPF
END

--*****************************************
CREATE PROCEDURE EditaNomeCliente
	@CPF varchar(14),
	@Nome varchar(50)
	
AS
BEGIN
	UPDATE Cliente
	SET Nome = @Nome
	WHERE CPF = @CPF
END



--******************************************************

CREATE PROCEDURE EditaDataNascCliente
	@CPF varchar(14),
	@DataNasc date
AS
BEGIN
	UPDATE Cliente
	SET DataNasc = @DataNasc
	WHERE CPF = @CPF
END

--************************************************************

CREATE PROCEDURE EditaSexoCliente
	@CPF varchar(14),
	@sexo char
AS
BEGIN
	UPDATE Cliente
	SET sexo = @sexo
	WHERE CPF = @CPF
END

--***********************************************


CREATE PROCEDURE IncluiFornecedor
	@CNPJ varchar(18),
	@RazaoSocial varchar(50),
	@DataAbertura date,
	@Situacao char
AS
BEGIN 
	INSERT INTO Fornecedor(CNPJ, RazaoSocial, DataAbertura, Situacao)
	VALUES (@CNPJ, @RazaoSocial, @DataAbertura, @Situacao)
END

--**************************************************

CREATE PROCEDURE EditaRazaoSocial
	@CNPJ varchar(18),
	@RazaoSocial varchar(50)	
AS
BEGIN 
	UPDATE Fornecedor
	SET RazaoSocial = @RazaoSocial
	WHERE CNPJ = @CNPJ
END

--*****************************************************

CREATE PROCEDURE EditaDataAbertura
	@CNPJ varchar(18),
	@DataAbertura varchar(50)	
AS
BEGIN 
	UPDATE Fornecedor
	SET DataAbertura = @DataAbertura
	WHERE CNPJ = @CNPJ
END

--**************************************************
CREATE PROCEDURE IncluiMP
	@Id varchar(6),
	@Nome varchar(50),	
	@Situacao char
AS
BEGIN 
	INSERT INTO MateriaPrima (Id, Nome, Situacao)
	VALUES (@Id, @Nome, @Situacao)
END

