CREATE TABLE [dbo].[Attributes] (
    [AttributeID]      INT           IDENTITY (1, 1) NOT NULL,
    [AttributeKeyName] VARCHAR (50)  NOT NULL,
    [AttributeName]    NVARCHAR (70) NOT NULL,
    [Priority]         TINYINT       NULL,
    [Purpose]          VARCHAR (10)  NULL,
    [FlagRequired]     BIT           CONSTRAINT [DF_Attributes_FlagRequired] DEFAULT ((0)) NULL,
    [FlagObsolete]     BIT           NULL,
    CONSTRAINT [PK_QualitativeAttributes] PRIMARY KEY CLUSTERED ([AttributeID] ASC),
    CONSTRAINT [IX_QualitativeAttributesKeyNames] UNIQUE NONCLUSTERED ([AttributeKeyName] ASC),
    CONSTRAINT [IX_QualitativeAttributesNames] UNIQUE NONCLUSTERED ([AttributeName] ASC)
);







