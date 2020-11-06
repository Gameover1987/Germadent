CREATE TABLE [dbo].[ToothCard] (
    [WorkOrderID]     INT     NOT NULL,
    [ToothNumber]     TINYINT NULL,
    [PricePositionID] INT     NULL,
    [ConditionID]     INT     NULL,
    [MaterialID]      INT     NULL,
    [ProstheticsID]   INT     NULL,
    [Price]           MONEY   NULL,
    [HasBridge]       BIT     NULL,
    CONSTRAINT [FK_ToothCard_ConditionsOfProsthetics] FOREIGN KEY ([ConditionID]) REFERENCES [dbo].[ConditionsOfProsthetics] ([ConditionID]),
    CONSTRAINT [FK_ToothCard_Materials] FOREIGN KEY ([MaterialID]) REFERENCES [dbo].[Materials] ([MaterialID]),
    CONSTRAINT [FK_ToothCard_PricePositions] FOREIGN KEY ([PricePositionID]) REFERENCES [dbo].[PricePositions] ([PricePositionID]),
    CONSTRAINT [FK_ToothCard_Product] FOREIGN KEY ([ProstheticsID]) REFERENCES [dbo].[TypesOfProsthetics] ([ProstheticsID]),
    CONSTRAINT [FK_ToothCard_WorkOrder] FOREIGN KEY ([WorkOrderID]) REFERENCES [dbo].[WorkOrder] ([WorkOrderID]) ON DELETE CASCADE,
    CONSTRAINT [IX_ToothCard] UNIQUE CLUSTERED ([WorkOrderID] ASC, [ToothNumber] ASC, [PricePositionID] ASC)
);


















































GO
CREATE NONCLUSTERED INDEX [IX_ToothCard_PricePositionID]
    ON [dbo].[ToothCard]([PricePositionID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ToothCard_ProdictID]
    ON [dbo].[ToothCard]([ProstheticsID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ToothCard_MaterialID]
    ON [dbo].[ToothCard]([MaterialID] ASC);

