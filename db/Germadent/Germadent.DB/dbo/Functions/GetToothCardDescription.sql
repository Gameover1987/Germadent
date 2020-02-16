-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 13.02.2020
-- Description:	Возвращает список условий, типов протезирования и материалов для зубов в зубной карте
-- =============================================
CREATE FUNCTION [dbo].[GetToothCardDescription]
(	
	
	@workOrderId int = NULL

)
RETURNS TABLE 
AS
RETURN 
(
	SELECT CONCAT(tc.ToothNumber, ' - ', c.ConditionName, ' / ', p.ProstheticsName, ' / ', m.MaterialName,
		CASE WHEN tc.FlagBridge = 1 THEN ' / Мост' ELSE '' END) AS ToothCardDescription
	FROM ToothCard tc 
	INNER JOIN ConditionsOfProsthetics c ON tc.ConditionID = c.ConditionID
	INNER JOIN TypesOfProsthetics p ON tc.ProstheticsID = p.ProstheticsID
	INNER JOIN Materials m ON tc.MaterialID = m.MaterialID

	WHERE tc.WorkOrderID = ISNULL(@workOrderId, tc.WorkOrderID)
)