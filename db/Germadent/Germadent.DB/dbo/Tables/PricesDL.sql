CREATE TABLE [dbo].[PricesDL] (
    [PricePositionID] INT   NOT NULL,
    [DateBegin]       DATE  NOT NULL,
    [DateEnd]         DATE  NULL,
    [Price]           MONEY NOT NULL,
    CONSTRAINT [PK_PricesDL] PRIMARY KEY CLUSTERED ([PricePositionID] ASC),
    CONSTRAINT [FK_PricesDL_PricePositions] FOREIGN KEY ([PricePositionID]) REFERENCES [dbo].[PricePositions] ([PricePositionID])
);



