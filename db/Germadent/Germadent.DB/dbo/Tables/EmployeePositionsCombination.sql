CREATE TABLE [dbo].[EmployeePositionsCombination] (
    [EmployeeID]         INT     NOT NULL,
    [EmployeePositionID] INT     NOT NULL,
    [QualifyingRank]     TINYINT NOT NULL,
    CONSTRAINT [FK_EmployeePositionsCombination_Employee] FOREIGN KEY ([EmployeeID]) REFERENCES [dbo].[Employee] ([EmployeeID]),
    CONSTRAINT [FK_EmployeePositionsCombination_EmployeePositions] FOREIGN KEY ([EmployeePositionID]) REFERENCES [dbo].[EmployeePositions] ([EmployeePositionID])
);

