-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 02.04.2020
-- Description:	Объединение ответственных лиц, удаление двойников
-- =============================================
CREATE PROCEDURE UnionResponsiblePersons 
	
	@oldResponsiblePersonId int,
	@newResponsiblePersonId int

AS
BEGIN
	
	SET NOCOUNT ON;


	UPDATE WorkOrder 
	SET CustomerID = @newResponsiblePersonId 
	WHERE CustomerID = @oldResponsiblePersonId


	DELETE
	FROM ResponsiblePersons
	WHERE ResponsiblePersonID = @oldResponsiblePersonId

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UnionResponsiblePersons] TO [gdl_user]
    AS [dbo];

