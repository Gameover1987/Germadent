CREATE TABLE [dbo].[QualitAttributesSet] (
    [WorkOrderID] INT NOT NULL,
    [AttributeID] INT NOT NULL,
    [QitemID]     INT NOT NULL,
    CONSTRAINT [FK_QualitAttributesSet_QualitativeAttributes] FOREIGN KEY ([AttributeID]) REFERENCES [dbo].[QualitativeAttributes] ([AttributeID]),
    CONSTRAINT [FK_QualitAttributesSet_QualitativeItems] FOREIGN KEY ([QitemID]) REFERENCES [dbo].[QualitativeItems] ([QitemID]),
    CONSTRAINT [FK_QualitAttributesSet_WorkOrder] FOREIGN KEY ([WorkOrderID]) REFERENCES [dbo].[WorkOrder] ([WorkOrderID]) ON DELETE CASCADE
);



