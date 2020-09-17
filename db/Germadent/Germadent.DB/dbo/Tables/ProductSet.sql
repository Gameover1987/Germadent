CREATE TABLE [dbo].[ProductSet] (
    [ProductID]       INT NOT NULL,
    [PricePositionID] INT NOT NULL,
    CONSTRAINT [FK_ProductSet_PricePositions] FOREIGN KEY ([PricePositionID]) REFERENCES [dbo].[PricePositions] ([PricePositionID]),
    CONSTRAINT [FK_ProductSet_Product] FOREIGN KEY ([ProductID]) REFERENCES [dbo].[Product] ([ProductID])
);

