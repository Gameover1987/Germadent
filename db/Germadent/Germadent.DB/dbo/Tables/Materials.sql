﻿CREATE TABLE [dbo].[Materials] (
    [MaterialID]   INT            IDENTITY (1, 1) NOT NULL,
    [MaterialName] NVARCHAR (150) NOT NULL,
    [FlagUnused]   BIT            CONSTRAINT [DF_Materials_Used] DEFAULT (NULL) NULL,
    CONSTRAINT [PK_Materials] PRIMARY KEY CLUSTERED ([MaterialID] ASC),
    CONSTRAINT [IX_MaterialName] UNIQUE NONCLUSTERED ([MaterialName] ASC)
);









