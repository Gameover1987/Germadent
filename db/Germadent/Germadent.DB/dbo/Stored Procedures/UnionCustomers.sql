-- =============================================
-- Author:		Alexey Kolosenok
-- Create date: 02.04.2020
-- Description:	Объединение клиентов, удаление дубликатов. В случае отсутствия зависимых заказ-нарядов - простое удаление
-- =============================================
CREATE PROCEDURE [dbo].[UnionCustomers] 
	
	@oldCustomerId int,
	@newCustomerId int = NULL,
	@resultCount int output

AS
BEGIN
	
	SET NOCOUNT ON;
	
	IF @newCustomerId IS NOT NULL 
		BEGIN
			UPDATE dbo.WorkOrder 
			SET CustomerID = @newCustomerId 
			WHERE CustomerID = @oldCustomerId
	
			DELETE
			FROM dbo.Customers
			WHERE CustomerID = @oldCustomerId

			SET @resultCount = @@rowcount
		END
		ELSE IF @newCustomerId IS NULL AND NOT EXISTS (SELECT CustomerID FROM dbo.WorkOrder WHERE CustomerID = @oldCustomerId)
			BEGIN
				DELETE
				FROM dbo.Customers
				WHERE CustomerID = @oldCustomerId

				SET @resultCount = @@rowcount

			END
			ELSE BEGIN
				SET @resultCount = 0
				END	
    
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UnionCustomers] TO [gdl_user]
    AS [dbo];

