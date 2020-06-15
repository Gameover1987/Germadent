-- =============================================
-- Author:		Alexey Kolosenok
-- Create date: 12.12.2019
-- Description:	Создание нового ответственного лица (доктора или техника) для заказчика
-- =============================================
CREATE PROCEDURE [dbo].[AddNewRespPerson] 
	
	@rp_position nvarchar(30),
	@responsiblePerson nvarchar(150),
	@rp_phone nvarchar(150) = NULL,
	@rp_email nvarchar(150) = NULL,
	@rp_description nvarchar(250) = NULL,
	@responsiblePersonId int output

AS
BEGIN
	
	SET NOCOUNT ON;

    -- Чтобы неоправданно не возрастало значение Id в ключевом поле - сначала его "подбивка":
	BEGIN
		DECLARE @max_Id int

		SELECT @max_Id = ISNULL(MAX(ResponsiblePersonID), 0)
		FROM ResponsiblePersons

		EXEC IdentifierAlignment ResponsiblePersons, @max_Id
		REVERT
	END
	-- Собственно вставка:
	
	INSERT INTO ResponsiblePersons
	(ResponsiblePerson, RP_Position, RP_Phone, RP_Email, RP_Description)
	values
	(@responsiblePerson, @rp_Position, @rp_phone, @rp_email, @rp_description)

	SET @responsiblePersonId = SCOPE_IDENTITY()

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AddNewRespPerson] TO [gdl_user]
    AS [dbo];

