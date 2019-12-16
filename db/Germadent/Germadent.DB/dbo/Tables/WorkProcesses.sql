CREATE TABLE [dbo].[WorkProcesses] (
    [WorkID]      INT            IDENTITY (1, 1) NOT NULL,
    [WorkOrderID] INT            NOT NULL,
    [EmployeeID]  INT            NOT NULL,
    [Status]      TINYINT        NULL,
    [Description] NVARCHAR (200) NULL,
    CONSTRAINT [FK_WorkProcesses_TypesOfWorks] FOREIGN KEY ([WorkID]) REFERENCES [dbo].[TypesOfWorks] ([WorkID]),
    CONSTRAINT [FK_Works_Employee] FOREIGN KEY ([EmployeeID]) REFERENCES [dbo].[Employee] ([EmployeeID]),
    CONSTRAINT [FK_Works_WorkOrder] FOREIGN KEY ([WorkOrderID]) REFERENCES [dbo].[WorkOrder] ([WorkOrderID])
);





