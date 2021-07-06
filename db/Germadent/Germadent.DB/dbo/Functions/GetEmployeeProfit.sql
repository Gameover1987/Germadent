-- =============================================
-- Author:		Alexey Kolosenok
-- Create date: 30.06.2021
-- Description:	<Description,,>
-- =============================================
CREATE FUNCTION GetEmployeeProfit
(	
	@userId int,
	@dateFrom datetime,
	@dateTo datetime
)
RETURNS TABLE 
AS
RETURN 
(
	WITH closedStatuses (WorkOrderID) AS
	(SELECT WorkOrderID
	FROM dbo.StatusList
	WHERE Status > 89
		AND StatusChangeDateTime BETWEEN ISNULL(@dateFrom, '17530101') AND ISNULL(@dateTo, '99991231'))

	SELECT tc.WorkOrderID, wo.DocNumber, SUM(tc.Price) Proceeds, SUM(tc.Price) * 0.3 Profit
	FROM dbo.ToothCard tc 
		INNER JOIN dbo.WorkOrder wo ON tc.WorkOrderID = wo.WorkOrderID
		INNER JOIN dbo.WorkList wl ON tc.WorkOrderID = wl.WorkOrderID AND tc.ProductID = wl.ProductID
		INNER JOIN closedStatuses ON tc.WorkOrderID = closedStatuses.WorkOrderID
	WHERE wl.EmployeeIDStarted = @userId
	GROUP BY tc.WorkOrderID, wo.DocNumber
)