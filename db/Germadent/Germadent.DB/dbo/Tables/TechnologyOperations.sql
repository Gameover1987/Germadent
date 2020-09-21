CREATE TABLE [dbo].[TechnologyOperations] (
    [TechnologyOperationID]   INT           IDENTITY (1, 1) NOT NULL,
    [TechnologyOperationName] NVARCHAR (50) NOT NULL,
    [EmployeePosition]        NVARCHAR (30) NOT NULL,
    CONSTRAINT [PK_Works] PRIMARY KEY CLUSTERED ([TechnologyOperationID] ASC)
);

