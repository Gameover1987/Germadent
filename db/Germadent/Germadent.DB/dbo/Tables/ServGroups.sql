CREATE TABLE [dbo].[ServGroups] (
    [ServiceGroupID]   INT            IDENTITY (1, 1) NOT NULL,
    [ServiceGroupName] NVARCHAR (200) NOT NULL,
    [BranchTypeID]     INT            NOT NULL,
    CONSTRAINT [PK_ServiceGroups] PRIMARY KEY CLUSTERED ([ServiceGroupID] ASC)
);



