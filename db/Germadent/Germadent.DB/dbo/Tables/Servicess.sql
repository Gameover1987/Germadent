CREATE TABLE [dbo].[Servicess] (
    [ServiceID]      INT            IDENTITY (1, 1) NOT NULL,
    [BranchTypeID]   INT            NOT NULL,
    [ServiceGroupID] INT            NOT NULL,
    [MaterialID]     INT            NOT NULL,
    [PriceGroupID]   INT            NOT NULL,
    [SeviceName]     NVARCHAR (MAX) NOT NULL,
    [FlagUnused]     BIT            NULL,
    CONSTRAINT [PK_Services] PRIMARY KEY CLUSTERED ([ServiceID] ASC),
    CONSTRAINT [FK_Services_BranchTypes] FOREIGN KEY ([BranchTypeID]) REFERENCES [dbo].[BranchTypes] ([BranchTypeID]),
    CONSTRAINT [FK_Services_Materials] FOREIGN KEY ([MaterialID]) REFERENCES [dbo].[Materials] ([MaterialID]),
    CONSTRAINT [FK_Services_PriceGroups] FOREIGN KEY ([PriceGroupID]) REFERENCES [dbo].[PriceGroups] ([PriceGroupID]),
    CONSTRAINT [FK_Services_ServicesGroups] FOREIGN KEY ([ServiceGroupID]) REFERENCES [dbo].[ServicesGroups] ([ServiceGroupID])
);


GO
CREATE NONCLUSTERED INDEX [IX_Services]
    ON [dbo].[Servicess]([ServiceGroupID] ASC);

