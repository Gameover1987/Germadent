CREATE TABLE [dbo].[RolesAndRights] (
    [RoleID]  INT NOT NULL,
    [RightID] INT NOT NULL,
    CONSTRAINT [FK_RolesAndRights_Rights] FOREIGN KEY ([RightID]) REFERENCES [dbo].[Rights] ([RightID]),
    CONSTRAINT [FK_RolesAndRights_Roles] FOREIGN KEY ([RoleID]) REFERENCES [dbo].[Roles] ([RoleID]) ON DELETE CASCADE,
    CONSTRAINT [IX_RolesAndRights] UNIQUE NONCLUSTERED ([RoleID] ASC, [RightID] ASC)
);

