CREATE TABLE [dbo].[PriceGroups] (
    [PriceGroupID]   INT            IDENTITY (1, 1) NOT NULL,
    [PriceGroupName] NVARCHAR (200) NOT NULL,
    [BranchTypeID]   INT            NOT NULL,
    CONSTRAINT [PK_ServiceGroups] PRIMARY KEY CLUSTERED ([PriceGroupID] ASC),
    CONSTRAINT [FK_PriceGroups_BranchTypes] FOREIGN KEY ([BranchTypeID]) REFERENCES [dbo].[BranchTypes] ([BranchTypeID])
);

