CREATE TABLE [dbo].[TechnologyOperations] (
    [TechnologyOperationID]       INT            IDENTITY (1, 1) NOT NULL,
    [TechnologyOperationUserCode] NVARCHAR (20)  NULL,
    [TechnologyOperationName]     NVARCHAR (250) NOT NULL,
    [EmployeePositionID]          INT            NOT NULL,
    [GroupName]                   NVARCHAR (50)  NULL,
    CONSTRAINT [PK_Works] PRIMARY KEY CLUSTERED ([TechnologyOperationID] ASC),
    CONSTRAINT [FK_TechnologyOperations_EmployeePositions] FOREIGN KEY ([EmployeePositionID]) REFERENCES [dbo].[EmployeePositions] ([EmployeePositionID])
);







