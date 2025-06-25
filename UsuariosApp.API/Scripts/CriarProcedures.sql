CREATE PROCEDURE SP_CRIAR_USUARIO
	@NOME		VARCHAR(150),			-- Parâmetro de entrada
	@EMAIL		VARCHAR(100),			-- Parâmetro de entrada
	@SENHA		VARCHAR(100),			-- Parâmetro de entrada
	@PERFILID	UNIQUEIDENTIFIER,		-- Parâmetro de entrada
	@USUARIOID	UNIQUEIDENTIFIER OUTPUT	-- Parâmetro de saída
AS
BEGIN
	
	--Verificar se já existe um usuário com o email informado
	IF EXISTS (SELECT 1 FROM USUARIO WHERE EMAIL = @EMAIL)
	BEGIN
		RAISERROR('Já existe um usuário com este email.', 16, 1);
		RETURN; --Finaliza a procedure
	END;

	--Verificar se o perfil informado não existe
	IF NOT EXISTS (SELECT 1 FROM PERFIL WHERE ID = @PERFILID)
	BEGIN
		RAISERROR('O perfil informado não existe.', 16, 1);
		RETURN; --Finaliza a procedure
	END;

	--Gerando um ID para o usuário
	SET @USUARIOID = NEWID();

	BEGIN TRANSACTION;

	--Inserir o usuário na tabela
	INSERT INTO USUARIO(ID, NOME, EMAIL, SENHA, PERFILID)
	VALUES(@USUARIOID, @NOME, @EMAIL, @SENHA, @PERFILID);

	COMMIT;
END;
GO