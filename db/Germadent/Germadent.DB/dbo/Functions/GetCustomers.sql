-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 01.04.2020
-- Description:	Возвращает список заказчиков по заданным параметрам
-- =============================================
CREATE FUNCTION [dbo].[GetCustomers]
(	
	
	@customerId int = NULL,
	@customerName nvarchar(70) = NULL
--	@customerPhone nvarchar(250) = NULL,
--	@customerEmail nvarchar(250) = NULL,
--	@customerWebsite nvarchar(250) = NULL,
--	@customerDescription nvarchar(250) = NULL

)
RETURNS TABLE 
AS
RETURN 
(
	SELECT *
	FROM Customers
	WHERE CustomerID = ISNULL(@customerId, CustomerID)
		AND CustomerName LIKE '%'+ISNULL(@customerName, '')+'%'
	--	AND CustomerPhone LIKE '%'+ISNULL(@customerPhone, '')+'%'
	--	AND CustomerEmail LIKE '%'+ISNULL(@customerEmail, '')+'%'
	--	AND CustomerWebSite LIKE '%'+ISNULL(@customerWebsite, '')+'%'
	--	AND CustomerDescription LIKE '%'+ISNULL(@customerDescription, '')+'%'
)