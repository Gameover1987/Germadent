﻿-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 01.12.2019
-- Description:	Добавление зубной карты в заказ-наряд
-- =============================================
CREATE PROCEDURE [dbo].[AddNewToothCardInWO] 

	@jsonString varchar(MAX)

AS
BEGIN
	
	INSERT INTO ToothCard
		(WorkOrderID, ToothNumber, MaterialID, ProstheticsID)
	SELECT ToothNumber, WorkOrderID, MaterialID, ProstheticsID
	FROM OPENJSON (@jsonString)
		WITH (workOrderID int, toothNumber int, materialID int, prostheticsID int)		

END