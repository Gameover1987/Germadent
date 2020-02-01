CREATE TABLE [dbo].[AdditionalEquipment] (
    [WorkOrderID] INT     NOT NULL,
    [EquipmentID] INT     NOT NULL,
    [Quantity]    TINYINT NOT NULL,
    CONSTRAINT [FK_AdditionalEquipment_WorkOrder] FOREIGN KEY ([WorkOrderID]) REFERENCES [dbo].[WorkOrder] ([WorkOrderID]),
    CONSTRAINT [IX_AdditionalEquipment] UNIQUE CLUSTERED ([WorkOrderID] ASC, [EquipmentID] ASC)
);









