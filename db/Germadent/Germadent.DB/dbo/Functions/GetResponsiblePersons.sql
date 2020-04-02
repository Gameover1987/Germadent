-- =============================================
-- Author:		Alexey Kolosenok
-- Create date: 01.04.2020
-- Description:	Возвращает ответственных лиц (докторов или техников) по заданным критериям
-- =============================================
CREATE FUNCTION GetResponsiblePersons
(	
	@responsiblePersonId int = NULL,
	@customerId int = NULL,
	@responsiblePerson nvarchar(50) = NULL,
	@rp_Position nvarchar(30) = NULL,
	@rp_Phone nvarchar(15) = NULL
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT ResponsiblePersonID, CustomerID, ResponsiblePerson, RP_Position, RP_Phone
	FROM ResponsiblePersons
	WHERE ResponsiblePersonID = ISNULL(@responsiblePersonId, ResponsiblePersonID)
		AND CustomerID = ISNULL(@customerId, CustomerID)
		AND ResponsiblePerson LIKE '%'+ISNULL(@responsiblePerson, '')+'%'
		AND RP_Position LIKE '%'+ISNULL(@rp_Position, '')+'%'
		AND RP_Phone LIKE '%'+ISNULL(@rp_Phone, '')+'%'
)