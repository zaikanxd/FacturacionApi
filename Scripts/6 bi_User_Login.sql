USE Billing
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE bi_User_Login
@username VARCHAR(50),
@password VARCHAR(200),
@errorMessage VARCHAR(200) OUTPUT
AS

DECLARE @userId INT
DECLARE @status BIT

SELECT
	@userId = id,
	@status = status
FROM [User]
WHERE username = @username
AND password = @password

IF(@userId IS NULL)
BEGIN
	SET @errorMessage = 'Usuario o contraseña no son correctos';
END
ELSE
BEGIN
	IF(@status = 0)
	BEGIN
		SET @errorMessage = 'Usuario desactivado';
	END
END