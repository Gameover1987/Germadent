CREATE TABLE [dbo].[BranchTypes] (
    [BranchTypeID] INT           IDENTITY (1, 1) NOT NULL,
    [BranchType]   NVARCHAR (10) NOT NULL,
    CONSTRAINT [PK_Branches] PRIMARY KEY CLUSTERED ([BranchTypeID] ASC)
);

