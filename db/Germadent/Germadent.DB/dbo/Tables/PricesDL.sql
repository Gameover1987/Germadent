CREATE TABLE [dbo].[PricesDL] (
    [ServiceID] INT   NOT NULL,
    [DateBegin] DATE  NOT NULL,
    [DateEnd]   DATE  NULL,
    [Price]     MONEY NOT NULL,
    CONSTRAINT [PK_PricesDL] PRIMARY KEY CLUSTERED ([ServiceID] ASC),
    CONSTRAINT [FK_PricesDL_Services] FOREIGN KEY ([ServiceID]) REFERENCES [dbo].[Serv] ([ServiceID])
);

