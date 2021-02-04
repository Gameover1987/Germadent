-- =============================================
-- Author:		Alexey Kolosenok
-- Create date: 07.02.2020
-- Description:	Возвращает список id заказ-нарядов, в зубных картах которых встречаются искомые материалы
-- =============================================
CREATE FUNCTION [dbo].[GetWorkOrderIdForMaterialSelect] 
(	
	@materialSet nvarchar(MAX) -- в составе json-строки обязательно должны быть id материалов
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT WorkOrderID
	FROM dbo.ToothCard
	WHERE MaterialID IN 
	(SELECT Id
		FROM OPENJSON(@materialSet) 
		WITH (Id int))
)