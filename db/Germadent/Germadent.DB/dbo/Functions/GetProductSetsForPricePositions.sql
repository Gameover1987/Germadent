﻿-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 08.10.2020
-- Description:	Возвращает перечень изделий, сопоставленный с ценовыми позициями
-- =============================================
CREATE FUNCTION [dbo].[GetProductSetsForPricePositions] 
(	
	
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT ps.*, p.ProductName
	FROM dbo.ProductSet ps
	INNER JOIN dbo.Products p ON ps.ProductID = p.ProductID
)