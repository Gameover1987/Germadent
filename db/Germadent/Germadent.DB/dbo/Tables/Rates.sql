CREATE TABLE [dbo].[Rates] (
    [WorkID]    INT   NOT NULL,
    [Rate]      MONEY NOT NULL,
    [DateBegin] DATE  NOT NULL,
    [DateEnd]   DATE  NULL,
    CONSTRAINT [PK_Rates] PRIMARY KEY CLUSTERED ([WorkID] ASC),
    CONSTRAINT [FK_Rates_Works] FOREIGN KEY ([WorkID]) REFERENCES [dbo].[Works] ([WorkID])
);

