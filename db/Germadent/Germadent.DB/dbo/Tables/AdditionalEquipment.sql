CREATE TABLE [dbo].[AdditionalEquipment] (
    [WorkOrderID] INT NOT NULL,
    [EquipmentID] INT NOT NULL,
    [Quantity]    INT NOT NULL,
    CONSTRAINT [FK_AdditionalEquipment_Equipments] FOREIGN KEY ([EquipmentID]) REFERENCES [dbo].[Equipments] ([EquipmentID]),
    CONSTRAINT [FK_AdditionalEquipment_WorkOrder] FOREIGN KEY ([WorkOrderID]) REFERENCES [dbo].[WorkOrder] ([WorkOrderID]),
    CONSTRAINT [IX_AdditionalEquipment] UNIQUE CLUSTERED ([WorkOrderID] ASC, [EquipmentID] ASC)
);













