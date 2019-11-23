CREATE TABLE [dbo].[Materials] (
    [MaterialID]   INT           IDENTITY (1, 1) NOT NULL,
    [MaterialName] NVARCHAR (30) NULL,
    [FlagUsed]     BIT           CONSTRAINT [DF_Materials_Used] DEFAULT ((1)) NULL,
    CONSTRAINT [PK_Materials] PRIMARY KEY CLUSTERED ([MaterialID] ASC)
);

