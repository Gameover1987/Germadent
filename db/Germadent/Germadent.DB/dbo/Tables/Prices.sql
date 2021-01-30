CREATE TABLE [dbo].[Prices] (
    [PricePositionID] INT   NOT NULL,
    [DateBeginning]   DATE  NOT NULL,
    [PriceSTL]        MONEY NULL,
    [PriceModel]      MONEY NOT NULL,
    [DateEnd]         DATE  NULL,
    CONSTRAINT [FK_PricesMC_PricePositions] FOREIGN KEY ([PricePositionID]) REFERENCES [dbo].[PricePositions] ([PricePositionID]) ON DELETE CASCADE
);






GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_PricesMC_PricePositionID]
    ON [dbo].[Prices]([PricePositionID] ASC, [DateBeginning] ASC);





