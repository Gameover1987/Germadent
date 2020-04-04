-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 01.04.2020
-- Description:	Редактирование ответственного лица (доктора или техника)
-- =============================================
CREATE PROCEDURE [dbo].[UpdateRespPerson]
	
	@responsiblePersonId int,
	@customerId int, 
	@rp_Position nvarchar(30),
	@responsiblePerson nvarchar(150),
	@rp_phone varchar(150),
	@rp_email nvarchar(150),
	@rp_description nvarchar(250)

AS
BEGIN
	
	SET NOCOUNT ON;

    UPDATE ResponsiblePersons
	SET CustomerID = @customerId
		, RP_Position = @rp_Position
		, ResponsiblePerson = @responsiblePerson
		, RP_Phone = @rp_phone
		, RP_Email = @rp_email
		, RP_Description = @rp_description
	WHERE ResponsiblePersonID = @responsiblePersonId

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateRespPerson] TO [gdl_user]
    AS [dbo];

