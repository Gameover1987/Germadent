CREATE TABLE [dbo].[QualitativeValues] (
    [QValueID]         INT            IDENTITY (1, 1) NOT NULL,
    [AttributeID]      INT            NOT NULL,
    [QualitativeValue] NVARCHAR (100) NOT NULL,
    CONSTRAINT [PK_QualitativeValues] PRIMARY KEY CLUSTERED ([QValueID] ASC),
    CONSTRAINT [FK_QualitativeValues_QualitativeAttributes] FOREIGN KEY ([AttributeID]) REFERENCES [dbo].[QualitativeAttributes] ([AttributeID])
);

