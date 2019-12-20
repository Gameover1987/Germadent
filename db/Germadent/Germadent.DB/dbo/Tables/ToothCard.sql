CREATE TABLE [dbo].[ToothCard] (
    [WorkOrderID]     INT           NOT NULL,
    [ToothNumber]     TINYINT       NOT NULL,
    [MaterialID]      INT           NULL,
    [MaterialName]    NVARCHAR (30) NULL,
    [ProstheticsID]   INT           NULL,
    [ProstheticsName] NVARCHAR (50) NULL,
    [Bridge]          TINYINT       NULL,
    CONSTRAINT [FK_ToothCard_WorkOrder] FOREIGN KEY ([WorkOrderID]) REFERENCES [dbo].[WorkOrder] ([WorkOrderID]),
    CONSTRAINT [IX_ToothCard] UNIQUE CLUSTERED ([WorkOrderID] ASC, [ToothNumber] ASC)
);










GO


