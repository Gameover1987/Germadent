CREATE TABLE [dbo].[ProductSet] (
    [PricePositionID] INT NOT NULL,
    [ProductID]       INT NOT NULL,
    CONSTRAINT [FK_ProductSet_PricePositions] FOREIGN KEY ([PricePositionID]) REFERENCES [dbo].[PricePositions] ([PricePositionID]),
    CONSTRAINT [FK_ProductSet_Product] FOREIGN KEY ([ProductID]) REFERENCES [dbo].[TypesOfProsthetics] ([ProstheticsID])
);








GO
CREATE NONCLUSTERED INDEX [IX_ProductSet_ProductID]
    ON [dbo].[ProductSet]([ProductID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ProductSet_PricePositionID]
    ON [dbo].[ProductSet]([PricePositionID] ASC);

