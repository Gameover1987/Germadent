﻿CREATE TABLE [dbo].[PricePositions] (
    [PricePositionID]    INT            IDENTITY (1, 1) NOT NULL,
    [PriceGroupID]       INT            NOT NULL,
    [PricePositionCode]  NVARCHAR (50)  NOT NULL,
    [PricePositionName]  NVARCHAR (MAX) NOT NULL,
    [MaterialID]         INT            NULL,
    [IsObsoletePosition] BIT            NULL,
    CONSTRAINT [PK_PriceGroups] PRIMARY KEY CLUSTERED ([PricePositionID] ASC),
    CONSTRAINT [FK_PricePositions_PriceGroups] FOREIGN KEY ([PriceGroupID]) REFERENCES [dbo].[PriceGroups] ([PriceGroupID]),
    CONSTRAINT [UK_PricePositionCode] UNIQUE NONCLUSTERED ([PricePositionCode] ASC)
);
















GO
CREATE NONCLUSTERED INDEX [IX_ServiceGroup]
    ON [dbo].[PricePositions]([PriceGroupID] ASC);




GO
CREATE NONCLUSTERED INDEX [IX_Material]
    ON [dbo].[PricePositions]([MaterialID] ASC);

