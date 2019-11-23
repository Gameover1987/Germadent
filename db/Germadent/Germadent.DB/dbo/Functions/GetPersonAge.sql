-- =============================================
-- Author:		Alexey Kolosenok
-- Create date: 12.11.2019
-- Description:	Вычисление полных лет на текущую дату
-- =============================================
CREATE FUNCTION [dbo].[GetPersonAge] 
(	
	--Дата рождения
	@birthday datetime
)
RETURNS int
AS
BEGIN
	
	DECLARE 
		@age int,
		@thisYearBirthday datetime -- день рпождения в текущем году

	SET @age = DATEDIFF(YEAR, @birthday, GETDATE());
	SET @thisYearBirthday = DATEADD(YEAR, @age, @birthday);

	IF @thisYearBirthday > GETDATE() -- если днюхи ещё не было, тогда...
		SET @age = @age - 1
	
	RETURN @age

END