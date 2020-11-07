CREATE TABLE [dbo].[TechnologyOperations] (
    [TechnologyOperationID]   INT            IDENTITY (1, 1) NOT NULL,
    [TechnologyOperationName] NVARCHAR (250) NOT NULL,
    [EmployeePositionID]      INT            NOT NULL,
    [GroupName]               NVARCHAR (50)  NOT NULL,
    CONSTRAINT [PK_Works] PRIMARY KEY CLUSTERED ([TechnologyOperationID] ASC),
    CONSTRAINT [FK_TechnologyOperations_EmployeePositions] FOREIGN KEY ([EmployeePositionID]) REFERENCES [dbo].[EmployeePositions] ([EmployeePositionID])
);





