﻿-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 11.04.2020
-- Description:	Возвращает список атрибутов
-- =============================================
CREATE FUNCTION [dbo].[GetAttributes] 
(	
	
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT *
	FROM dbo.Attributes
)