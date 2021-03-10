-- =============================================
-- Author:		Alexey Kolosenok
-- Create date: 07.02.2020
-- Description:	Возвращает список материалов из заказ-наряда в виде одной строки
-- =============================================
CREATE FUNCTION [dbo].[GetMaterialsEnumByWOId] 
(
	@workOrderID int
)
RETURNS nvarchar(250)
AS
BEGIN

	DECLARE @materialsEnum nvarchar(250);

	--Во временную таблицу выводим список материалов
	WITH MaterialScroll AS 
	(
		SELECT DISTINCT m.MaterialName
		FROM dbo.ToothCard tc 
			INNER JOIN dbo.WorkOrder wo ON tc.WorkOrderID = wo.WorkOrderID
			INNER JOIN dbo.Materials m ON tc.MaterialID = m.MaterialID
		WHERE wo.WorkOrderID = @workOrderID)
	
	-- Агрегируем поле в строковое значение
	SELECT @materialsEnum = STRING_AGG(MaterialName, '; ')
	FROM MaterialScroll

	RETURN @materialsEnum

END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetMaterialsEnumByWOId] TO [gdl_user]
    AS [dbo];

