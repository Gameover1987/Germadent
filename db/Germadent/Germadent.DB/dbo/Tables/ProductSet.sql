CREATE TABLE [dbo].[ProductSet] (
    [PricePositionID] INT NULL,
    [ProductID]       INT NULL,
    CONSTRAINT [FK_ProductSet_PricePositions] FOREIGN KEY ([PricePositionID]) REFERENCES [dbo].[PricePositions] ([PricePositionID]),
    CONSTRAINT [FK_ProductSet_Product] FOREIGN KEY ([ProductID]) REFERENCES [dbo].[Products] ([ProductID])
);














GO



GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_ProductSet]
    ON [dbo].[ProductSet]([PricePositionID] ASC, [ProductID] ASC);

