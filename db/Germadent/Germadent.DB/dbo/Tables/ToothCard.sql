CREATE TABLE [dbo].[ToothCard] (
    [WorkOrderID]   INT     NOT NULL,
    [ToothNumber]   TINYINT NOT NULL,
    [MaterialID]    INT     NULL,
    [ProstheticsID] INT     NULL,
    [FlagBridge]    BIT     NULL,
    CONSTRAINT [FK_ToothCard_WorkOrder] FOREIGN KEY ([WorkOrderID]) REFERENCES [dbo].[WorkOrder] ([WorkOrderID]),
    CONSTRAINT [IX_ToothCard] UNIQUE CLUSTERED ([WorkOrderID] ASC, [ToothNumber] ASC)
);












GO


