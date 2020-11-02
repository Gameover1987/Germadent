CREATE TABLE [dbo].[PricesDL] (
    [PricePositionID] INT   NOT NULL,
    [DateBegin]       DATE  NOT NULL,
    [DateEnd]         DATE  NULL,
    [Price]           MONEY NOT NULL
);














GO
CREATE NONCLUSTERED INDEX [IX_PricesDL_PricePositionID]
    ON [dbo].[PricesDL]([PricePositionID] ASC);

