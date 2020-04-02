-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 01.04.2020
-- Description:	Редактирование заказчика
-- =============================================
CREATE PROCEDURE UpdateCustomer 
	
	@customerId int,
	@customerName nvarchar(70)

AS
BEGIN
	
	SET NOCOUNT ON;

    UPDATE Customers
	SET CustomerName = @customerName
	WHERE CustomerID  = @customerId

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateCustomer] TO [gdl_user]
    AS [dbo];

