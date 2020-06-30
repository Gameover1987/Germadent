CREATE TABLE [dbo].[AttributesSet] (
    [WorkOrderID] INT     NOT NULL,
    [ToothNumber] TINYINT NULL,
    [AttributeID] INT     NOT NULL,
    [AttrValueID] INT     NOT NULL,
    CONSTRAINT [FK_AttributesSet_Attributes] FOREIGN KEY ([AttributeID]) REFERENCES [dbo].[Attributes] ([AttributeID]),
    CONSTRAINT [FK_AttributesSet_AttrValues] FOREIGN KEY ([AttrValueID]) REFERENCES [dbo].[AttrValues] ([AttrValueID]),
    CONSTRAINT [FK_AttributesSet_WorkOrder] FOREIGN KEY ([WorkOrderID]) REFERENCES [dbo].[WorkOrder] ([WorkOrderID]) ON DELETE CASCADE,
    CONSTRAINT [IX_AttributesSet] UNIQUE NONCLUSTERED ([WorkOrderID] ASC, [AttributeID] ASC, [AttrValueID] ASC)
);







