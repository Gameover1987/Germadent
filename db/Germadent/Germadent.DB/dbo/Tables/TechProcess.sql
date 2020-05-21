CREATE TABLE [dbo].[TechProcess] (
    [WorkOrderID] INT      NOT NULL,
    [WorkID]      INT      NOT NULL,
    [EmployeeID]  INT      NULL,
    [Started]     DATETIME NULL,
    [Ended]       DATETIME NULL,
    [IsChecked]   BIT      NULL,
    CONSTRAINT [FK_TechProcess_Employee] FOREIGN KEY ([EmployeeID]) REFERENCES [dbo].[Employee] ([EmployeeID]),
    CONSTRAINT [FK_TechProcess_Works] FOREIGN KEY ([WorkID]) REFERENCES [dbo].[Works] ([WorkID])
);


GO
CREATE NONCLUSTERED INDEX [IX_WorkOperations]
    ON [dbo].[TechProcess]([WorkOrderID] ASC, [WorkID] ASC);

