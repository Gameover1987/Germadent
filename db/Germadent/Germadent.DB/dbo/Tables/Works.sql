CREATE TABLE [dbo].[Works] (
    [WorkOrderID]           INT      NOT NULL,
    [ProductID]             INT      NOT NULL,
    [TechnologyOperationID] INT      NOT NULL,
    [EmployeeID]            INT      NOT NULL,
    [OperationCost]         MONEY    NULL,
    [Started]               DATETIME NULL,
    [Ended]                 DATETIME NULL,
    [IsChecked]             BIT      NULL,
    CONSTRAINT [FK_Works_Products] FOREIGN KEY ([ProductID]) REFERENCES [dbo].[Products] ([ProductID]),
    CONSTRAINT [FK_Works_TechnologyOperations] FOREIGN KEY ([TechnologyOperationID]) REFERENCES [dbo].[TechnologyOperations] ([TechnologyOperationID]),
    CONSTRAINT [FK_Works_Users] FOREIGN KEY ([EmployeeID]) REFERENCES [dbo].[Users] ([UserID]),
    CONSTRAINT [FK_Works_WorkOrder] FOREIGN KEY ([WorkOrderID]) REFERENCES [dbo].[WorkOrder] ([WorkOrderID])
);










GO
CREATE NONCLUSTERED INDEX [IX_WorkOperations]
    ON [dbo].[Works]([WorkOrderID] ASC, [TechnologyOperationID] ASC);

