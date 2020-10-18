CREATE TABLE [dbo].[Rates] (
    [TechnologyOperationID] INT     NOT NULL,
    [QualifyingRank]        TINYINT NULL,
    [Rate]                  MONEY   NOT NULL,
    [DateBegin]             DATE    NOT NULL,
    [DateEnd]               DATE    NULL,
    CONSTRAINT [PK_Rates] PRIMARY KEY CLUSTERED ([TechnologyOperationID] ASC),
    CONSTRAINT [FK_Rates_TechnologyOperations] FOREIGN KEY ([TechnologyOperationID]) REFERENCES [dbo].[TechnologyOperations] ([TechnologyOperationID])
);





