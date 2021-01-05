CREATE TABLE [dbo].[AttributesSet] (
    [WorkOrderID]      INT     NOT NULL,
    [ToothNumber]      TINYINT NULL,
    [AttributeID]      INT     NOT NULL,
    [AttributeValueID] INT     NOT NULL,
    CONSTRAINT [FK_AttributesSet_Attributes] FOREIGN KEY ([AttributeID]) REFERENCES [dbo].[Attributes] ([AttributeID]),
    CONSTRAINT [FK_AttributesSet_AttrValues] FOREIGN KEY ([AttributeValueID]) REFERENCES [dbo].[AttrValues] ([AttributeValueID]),
    CONSTRAINT [FK_AttributesSet_WorkOrder] FOREIGN KEY ([WorkOrderID]) REFERENCES [dbo].[WorkOrder] ([WorkOrderID]) ON DELETE CASCADE,
    CONSTRAINT [IX_AttributesSet] UNIQUE NONCLUSTERED ([WorkOrderID] ASC, [AttributeID] ASC, [AttributeValueID] ASC)
);












GO
CREATE NONCLUSTERED INDEX [IX_AttributesSet_WorkOrderID]
    ON [dbo].[AttributesSet]([WorkOrderID] ASC);

