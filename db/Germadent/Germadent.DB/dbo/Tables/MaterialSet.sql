CREATE TABLE [dbo].[MaterialSet] (
    [PricePositionID] INT NOT NULL,
    [MaterialID]      INT NOT NULL,
    CONSTRAINT [FK_MaterialSet_Materials] FOREIGN KEY ([MaterialID]) REFERENCES [dbo].[Materials] ([MaterialID]),
    CONSTRAINT [FK_MaterialSet_PricePositions] FOREIGN KEY ([PricePositionID]) REFERENCES [dbo].[PricePositions] ([PricePositionID])
);


GO
CREATE NONCLUSTERED INDEX [IX_MaterialSet]
    ON [dbo].[MaterialSet]([PricePositionID] ASC, [MaterialID] ASC);

