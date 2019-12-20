CREATE TABLE [dbo].[AdditionalEquipment] (
    [WorkOrderMCID] INT           NOT NULL,
    [EquipmentID]   INT           NOT NULL,
    [EquipmentName] NVARCHAR (40) NULL,
    [Quantity]      TINYINT       NOT NULL,
    CONSTRAINT [FK_AdditionalEquipment_WorkOrderMC] FOREIGN KEY ([WorkOrderMCID]) REFERENCES [dbo].[WorkOrderMC] ([WorkOrderMCID]),
    CONSTRAINT [IX_AdditionalEquipment] UNIQUE CLUSTERED ([WorkOrderMCID] ASC, [EquipmentID] ASC)
);





