-- =============================================
-- Author:		Alexey Kolosenok
-- Create date: 02.04.2020
-- Description:	Объединение клиентов, удаление дубликатов
-- =============================================
CREATE PROCEDURE [dbo].[UnionCustomers] 
	
	@oldCustomerId int,
	@newCustomerId int

AS
BEGIN
	
	SET NOCOUNT ON;
	

	UPDATE ResponsiblePersons 
	SET CustomerID = @newCustomerId 
	WHERE CustomerID = @oldCustomerId

	UPDATE WorkOrder 
	SET CustomerID = @newCustomerId 
	WHERE CustomerID = @oldCustomerId
	
	DELETE
	FROM Customers
	WHERE CustomerID = @oldCustomerId
    
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UnionCustomers] TO [gdl_user]
    AS [dbo];

