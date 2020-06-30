CREATE TABLE [dbo].[PricesMC] (
    [PriceGroupID] INT   NOT NULL,
    [DateBegin]    DATE  NOT NULL,
    [DateEnd]      DATE  NULL,
    [PriceSTL]     MONEY NOT NULL,
    [PriceModel]   MONEY NOT NULL,
    CONSTRAINT [PK_PricesMC] PRIMARY KEY CLUSTERED ([PriceGroupID] ASC),
    CONSTRAINT [FK_PricesMC_PriceGroups] FOREIGN KEY ([PriceGroupID]) REFERENCES [dbo].[PriceGroups] ([PriceGroupID])
);



