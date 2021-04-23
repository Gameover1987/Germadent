-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 10.04.2021
-- Description:	Возвращает набор технологических операций, доступных сотруднику согласно его должности
-- =============================================
CREATE FUNCTION [dbo].[GetRelevantOperations]
(	
	@userId int,
	@workOrderId int
)
RETURNS TABLE 
AS
RETURN 
(
	-- Вытаскиваем все доступные для данного специалиста технологические операции вместе с актуальными расценками с учётом совмещения должностей	
	WITH teop (TechnologyOperationID, TechnologyOperationUserCode, TechnologyOperationName, QualifyingRank, Rate) AS (
		SELECT t.TechnologyOperationID, t.TechnologyOperationUserCode, t.TechnologyOperationName, epc.QualifyingRank, r.Rate
		FROM dbo.EmployeePositionsCombination epc
			INNER JOIN dbo.EmployeePositions ep ON epc.EmployeePositionID = ep.EmployeePositionID
			INNER JOIN dbo.TechnologyOperations t ON ep.EmployeePositionID = t.EmployeePositionID
			INNER JOIN dbo.Rates r ON t.TechnologyOperationID = r.TechnologyOperationID AND epc.QualifyingRank = r.QualifyingRank
		WHERE epc.EmployeeID = @userId
			AND GETDATE() BETWEEN ISNULL(r.DateBeginning, '17530101') AND ISNULL(r.DateEnd, '99991231')),
	
	-- Из зубной карты заказ-наряда тащим пользоваельские коды ценовых позиций и коды изделий. Группируем по изделиям, считаем количество
	codes (PricePositionCode, ProductID, ProductCount) AS (
		SELECT pp.PricePositionCode, tc.ProductID, COUNT(tc.WorkOrderID) AS ProductCount
		FROM dbo.ToothCard tc
			INNER JOIN dbo.PricePositions pp ON tc.PricePositionID = pp.PricePositionID
		WHERE tc.WorkOrderID = @workOrderId
		GROUP BY pp.PricePositionCode, tc.ProductID)

	-- Выводим сцепленные данные
	SELECT teop.*, codes.ProductID, codes.ProductCount
	FROM teop, codes
	WHERE teop.TechnologyOperationUserCode = codes.PricePositionCode
	
	-- Прицепляем технологические операции, доступные специалисту, но без пользовательского кода
	UNION ALL 
	SELECT teop.*, NULL, NULL
	FROM teop
	WHERE teop.TechnologyOperationUserCode IS NULL
)