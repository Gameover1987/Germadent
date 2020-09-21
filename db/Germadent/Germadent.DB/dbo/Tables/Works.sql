CREATE TABLE [dbo].[Works] (
    [WorkOrderID]           INT      NOT NULL,
    [TechnologyOperationID] INT      NOT NULL,
    [EmployeeID]            INT      NULL,
    [OperationCost]         MONEY    NULL,
    [Started]               DATETIME NULL,
    [Ended]                 DATETIME NULL,
    [IsChecked]             BIT      NULL,
    CONSTRAINT [FK_Works_Employee] FOREIGN KEY ([EmployeeID]) REFERENCES [dbo].[Employee] ([EmployeeID]),
    CONSTRAINT [FK_Works_TechnologyOperations] FOREIGN KEY ([TechnologyOperationID]) REFERENCES [dbo].[TechnologyOperations] ([TechnologyOperationID]),
    CONSTRAINT [FK_Works_WorkOrder] FOREIGN KEY ([WorkOrderID]) REFERENCES [dbo].[WorkOrder] ([WorkOrderID])
);




GO
CREATE NONCLUSTERED INDEX [IX_WorkOperations]
    ON [dbo].[Works]([WorkOrderID] ASC, [TechnologyOperationID] ASC);

