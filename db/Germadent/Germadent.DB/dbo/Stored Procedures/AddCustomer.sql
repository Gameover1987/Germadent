-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 12.12.2019
-- Description:	Создание нового заказчика
-- =============================================
CREATE PROCEDURE [dbo].[AddCustomer] 
	
	@customerName nvarchar(70),
	@customerPhone nvarchar(250) = NULL,
	@customerEmail nvarchar(250) = NULL,
	@customerWebsite nvarchar(250) = NULL,
	@customerDescription nvarchar(250) = NULL,
	@customerId int output
	 
AS
BEGIN	
	SET NOCOUNT ON;
	
	-- Чтобы неоправданно не возрастало значение Id в ключевом поле - сначала его "подбивка":
	BEGIN
		DECLARE @max_Id int
		SELECT @max_Id = ISNULL(MAX(CustomerID), 0)
		FROM Customers

		EXEC dbo.IdentifierAlignment Customers, @max_Id
	
		REVERT
	END
	-- Собственно вставка:
	
	INSERT INTO dbo.Customers
	(CustomerName, CustomerPhone, CustomerEmail, CustomerWebSite, CustomerDescription)
	values
	(@customerName, @customerPhone, @customerEmail, @customerWebSite, @customerDescription)

	SET @customerId = SCOPE_IDENTITY()

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AddCustomer] TO [gdl_user]
    AS [dbo];

