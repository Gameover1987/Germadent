CREATE TABLE [dbo].[Users] (
    [UserID]     INT           IDENTITY (1, 1) NOT NULL,
    [EmployeeID] INT           NOT NULL,
    [Login]      NVARCHAR (30) NOT NULL,
    [Password]   NVARCHAR (10) NULL,
    [FlagLock]   BIT           NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([UserID] ASC),
    CONSTRAINT [FK_Users_Employee] FOREIGN KEY ([EmployeeID]) REFERENCES [dbo].[Employee] ([EmployeeID]),
    CONSTRAINT [IX_Users] UNIQUE NONCLUSTERED ([UserID] ASC)
);










GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Обеспечивает уникальность логина пользователя', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Users', @level2type = N'CONSTRAINT', @level2name = N'IX_Users';

