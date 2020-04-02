-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 01.04.2020
-- Description:	Возвращает список заказчиков по заданным параметрам
-- =============================================
CREATE FUNCTION GetCustomers
(	
	
	@customerId int = NULL,
	@customerName nvarchar(70) = NULL

)
RETURNS TABLE 
AS
RETURN 
(
	SELECT *
	FROM Customers
	WHERE CustomerID = ISNULL(@customerId, CustomerID)
		AND CustomerName LIKE '%'+ISNULL(@customerName, '')+'%'
)