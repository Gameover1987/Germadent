CREATE TABLE [dbo].[ToothCard] (
    [WorkOrderID]   INT     NOT NULL,
    [ToothNumber]   TINYINT NOT NULL,
    [MaterialID]    INT     NOT NULL,
    [ProstheticsID] INT     NULL,
    [Bridge]        TINYINT NULL,
    CONSTRAINT [FK_ToothCard_Materials] FOREIGN KEY ([MaterialID]) REFERENCES [dbo].[Materials] ([MaterialID]),
    CONSTRAINT [FK_ToothCard_TypeOfProsthetics] FOREIGN KEY ([ProstheticsID]) REFERENCES [dbo].[TypesOfProsthetics] ([ProstheticsID]),
    CONSTRAINT [FK_ToothCard_WorkOrder] FOREIGN KEY ([WorkOrderID]) REFERENCES [dbo].[WorkOrder] ([WorkOrderID]),
    CONSTRAINT [IX_ToothCard] UNIQUE CLUSTERED ([WorkOrderID] ASC, [ToothNumber] ASC)
);








GO


