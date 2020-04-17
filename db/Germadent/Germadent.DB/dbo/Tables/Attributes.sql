CREATE TABLE [dbo].[Attributes] (
    [AttributeID]      INT           IDENTITY (1, 1) NOT NULL,
    [AttributeKeyName] VARCHAR (50)  NOT NULL,
    [AttributeName]    NVARCHAR (70) NOT NULL,
    CONSTRAINT [PK_QualitativeAttributes] PRIMARY KEY CLUSTERED ([AttributeID] ASC),
    CONSTRAINT [IX_QualitativeAttributesKeyNames] UNIQUE NONCLUSTERED ([AttributeKeyName] ASC),
    CONSTRAINT [IX_QualitativeAttributesNames] UNIQUE NONCLUSTERED ([AttributeName] ASC)
);

