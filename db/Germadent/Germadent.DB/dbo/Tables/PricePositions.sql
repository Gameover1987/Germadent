﻿CREATE TABLE [dbo].[PricePositions] (
    [PricePositionID]   INT            IDENTITY (1, 1) NOT NULL,
    [PriceGroupID]      INT            NULL,
    [PricePositionCode] VARCHAR (20)   NOT NULL,
    [PricePositionName] NVARCHAR (MAX) NULL,
    [MaterialID]        INT            NULL,
    CONSTRAINT [PK_PriceGroups] PRIMARY KEY CLUSTERED ([PricePositionID] ASC),
    CONSTRAINT [FK_PricePositions_Materials] FOREIGN KEY ([MaterialID]) REFERENCES [dbo].[Materials] ([MaterialID]),
    CONSTRAINT [FK_PricePositions_PriceGroups] FOREIGN KEY ([PriceGroupID]) REFERENCES [dbo].[PriceGroups] ([PriceGroupID])
);






GO
CREATE NONCLUSTERED INDEX [IX_ServiceGroup]
    ON [dbo].[PricePositions]([PriceGroupID] ASC);




GO
CREATE NONCLUSTERED INDEX [IX_Material]
    ON [dbo].[PricePositions]([MaterialID] ASC);
