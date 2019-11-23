CREATE TABLE [dbo].[ToothCard] (
    [ToothNumber]   TINYINT NOT NULL,
    [WorkOrderID]   INT     NOT NULL,
    [MaterialID]    INT     NOT NULL,
    [ProstheticsID] INT     NULL,
    CONSTRAINT [PK_ToothCard] PRIMARY KEY CLUSTERED ([ToothNumber] ASC),
    CONSTRAINT [FK_ToothCard_Materials] FOREIGN KEY ([MaterialID]) REFERENCES [dbo].[Materials] ([MaterialID]),
    CONSTRAINT [FK_ToothCard_TypeOfProsthetics] FOREIGN KEY ([ProstheticsID]) REFERENCES [dbo].[TypesOfProsthetics] ([ProstheticsID]),
    CONSTRAINT [FK_ToothCard_WorkOrder] FOREIGN KEY ([WorkOrderID]) REFERENCES [dbo].[WorkOrder] ([WorkOrderID])
);

