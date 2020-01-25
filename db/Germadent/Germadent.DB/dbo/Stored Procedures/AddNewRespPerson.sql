-- =============================================
-- Author:		Alexey Kolosenok
-- Create date: 12.12.2019
-- Description:	Создание нового ответственного лица (доктора или техника) для заказчика
-- =============================================
CREATE PROCEDURE [dbo].[AddNewRespPerson] 
	
	@customerID int, 
	@rp_Position nvarchar(30),
	@responsiblePerson nvarchar(50),
	@rp_phone varchar(15),
	@responsiblePersonId int output

AS
BEGIN
	
	SET NOCOUNT ON;

    -- Чтобы неоправданно не возрастало значение Id в ключевом поле - сначала его "подбивка":
	BEGIN
		DECLARE @max_Id int

		SELECT @max_Id = MAX(ResponsiblePersonID)
		FROM ResponsiblePersons

		EXEC IdentifierAlignment ResponsiblePersons, @max_Id
		REVERT
	END
	-- Собственно вставка:
	
	INSERT INTO ResponsiblePersons
	(CustomerID, ResponsiblePerson, RP_Position, RP_Phone)
	values
	(@customerID, @responsiblePerson, @rp_Position, @rp_phone)

	SET @responsiblePersonId = SCOPE_IDENTITY()

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AddNewRespPerson] TO [gdl_user]
    AS [dbo];

