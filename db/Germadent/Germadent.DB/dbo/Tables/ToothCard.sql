CREATE TABLE [dbo].[ToothCard] (
    [WorkOrderID]        INT     NOT NULL,
    [ToothNumber]        TINYINT NOT NULL,
    [ServiceID]          INT     NULL,
    [Price]              MONEY   NULL,
    [ContructionColorID] INT     NULL,
    [ConditionID]        INT     NOT NULL,
    [ProstheticsID]      INT     NOT NULL,
    [TrasparencyID]      INT     NULL,
    [FlagBridge]         BIT     NULL,
    CONSTRAINT [FK_ToothCard_ConditionsOfProsthetics] FOREIGN KEY ([ConditionID]) REFERENCES [dbo].[ConditionsOfProsthetics] ([ConditionID]),
    CONSTRAINT [FK_ToothCard_ConstructionColors] FOREIGN KEY ([ContructionColorID]) REFERENCES [dbo].[ConstructionColors] ([ConstructionColorID]),
    CONSTRAINT [FK_ToothCard_Services] FOREIGN KEY ([ServiceID]) REFERENCES [dbo].[Servicess] ([ServiceID]),
    CONSTRAINT [FK_ToothCard_Transparences] FOREIGN KEY ([TrasparencyID]) REFERENCES [dbo].[Transparences] ([TransparencyID]),
    CONSTRAINT [FK_ToothCard_TypesOfProsthetics] FOREIGN KEY ([ProstheticsID]) REFERENCES [dbo].[TypesOfProsthetics] ([ProstheticsID]),
    CONSTRAINT [FK_ToothCard_WorkOrder] FOREIGN KEY ([WorkOrderID]) REFERENCES [dbo].[WorkOrder] ([WorkOrderID]) ON DELETE CASCADE,
    CONSTRAINT [IX_ToothCard] UNIQUE CLUSTERED ([WorkOrderID] ASC, [ToothNumber] ASC)
);
































GO


