CREATE TABLE [dbo].[UsersAndRoles] (
    [UserID] INT NOT NULL,
    [RoleID] INT NOT NULL,
    CONSTRAINT [FK_UsersAndRoles_Roles] FOREIGN KEY ([RoleID]) REFERENCES [dbo].[Roles] ([RoleID]),
    CONSTRAINT [FK_UsersAndRoles_Users] FOREIGN KEY ([UserID]) REFERENCES [dbo].[Users] ([UserID]) ON DELETE CASCADE,
    CONSTRAINT [IX_UsersAndRoles] UNIQUE NONCLUSTERED ([UserID] ASC, [RoleID] ASC)
);





