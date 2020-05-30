CREATE TABLE [dbo].[AttrValues] (
    [AttrValueID]    INT            IDENTITY (1, 1) NOT NULL,
    [AttributeID]    INT            NOT NULL,
    [AttributeValue] NVARCHAR (100) NOT NULL,
    [FlagObsolete]   BIT            NULL,
    CONSTRAINT [PK_QualitativeValues] PRIMARY KEY CLUSTERED ([AttrValueID] ASC),
    CONSTRAINT [FK_AttrValues_Attributes] FOREIGN KEY ([AttributeID]) REFERENCES [dbo].[Attributes] ([AttributeID])
);

