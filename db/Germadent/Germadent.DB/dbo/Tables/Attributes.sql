CREATE TABLE [dbo].[Attributes] (
    [AttributeID]      INT            IDENTITY (1, 1) NOT NULL,
    [AttributeKeyName] NVARCHAR (70)  NOT NULL,
    [AttributeName]    NVARCHAR (100) NOT NULL,
    [Priority]         TINYINT        NULL,
    [IsObsolete]       BIT            NULL,
    CONSTRAINT [PK_QualitativeAttributes] PRIMARY KEY CLUSTERED ([AttributeID] ASC),
    CONSTRAINT [IX_QualitativeAttributesKeyNames] UNIQUE NONCLUSTERED ([AttributeKeyName] ASC),
    CONSTRAINT [IX_QualitativeAttributesNames] UNIQUE NONCLUSTERED ([AttributeName] ASC)
);









