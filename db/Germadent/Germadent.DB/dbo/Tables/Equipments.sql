CREATE TABLE [dbo].[Equipments] (
    [EquipmentID]   INT           IDENTITY (1, 1) NOT NULL,
    [EquipmentName] NVARCHAR (40) NULL,
    CONSTRAINT [PK_Equipments] PRIMARY KEY CLUSTERED ([EquipmentID] ASC)
);

