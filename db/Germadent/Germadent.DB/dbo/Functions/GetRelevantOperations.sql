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
	-- Вытаскиваем все доступные для данного специалиста технологические операции вместе с актуальными расценками с учётом совмещения должностей, наличия премиум-расценки и премиум-цены
	WITH teop (UserID, 
				UserFullName, 
				EmployeePositionID,
				EmployeePositionName, 
				TechnologyOperationID, 
				TechnologyOperationUserCode, 
				TechnologyOperationName, 
				QualifyingRank, 
				Rate) AS (
		SELECT u.UserID, 
				CONCAT(u.FamilyName,' ', LEFT(u.FirstName, 1), '.', LEFT(u.Patronymic, 1), '.') AS UserFullName,
				ep.EmployeePositionID,
				ep.EmployeePositionName, 
				t.TechnologyOperationID, 
				t.TechnologyOperationUserCode, 
				t.TechnologyOperationName, 
				epc.QualifyingRank, 
				IIF(prem.Rate IS NULL OR (SELECT FlagStl FROM dbo.WorkOrder WHERE WorkOrderID = @workOrderId) = 1, r.Rate, prem.Rate) AS Rate
		FROM dbo.EmployeePositionsCombination epc
			INNER JOIN dbo.EmployeePositions ep ON epc.EmployeePositionID = ep.EmployeePositionID
			INNER JOIN dbo.TechnologyOperations t ON ep.EmployeePositionID = t.EmployeePositionID
			INNER JOIN dbo.Rates r ON t.TechnologyOperationID = r.TechnologyOperationID 
				AND epc.QualifyingRank = r.QualifyingRank
			INNER JOIN dbo.Users u ON epc.EmployeeID = u.UserID
			LEFT JOIN dbo.Rates prem ON t.TechnologyOperationID = prem.TechnologyOperationID -- цепляем премиум-расценки
				AND prem.QualifyingRank = 4 
				AND epc.QualifyingRank = 3 
				AND GETDATE() BETWEEN ISNULL(prem.DateBeginning, '17530101') AND ISNULL(prem.DateEnd, '99991231')
		WHERE epc.EmployeeID = @userId
			AND GETDATE() BETWEEN ISNULL(r.DateBeginning, '17530101') AND ISNULL(r.DateEnd, '99991231')),
	
	-- Из зубной карты заказ-наряда тащим пользовательские коды ценовых позиций и коды изделий. Группируем по изделиям, считаем количество
	codes (PricePositionCode, ProductID, ProductName, ProductCount) AS (
		SELECT pp.PricePositionCode, tc.ProductID, P.ProductName, COUNT(tc.ProductID) AS ProductCount
		FROM dbo.ToothCard tc
			INNER JOIN dbo.PricePositions pp ON tc.PricePositionID = pp.PricePositionID
			INNER JOIN dbo.Products p ON tc.ProductID = p.ProductID
		WHERE tc.WorkOrderID = @workOrderId --4288--
		GROUP BY pp.PricePositionCode, tc.ProductID, P.ProductName),

	-- Тащим дополнительные коды технологических операций
	adlc (CodeMC, ProductID, ProductName, ProductCount) AS(
		SELECT cc.CodeMC, codes.ProductID, codes.ProductName, codes.ProductCount
		FROM dbo.CodesCompliance cc, codes
		WHERE LEFT(PricePositionCode, 3) = CodeDL),
			   		 
	unitedTeOp AS (
	-- Выводим совмещённые данные
	SELECT teop.*, codes.ProductID, codes.ProductName, codes.ProductCount, dbo.GetUrgencyRatioForWO(@workOrderId) AS UrgencyRatio, teop.Rate * codes.ProductCount * dbo.GetUrgencyRatioForWO(@workOrderId) AS OperationCost
	FROM teop, codes
	WHERE teop.TechnologyOperationUserCode = LEFT(codes.PricePositionCode, 3)

	UNION 
	SELECT teop.*, adlc.ProductID, adlc.ProductName, adlc.ProductCount, dbo.GetUrgencyRatioForWO(@workOrderId) AS UrgencyRatio, teop.Rate * adlc.ProductCount * dbo.GetUrgencyRatioForWO(@workOrderId) AS OperationCost
	FROM teop, adlc
	WHERE teop.TechnologyOperationUserCode = adlc.CodeMC
	
	-- Прицепляем технологические операции, доступные специалисту, но без пользовательского кода
	UNION 
	SELECT teop.*, codes.ProductID, codes.ProductName, codes.ProductCount, dbo.GetUrgencyRatioForWO(@workOrderId) AS UrgencyRatio, teop.Rate * codes.ProductCount * dbo.GetUrgencyRatioForWO(@workOrderId) AS OperationCost
	FROM teop, codes
	WHERE codes.ProductName NOT LIKE '%Реализация%'
		AND (LEN(teop.TechnologyOperationUserCode) = 0 OR teop.TechnologyOperationUserCode IS NULL))

-- Исключаем из перечня те операции, что уже выбраны для выполнения
	SELECT *
	FROM unitedTeOp
	WHERE TechnologyOperationID NOT IN (SELECT TechnologyOperationID 
										FROM WorkList 
										WHERE WorkOrderID = @workOrderId)
)