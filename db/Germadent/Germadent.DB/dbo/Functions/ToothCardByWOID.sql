-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 23.11.2019
-- Description:	Зубная карта из заказ-наряда по его ID
-- =============================================
CREATE FUNCTION ToothCardByWOID 
(	
	
	@WorkOrderID int
	
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT tc.ToothNumber, m.MaterialName, p.ProstheticsName
	FROM ToothCard tc INNER JOIN WorkOrder wo ON tc.WorkOrderID = wo.WorkOrderID
		INNER JOIN Materials m ON tc.MaterialID = m.MaterialID
		INNER JOIN TypeOfProsthetics p ON tc.ProstheticsID = p.ProstheticsID
	WHERE wo.WorkOrderID = @WorkOrderID
)
