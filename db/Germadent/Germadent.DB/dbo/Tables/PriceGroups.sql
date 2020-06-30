CREATE TABLE [dbo].[PriceGroups] (
    [PriceGroupID]   INT          IDENTITY (1, 1) NOT NULL,
    [BranchTypeID]   INT          NOT NULL,
    [PriceGroupCode] VARCHAR (20) NOT NULL,
    CONSTRAINT [PK_PriceGroups] PRIMARY KEY CLUSTERED ([PriceGroupID] ASC),
    CONSTRAINT [FK_PriceGroups_BranchTypes] FOREIGN KEY ([BranchTypeID]) REFERENCES [dbo].[BranchTypes] ([BranchTypeID])
);







