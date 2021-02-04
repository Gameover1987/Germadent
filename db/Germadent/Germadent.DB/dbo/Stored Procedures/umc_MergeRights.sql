-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 26.10.2020
-- Description:	Добавление или редактирование прав
-- =============================================
CREATE PROCEDURE [dbo].[umc_MergeRights] 
	
	@jsonString nvarchar(MAX),
	@insertedId nvarchar(MAX) output
	
AS
BEGIN
	
	SET NOCOUNT ON;

	CREATE TABLE #insertedData (Action varchar(20), RightID int)

	-- На всякий случай - подбивка значения Id в ключевом поле:
	BEGIN
		DECLARE @max_Id int
		SELECT @max_Id = ISNULL(MAX(RightID), 0)
		FROM Rights

		EXEC dbo.IdentifierAlignment Rights, @max_Id
	
		REVERT
	END;

	-- Скармливаем json-строку обобщённому табличному выражению, оно же - таблица-источник:
    WITH src(RightID, ApplicationID, RightName, RightDescription, IsObsolete) 
	AS 
	(
		SELECT RightID, ApplicationID, RightName, RightDescription, IsObsolete 
			FROM OPENJSON (@jsonString) 
			WITH (RightId int, ApplicationId int, RightName nvarchar(MAX), RightDescription nvarchar(MAX), IsObsolete bit)
	)
	
	-- Собственно слияние:
	MERGE dbo.Rights AS trg
	USING src
	ON (trg.RightID = src.RightID)
	WHEN MATCHED 
		THEN UPDATE
			SET 	ApplicationID = src.ApplicationID,
				RightName = src.RightName, 
				RightDescription = src.RightDescription,
				IsObsolete = src.IsObsolete
	WHEN NOT MATCHED 
		THEN INSERT (ApplicationID, RightName, RightDescription, IsObsolete)
			VALUES (src.ApplicationID, src.RightName, src.RightDescription, src.IsObsolete)

	OUTPUT $action, inserted.RightID INTO #insertedData;

	-- Получение списка идентификаторов для вновь добавленных записей:
	SELECT  @insertedId = STRING_AGG(RightID, CHAR(13))
	FROM #insertedData
	WHERE Action = 'INSERT'
	GROUP BY Action

	DROP TABLE #insertedData

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[umc_MergeRights] TO [gdl_user]
    AS [dbo];

