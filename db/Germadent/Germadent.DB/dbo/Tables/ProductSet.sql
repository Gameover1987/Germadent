CREATE TABLE [dbo].[ProductSet] (
    [ProductID]       INT NOT NULL,
    [PricePositionID] INT NOT NULL,
    CONSTRAINT [FK_ProductSet_PricePositions] FOREIGN KEY ([PricePositionID]) REFERENCES [dbo].[PricePositions] ([PricePositionID]),
    CONSTRAINT [FK_ProductSet_Product] FOREIGN KEY ([ProductID]) REFERENCES [dbo].[Product] ([ProductID])
);




GO
CREATE NONCLUSTERED INDEX [IX_ProductSet_ProductID]
    ON [dbo].[ProductSet]([ProductID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ProductSet_PricePositionID]
    ON [dbo].[ProductSet]([PricePositionID] ASC);

