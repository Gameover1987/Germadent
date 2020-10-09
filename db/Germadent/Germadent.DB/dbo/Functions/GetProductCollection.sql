﻿-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 08.10.2020
-- Description:	Возвращает перечень изделий, сопоставленный с ценовыми позициями
-- =============================================
CREATE FUNCTION GetProductCollection 
(	
	
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT ps.*, p.ProstheticsName
	FROM ProductSet ps
	INNER JOIN TypesOfProsthetics p ON ps.ProductID = P.ProstheticsID
)