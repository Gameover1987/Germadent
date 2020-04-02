-- =============================================
-- Author:		sql.ru
-- Create date: 02.04.2020
-- Description:	Возвращает список внешних ключей со связанными таблицами и полями
-- =============================================
CREATE FUNCTION [dbo].[GetContraints]
(	
	@pk_Table varchar(50),
	@pk_Name varchar(50)
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT OBJECT_NAME(constid) as name, OBJECT_NAME(rkeyid) as PK_TABLE, OBJECT_NAME(fkeyid) as FK_TABLE, sc1.name as PK_NAME, sc2.name as FK_NAME
	FROM sysforeignkeys fk
	INNER JOIN syscolumns sc1 ON fk.rkeyid = sc1.id AND fk.rkey = sc1.colid
	INNER JOIN syscolumns sc2 ON fk.fkeyid = sc2.id AND fk.fkey = sc2.colid 
	WHERE OBJECT_NAME(rkeyid) = @pk_Table
		AND sc1.name = @pk_Name
)