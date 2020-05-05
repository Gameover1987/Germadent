CREATE TABLE [dbo].[AttributesSet] (
    [WorkOrderID] INT NOT NULL,
    [AttributeID] INT NOT NULL,
    [AttrValueID] INT NOT NULL,
    CONSTRAINT [FK_AttributesSet_Attributes] FOREIGN KEY ([AttributeID]) REFERENCES [dbo].[Attributes] ([AttributeID]),
    CONSTRAINT [FK_AttributesSet_AttributesValues] FOREIGN KEY ([AttrValueID]) REFERENCES [dbo].[AttributesValues] ([AttrValueID]),
    CONSTRAINT [FK_AttributesSet_WorkOrder] FOREIGN KEY ([WorkOrderID]) REFERENCES [dbo].[WorkOrder] ([WorkOrderID]) ON DELETE CASCADE,
    CONSTRAINT [IX_AttributesSet] UNIQUE NONCLUSTERED ([WorkOrderID] ASC, [AttributeID] ASC, [AttrValueID] ASC)
);



