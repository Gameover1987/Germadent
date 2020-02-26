-- =============================================
-- Author:		Alexey Kolosenok
-- Create date: 25.02.2020
-- Description:	Создание нового пациента
-- =============================================
CREATE PROCEDURE [dbo].[AddNewPatient]
	@familyName nvarchar(35) = NULL,
	@name nvarchar(30) = NULL,
	@patronymic nvarchar(30) = NULL,
	@gender bit = NULL,
	@birthday date = NULL,
	@patientId int output

AS
BEGIN
	
	SET NOCOUNT ON;

	-- Чтобы неоправданно не возрастало значение Id в ключевом поле - сначала его "подбивка":
	BEGIN
		DECLARE @max_Id int
		SELECT @max_Id = ISNULL(MAX(PatientID), 0)
		FROM Patients

		EXEC IdentifierAlignment Patients, @max_Id
	
		REVERT
	END

	-- Собственно вставка:	
    INSERT INTO Patients
	(FamilyName, Name, Patronymic, Gender, Birthday)
	VALUES
	(@familyName, @name, @patronymic, @gender, @birthday)

	SET @patientId = SCOPE_IDENTITY()

END