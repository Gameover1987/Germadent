CREATE TABLE [dbo].[ToothCard] (
    [WorkOrderID]   INT     NOT NULL,
    [ToothNumber]   TINYINT NOT NULL,
    [ConditionID]   INT     NOT NULL,
    [ProstheticsID] INT     NULL,
    [MaterialID]    INT     NULL,
    [FlagBridge]    BIT     NULL,
    CONSTRAINT [FK_ToothCard_ConditionsOfProsthetics] FOREIGN KEY ([ConditionID]) REFERENCES [dbo].[ConditionsOfProsthetics] ([ConditionID]),
    CONSTRAINT [FK_ToothCard_Materials] FOREIGN KEY ([MaterialID]) REFERENCES [dbo].[Materials] ([MaterialID]),
    CONSTRAINT [FK_ToothCard_TypesOfProsthetics] FOREIGN KEY ([ProstheticsID]) REFERENCES [dbo].[TypesOfProsthetics] ([ProstheticsID]),
    CONSTRAINT [FK_ToothCard_WorkOrder] FOREIGN KEY ([WorkOrderID]) REFERENCES [dbo].[WorkOrder] ([WorkOrderID]) ON DELETE CASCADE,
    CONSTRAINT [IX_ToothCard] UNIQUE CLUSTERED ([WorkOrderID] ASC, [ToothNumber] ASC)
);
























GO


