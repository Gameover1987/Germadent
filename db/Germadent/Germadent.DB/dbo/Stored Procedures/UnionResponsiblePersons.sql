-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 02.04.2020
-- Description:	Объединение ответственных лиц, удаление двойников. В случае отсутствия зависимых заказ-нарядов - простое удаление
-- =============================================
CREATE PROCEDURE [dbo].[UnionResponsiblePersons] 
	
	@oldResponsiblePersonId int,
	@newResponsiblePersonId int = NULL,
	@resultCount int output

AS
BEGIN
	
	SET NOCOUNT ON;

	IF @newResponsiblePersonId IS NOT NULL
		BEGIN
			UPDATE dbo.WorkOrder 
			SET ResponsiblePersonID = @newResponsiblePersonId 
			WHERE ResponsiblePersonID = @oldResponsiblePersonId

			DELETE
			FROM dbo.ResponsiblePersons
			WHERE ResponsiblePersonID = @oldResponsiblePersonId

			SET @resultCount = @@rowcount
		END
		ELSE IF @newResponsiblePersonId IS NULL AND NOT EXISTS (SELECT ResponsiblePersonID FROM dbo.WorkOrder WHERE ResponsiblePersonID = @oldResponsiblePersonId)
			BEGIN
				DELETE
				FROM dbo.ResponsiblePersons
				WHERE ResponsiblePersonID = @oldResponsiblePersonId

				SET @resultCount = @@rowcount
			END
			ELSE BEGIN
				SET @resultCount = 0
				END	
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UnionResponsiblePersons] TO [gdl_user]
    AS [dbo];

