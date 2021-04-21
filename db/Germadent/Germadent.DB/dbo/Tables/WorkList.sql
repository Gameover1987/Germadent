CREATE TABLE [dbo].[WorkList] (
    [WorkOrderID]           INT      NOT NULL,
    [ProductID]             INT      NOT NULL,
    [TechnologyOperationID] INT      NOT NULL,
    [EmployeeID]            INT      NOT NULL,
    [Rate]                  MONEY    NOT NULL,
    [Quantity]              INT      NOT NULL,
    [OperationCost]         AS       ([Rate]*[Quantity]),
    [Started]               DATETIME NULL,
    [Ended]                 DATETIME NULL,
    [IsChecked]             BIT      NULL,
    CONSTRAINT [FK_WorkList_Products] FOREIGN KEY ([ProductID]) REFERENCES [dbo].[Products] ([ProductID]),
    CONSTRAINT [FK_WorkList_TechnologyOperations] FOREIGN KEY ([TechnologyOperationID]) REFERENCES [dbo].[TechnologyOperations] ([TechnologyOperationID]),
    CONSTRAINT [FK_WorkList_Users] FOREIGN KEY ([EmployeeID]) REFERENCES [dbo].[Users] ([UserID]),
    CONSTRAINT [FK_WorkList_WorkOrder] FOREIGN KEY ([WorkOrderID]) REFERENCES [dbo].[WorkOrder] ([WorkOrderID]) ON DELETE CASCADE
);




GO
CREATE NONCLUSTERED INDEX [IX_WorkOperations]
    ON [dbo].[WorkList]([WorkOrderID] ASC, [TechnologyOperationID] ASC);

