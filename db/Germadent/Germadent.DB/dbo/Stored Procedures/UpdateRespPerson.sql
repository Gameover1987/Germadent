-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 01.04.2020
-- Description:	Редактирование ответственного лица (доктора или техника)
-- =============================================
CREATE PROCEDURE UpdateRespPerson
	
	@responsiblePersonId int,
	@customerId int, 
	@rp_Position nvarchar(30),
	@responsiblePerson nvarchar(50),
	@rp_phone varchar(15)	

AS
BEGIN
	
	SET NOCOUNT ON;

    UPDATE ResponsiblePersons
	SET CustomerID = @customerId,
		RP_Position = @rp_Position,
		ResponsiblePerson = @responsiblePerson,
		RP_Phone = @rp_phone
	WHERE ResponsiblePersonID = @responsiblePersonId

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateRespPerson] TO [gdl_user]
    AS [dbo];

