-- =============================================
-- Author:		Alexey Kolosenok
-- Create date: 01.04.2020
-- Description:	Возвращает ответственных лиц (докторов или техников) по заданным критериям
-- =============================================
CREATE FUNCTION [dbo].[GetResponsiblePersons]
(	
	@responsiblePersonId int = NULL,
	@responsiblePerson nvarchar(50) = NULL
--	@rp_position nvarchar(30) = NULL,
--	@rp_phone nvarchar(150) = NULL,
--	@rp_email nvarchar(150) = NULL,
--	@rp_description nvarchar(250) = NULL
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT ResponsiblePersonID, ResponsiblePerson, RP_Position, RP_Phone, RP_Email, RP_Description
	FROM dbo.ResponsiblePersons
	WHERE ResponsiblePersonID = ISNULL(@responsiblePersonId, ResponsiblePersonID)
		AND ResponsiblePerson LIKE '%'+ISNULL(@responsiblePerson, '')+'%'
	--	AND RP_Position LIKE '%'+ISNULL(@rp_position, '')+'%'
	--	AND RP_Phone LIKE '%'+ISNULL(@rp_phone, '')+'%'
	--	AND RP_Email LIKE '%'+ISNULL(@rp_email, '')+'%'
	--	AND RP_Description LIKE '%'+ISNULL(@rp_description, '')+'%'
)