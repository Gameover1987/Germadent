CREATE TABLE [dbo].[ToothCard] (
    [WorkOrderID] INT     NOT NULL,
    [ToothNumber] TINYINT NOT NULL,
    [ServiceID]   INT     NULL,
    [Price]       MONEY   NULL,
    [FlagBridge]  BIT     NULL,
    CONSTRAINT [FK_ToothCard_Services] FOREIGN KEY ([ServiceID]) REFERENCES [dbo].[Servicess] ([ServiceID]),
    CONSTRAINT [FK_ToothCard_WorkOrder] FOREIGN KEY ([WorkOrderID]) REFERENCES [dbo].[WorkOrder] ([WorkOrderID]) ON DELETE CASCADE,
    CONSTRAINT [IX_ToothCard] UNIQUE CLUSTERED ([WorkOrderID] ASC, [ToothNumber] ASC)
);


































GO


