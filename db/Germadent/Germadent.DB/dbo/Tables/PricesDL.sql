﻿CREATE TABLE [dbo].[PricesDL] (
    [PricePositionID] INT   NOT NULL,
    [DateBegin]       DATE  NOT NULL,
    [DateEnd]         DATE  NULL,
    [Price]           MONEY NOT NULL,
    CONSTRAINT [FK_PricesDL_PricePositions] FOREIGN KEY ([PricePositionID]) REFERENCES [dbo].[PricePositions] ([PricePositionID])
);












GO
CREATE NONCLUSTERED INDEX [IX_PricesDL_PricePositionID]
    ON [dbo].[PricesDL]([PricePositionID] ASC);

