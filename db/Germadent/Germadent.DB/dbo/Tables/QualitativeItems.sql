CREATE TABLE [dbo].[QualitativeItems] (
    [QitemID]         INT            IDENTITY (1, 1) NOT NULL,
    [AttributeID]     INT            NOT NULL,
    [QualitativeItem] NVARCHAR (100) NOT NULL,
    CONSTRAINT [PK_QualitativeItems] PRIMARY KEY CLUSTERED ([QitemID] ASC),
    CONSTRAINT [FK_QualitativeItems_QualitativeAttributes] FOREIGN KEY ([AttributeID]) REFERENCES [dbo].[QualitativeAttributes] ([AttributeID])
);

