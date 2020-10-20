﻿-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 19.10.2020
-- Description:	Добавление пользователя
-- =============================================
CREATE PROCEDURE [dbo].[umc_AddUser] 
	
	@familyName nvarchar(MAX),
	@firstName nvarchar(MAX),
	@patronymic nvarchar(MAX) = NULL,
	@phone nvarchar(MAX) = NULL,
	@login nvarchar(MAX),
	@password nvarchar(MAX),
	@isLocked bit = NULL,
	@description nvarchar(MAX) = NULL,
	@userId int output

AS
BEGIN
	
	SET NOCOUNT ON;

	-- Чтобы неоправданно не возрастало значение Id в ключевом поле - сначала его "подбивка":
	BEGIN
		DECLARE @max_Id int
		SELECT @max_Id = ISNULL(MAX(UserID), 0)
		FROM Users

		EXEC IdentifierAlignment Users, @max_Id
	
		REVERT
	END
    

	INSERT INTO Users
	(Login, Password, FamilyName, FirstName, Patronymic, Phone, IsLocked, Description)
	VALUES
	(@login, @password, @familyName, @firstName, @patronymic, @phone, @isLocked, @description)

	SET @userId = SCOPE_IDENTITY()

END