CREATE TABLE [dbo].[Employee] (
    [EmployeeID] INT           IDENTITY (1, 1) NOT NULL,
    [FamilyName] NVARCHAR (30) NOT NULL,
    [Name]       NVARCHAR (30) NOT NULL,
    [Patronymic] NVARCHAR (30) NULL,
    [Phone]      NVARCHAR (15) NULL,
    CONSTRAINT [PK_Employee] PRIMARY KEY CLUSTERED ([EmployeeID] ASC)
);

