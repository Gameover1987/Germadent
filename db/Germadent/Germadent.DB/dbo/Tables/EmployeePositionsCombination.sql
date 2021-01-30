CREATE TABLE [dbo].[EmployeePositionsCombination] (
    [EmployeeID]         INT     NOT NULL,
    [EmployeePositionID] INT     NOT NULL,
    [QualifyingRank]     TINYINT NOT NULL,
    CONSTRAINT [FK_EmployeePositionsCombination_EmployeePositions] FOREIGN KEY ([EmployeePositionID]) REFERENCES [dbo].[EmployeePositions] ([EmployeePositionID]),
    CONSTRAINT [FK_EmployeePositionsCombination_Users] FOREIGN KEY ([EmployeeID]) REFERENCES [dbo].[Users] ([UserID]) ON DELETE CASCADE
);





