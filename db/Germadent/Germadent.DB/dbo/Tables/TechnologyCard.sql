CREATE TABLE [dbo].[TechnologyCard] (
    [ProductID]             INT NOT NULL,
    [BranchTypeID]          INT NOT NULL,
    [TechnologyOperationID] INT NOT NULL,
    [ParentOperationID]     INT NULL,
    CONSTRAINT [FK_TechnologyCard_BranchTypes] FOREIGN KEY ([BranchTypeID]) REFERENCES [dbo].[BranchTypes] ([BranchTypeID]),
    CONSTRAINT [FK_TechnologyCard_Product] FOREIGN KEY ([ProductID]) REFERENCES [dbo].[Products] ([ProductID]),
    CONSTRAINT [FK_TechnologyCard_TechnologyOperations] FOREIGN KEY ([ProductID]) REFERENCES [dbo].[TechnologyOperations] ([TechnologyOperationID])
);










GO
CREATE NONCLUSTERED INDEX [IX_TechnologyCard_TechnologyOperationID]
    ON [dbo].[TechnologyCard]([TechnologyOperationID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TechnologyCard_ProductID]
    ON [dbo].[TechnologyCard]([ProductID] ASC);

