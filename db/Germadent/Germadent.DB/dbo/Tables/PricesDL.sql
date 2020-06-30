CREATE TABLE [dbo].[PricesDL] (
    [PriceGroupID] INT   NOT NULL,
    [DateBegin]    DATE  NOT NULL,
    [DateEnd]      DATE  NULL,
    [Price]        MONEY NOT NULL,
    CONSTRAINT [PK_PricesDL] PRIMARY KEY CLUSTERED ([PriceGroupID] ASC),
    CONSTRAINT [FK_PricesDL_PriceGroups] FOREIGN KEY ([PriceGroupID]) REFERENCES [dbo].[PriceGroups] ([PriceGroupID])
);



