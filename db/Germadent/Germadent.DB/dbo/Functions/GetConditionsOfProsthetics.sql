-- =============================================
-- Author:		Alexey Kolosenok
-- Create date: 05.02.2020
-- Description:	Возвращает список условий протезирования
-- =============================================
CREATE FUNCTION [dbo].[GetConditionsOfProsthetics]
(	
	
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT ConditionID, ConditionName
	FROM dbo.ConditionsOfProsthetics 
)