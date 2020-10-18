CREATE TABLE [dbo].[EmployeePositions] (
    [EmployeePositionID]   INT           IDENTITY (1, 1) NOT NULL,
    [EmployeePositionName] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_EmployeePositions] PRIMARY KEY CLUSTERED ([EmployeePositionID] ASC)
);

