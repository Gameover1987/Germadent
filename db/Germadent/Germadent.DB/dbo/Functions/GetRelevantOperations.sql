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
	WITH teop (UserID, UserFullName, 
				EmployeePositionName, TechnologyOperationID, TechnologyOperationUserCode, TechnologyOperationName, QualifyingRank, Rate) AS (
		SELECT u.UserID, CONCAT(u.FamilyName,' ', LEFT(u.FirstName, 1), '.', LEFT(u.Patronymic, 1), '.') AS UserFullName,
				ep.EmployeePositionName, t.TechnologyOperationID, t.TechnologyOperationUserCode, t.TechnologyOperationName, epc.QualifyingRank, r.Rate
		FROM dbo.EmployeePositionsCombination epc
			INNER JOIN dbo.EmployeePositions ep ON epc.EmployeePositionID = ep.EmployeePositionID
			INNER JOIN dbo.TechnologyOperations t ON ep.EmployeePositionID = t.EmployeePositionID
			INNER JOIN dbo.Rates r ON t.TechnologyOperationID = r.TechnologyOperationID AND epc.QualifyingRank = r.QualifyingRank
			INNER JOIN dbo.Users u ON epc.EmployeeID = u.UserID
		WHERE epc.EmployeeID = @userId --8 --
			AND GETDATE() BETWEEN ISNULL(r.DateBeginning, '17530101') AND ISNULL(r.DateEnd, '99991231')),
	
	-- Из зубной карты заказ-наряда тащим пользовательские коды ценовых позиций и коды изделий. Группируем по изделиям, считаем количество
	codes (PricePositionCode, ProductID, ProductCount) AS (
		SELECT pp.PricePositionCode, tc.ProductID, COUNT(tc.ProductID) AS ProductCount
		FROM dbo.ToothCard tc
			INNER JOIN dbo.PricePositions pp ON tc.PricePositionID = pp.PricePositionID
		WHERE tc.WorkOrderID = @workOrderId --4288--
		GROUP BY pp.PricePositionCode, tc.ProductID),

	-- Тащим дополнительные коды технологических операций
	adlc (CodeMC, ProductID, ProductCount) AS(
		SELECT cc.CodeMC, codes.ProductID, codes.ProductCount
		FROM dbo.CodesCompliance cc, codes
		WHERE cc.CodeDL IN (SELECT LEFT(PricePositionCode, 3) FROM codes))
			   		 

	-- Выводим совмещённые данные
	SELECT teop.*, codes.ProductID, codes.ProductCount, dbo.GetUrgencyRatioForWO(@workOrderId) AS UrgencyRatio, teop.Rate * codes.ProductCount * dbo.GetUrgencyRatioForWO(@workOrderId) AS OperationCost
	FROM teop, codes
	WHERE teop.TechnologyOperationUserCode = LEFT(codes.PricePositionCode, 3)

	UNION 
	SELECT teop.*, adlc.ProductID, adlc.ProductCount, dbo.GetUrgencyRatioForWO(@workOrderId) AS UrgencyRatio, teop.Rate * adlc.ProductCount * dbo.GetUrgencyRatioForWO(@workOrderId) AS OperationCost
	FROM teop, adlc
	WHERE teop.TechnologyOperationUserCode = adlc.CodeMC
	
	-- Прицепляем технологические операции, доступные специалисту, но без пользовательского кода
	UNION 
	SELECT teop.*, NULL, codes.ProductCount, dbo.GetUrgencyRatioForWO(@workOrderId) AS UrgencyRatio, teop.Rate * codes.ProductCount * dbo.GetUrgencyRatioForWO(@workOrderId) AS OperationCost
	FROM teop, codes
	WHERE LEN(teop.TechnologyOperationUserCode) = 0 OR teop.TechnologyOperationUserCode IS NULL
)