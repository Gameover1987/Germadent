CREATE TABLE [dbo].[StatusRoute] (
    [EmployeePositionID]    INT NULL,
    [StatusFrom]            INT NOT NULL,
    [StatusTo]              INT NOT NULL,
    [TechnologyOperationID] INT NULL,
    CONSTRAINT [FK_StatusRoute_EmployeePositions] FOREIGN KEY ([EmployeePositionID]) REFERENCES [dbo].[EmployeePositions] ([EmployeePositionID]),
    CONSTRAINT [FK_StatusRoute_StatusEnumeration] FOREIGN KEY ([StatusFrom]) REFERENCES [dbo].[StatusEnumeration] ([Status]),
    CONSTRAINT [FK_StatusRoute_StatusEnumeration1] FOREIGN KEY ([StatusTo]) REFERENCES [dbo].[StatusEnumeration] ([Status])
);



