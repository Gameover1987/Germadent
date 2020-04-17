CREATE TABLE [dbo].[AttributesValues] (
    [AttrValueID]    INT            IDENTITY (1, 1) NOT NULL,
    [AttributeID]    INT            NOT NULL,
    [AttributeValue] NVARCHAR (100) NOT NULL,
    CONSTRAINT [PK_QualitativeValues] PRIMARY KEY CLUSTERED ([AttrValueID] ASC),
    CONSTRAINT [FK_AttributesValues_Attributes] FOREIGN KEY ([AttributeID]) REFERENCES [dbo].[Attributes] ([AttributeID])
);

