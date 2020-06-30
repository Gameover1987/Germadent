CREATE TABLE [dbo].[ConstructionColors] (
    [ConstructionColorID]   INT           IDENTITY (1, 1) NOT NULL,
    [ConstructionColorName] NVARCHAR (30) NOT NULL,
    CONSTRAINT [PK_ConstructionColors] PRIMARY KEY CLUSTERED ([ConstructionColorID] ASC)
);

