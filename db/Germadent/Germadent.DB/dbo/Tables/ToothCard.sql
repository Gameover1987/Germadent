CREATE TABLE [dbo].[ToothCard] (
    [WorkOrderID]     INT     NOT NULL,
    [ToothNumber]     TINYINT NOT NULL,
    [PricePositionID] INT     NULL,
    [MaterialID]      INT     NULL,
    [ProductID]       INT     NULL,
    [Price]           MONEY   NULL,
    [FlagBridge]      BIT     NULL,
    CONSTRAINT [FK_ToothCard_Materials] FOREIGN KEY ([MaterialID]) REFERENCES [dbo].[Materials] ([MaterialID]),
    CONSTRAINT [FK_ToothCard_PricePositions] FOREIGN KEY ([PricePositionID]) REFERENCES [dbo].[PricePositions] ([PricePositionID]),
    CONSTRAINT [FK_ToothCard_Product] FOREIGN KEY ([ProductID]) REFERENCES [dbo].[Product] ([ProductID]),
    CONSTRAINT [FK_ToothCard_WorkOrder] FOREIGN KEY ([WorkOrderID]) REFERENCES [dbo].[WorkOrder] ([WorkOrderID]) ON DELETE CASCADE,
    CONSTRAINT [IX_ToothCard] UNIQUE CLUSTERED ([WorkOrderID] ASC, [ToothNumber] ASC)
);




































GO


