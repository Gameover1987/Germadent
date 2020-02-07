CREATE TABLE [dbo].[QualitativeAttributes] (
    [AttributeID]   INT           IDENTITY (1, 1) NOT NULL,
    [AttributeName] NVARCHAR (70) NOT NULL,
    CONSTRAINT [PK_QualitativeAttributes] PRIMARY KEY CLUSTERED ([AttributeID] ASC)
);

