-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 01.04.2020
-- Description:	Редактирование заказчика
-- =============================================
CREATE PROCEDURE [dbo].[UpdateCustomer] 
	
	@customerId int,
	@customerName nvarchar(70),
	@customerEmail nvarchar(250),
	@customerWebsite nvarchar(250),
	@customerDescription nvarchar(250)

AS
BEGIN
	
	SET NOCOUNT ON;

    UPDATE Customers
	SET CustomerName = @customerName
		, CustomerEmail = @customerEmail
		, CustomerWebSite = @customerWebsite
		, CustomerDescription = @customerDescription
	WHERE CustomerID  = @customerId

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateCustomer] TO [gdl_user]
    AS [dbo];

