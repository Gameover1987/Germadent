CREATE TABLE [dbo].[PricePositions] (
    [PricePositionID]   INT            IDENTITY (1, 1) NOT NULL,
    [ServiceGroupID]    INT            NULL,
    [PricePositionCode] VARCHAR (20)   NOT NULL,
    [PricePositionName] NVARCHAR (MAX) NULL,
    [MaterialID]        INT            NULL,
    CONSTRAINT [PK_PriceGroups] PRIMARY KEY CLUSTERED ([PricePositionID] ASC),
    CONSTRAINT [FK_PricePositions_Materials] FOREIGN KEY ([MaterialID]) REFERENCES [dbo].[Materials] ([MaterialID]),
    CONSTRAINT [FK_PricePositions_ServicesGroups] FOREIGN KEY ([ServiceGroupID]) REFERENCES [dbo].[ServicesGroups] ([ServiceGroupID])
);




GO
CREATE NONCLUSTERED INDEX [IX_ServiceGroup]
    ON [dbo].[PricePositions]([ServiceGroupID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Material]
    ON [dbo].[PricePositions]([MaterialID] ASC);

