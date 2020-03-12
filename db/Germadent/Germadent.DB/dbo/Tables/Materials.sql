CREATE TABLE [dbo].[Materials] (
    [MaterialID]   INT           IDENTITY (1, 1) NOT NULL,
    [MaterialName] NVARCHAR (30) NOT NULL,
    [FlagUnused]   BIT           CONSTRAINT [DF_Materials_Used] DEFAULT (NULL) NULL,
    CONSTRAINT [PK_Materials] PRIMARY KEY CLUSTERED ([MaterialID] ASC)
);





