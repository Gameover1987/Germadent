CREATE TABLE [dbo].[PricesMC] (
    [ServiceID]  INT   NOT NULL,
    [DateBegin]  DATE  NOT NULL,
    [DateEnd]    DATE  NULL,
    [PriceSTL]   MONEY NOT NULL,
    [PriceModel] MONEY NOT NULL,
    CONSTRAINT [PK_PricesMC] PRIMARY KEY CLUSTERED ([ServiceID] ASC),
    CONSTRAINT [FK_PricesMC_Services] FOREIGN KEY ([ServiceID]) REFERENCES [dbo].[Serv] ([ServiceID])
);

