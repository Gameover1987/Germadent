CREATE TABLE [dbo].[Users] (
    [UserID]      INT            IDENTITY (1, 1) NOT NULL,
    [Login]       NVARCHAR (MAX) NOT NULL,
    [Password]    NVARCHAR (MAX) NOT NULL,
    [FamilyName]  NVARCHAR (MAX) NOT NULL,
    [FirstName]   NVARCHAR (MAX) NOT NULL,
    [Patronymic]  NVARCHAR (MAX) NULL,
    [Phone]       NVARCHAR (50)  NULL,
    [IsLocked]    BIT            CONSTRAINT [DF_Users_FlagLock] DEFAULT ((0)) NULL,
    [Description] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([UserID] ASC),
    CONSTRAINT [IX_Users] UNIQUE NONCLUSTERED ([UserID] ASC)
);




















GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Обеспечивает уникальность логина пользователя', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Users', @level2type = N'CONSTRAINT', @level2name = N'IX_Users';

