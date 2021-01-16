CREATE TABLE [dbo].[Rates] (
    [TechnologyOperationID] INT     NOT NULL,
    [QualifyingRank]        TINYINT NULL,
    [Rate]                  MONEY   NOT NULL,
    [DateBeginning]         DATE    NOT NULL,
    [DateEnd]               DATE    NULL,
    CONSTRAINT [FK_Rates_TechnologyOperations] FOREIGN KEY ([TechnologyOperationID]) REFERENCES [dbo].[TechnologyOperations] ([TechnologyOperationID])
);










GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Rates]
    ON [dbo].[Rates]([TechnologyOperationID] ASC, [QualifyingRank] ASC);

