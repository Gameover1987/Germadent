CREATE TABLE [dbo].[PricesMC] (
    [PricePositionID] INT  NOT NULL,
    [DateBegin]       DATE NOT NULL,
    [DateEnd]         DATE NULL,
    [PriceSTL]        INT  NULL,
    [PriceModel]      INT  NOT NULL,
    CONSTRAINT [PK_PricesMC] PRIMARY KEY CLUSTERED ([PricePositionID] ASC),
    CONSTRAINT [FK_PricesMC_PricePositions] FOREIGN KEY ([PricePositionID]) REFERENCES [dbo].[PricePositions] ([PricePositionID])
);







