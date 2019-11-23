CREATE TABLE [dbo].[AdditionalEquipment] (
    [WorkOrderMCID] INT NOT NULL,
    [EquipmentID]   INT NOT NULL,
    [Quantity]      INT NOT NULL,
    CONSTRAINT [FK_AdditionalEquipment_Equipments] FOREIGN KEY ([EquipmentID]) REFERENCES [dbo].[Equipments] ([EquipmentID]),
    CONSTRAINT [FK_AdditionalEquipment_WorkOrderMC] FOREIGN KEY ([WorkOrderMCID]) REFERENCES [dbo].[WorkOrderMC] ([WorkOrderMCID])
);

