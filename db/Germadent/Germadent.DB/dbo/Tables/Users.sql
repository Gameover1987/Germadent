CREATE TABLE [dbo].[Users] (
    [UserID]      INT            IDENTITY (1, 1) NOT NULL,
    [Login]       NVARCHAR (100) NOT NULL,
    [Password]    NVARCHAR (MAX) NOT NULL,
    [FamilyName]  NVARCHAR (MAX) NOT NULL,
    [FirstName]   NVARCHAR (MAX) NOT NULL,
    [Patronymic]  NVARCHAR (MAX) NULL,
    [Phone]       NVARCHAR (50)  NULL,
    [IsLocked]    BIT            CONSTRAINT [DF_Users_FlagLock] DEFAULT ((0)) NULL,
    [Description] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([UserID] ASC),
    CONSTRAINT [IX_Users_Login] UNIQUE NONCLUSTERED ([Login] ASC)
);






















GO


