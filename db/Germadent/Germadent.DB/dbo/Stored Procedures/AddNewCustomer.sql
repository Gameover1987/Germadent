-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 12.12.2019
-- Description:	Создание нового заказчика
-- =============================================
CREATE PROCEDURE [dbo].[AddNewCustomer] 
	
	@customerName nvarchar(70),
	@customerId int output
	 
AS
BEGIN
	
	SET NOCOUNT ON;
	
	-- Чтобы неоправданно не возрастало значение Id в ключевом поле - сначала его "подбивка":
	DECLARE @max_Id int

	SELECT @max_Id = MAX(CustomerID)
	FROM Customers
	DBCC checkident (Customers, reseed, @max_Id)

	-- Собственно вставка:
	
	INSERT INTO Customers
	(CustomerName)
	values
	(@customerName)

	SET @customerId = SCOPE_IDENTITY()

END