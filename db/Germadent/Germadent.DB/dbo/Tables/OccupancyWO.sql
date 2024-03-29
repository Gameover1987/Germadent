﻿CREATE TABLE [dbo].[OccupancyWO] (
    [WorkOrderID]       INT      NOT NULL,
    [OccupancyDateTime] DATETIME NOT NULL,
    [UserID]            INT      NOT NULL,
    CONSTRAINT [FK_OccupancyWO_Users] FOREIGN KEY ([UserID]) REFERENCES [dbo].[Users] ([UserID]),
    CONSTRAINT [FK_OccupancyWO_WorkOrder] FOREIGN KEY ([WorkOrderID]) REFERENCES [dbo].[WorkOrder] ([WorkOrderID]) ON DELETE CASCADE
);








GO
CREATE NONCLUSTERED INDEX [IX_OccupancyWO]
    ON [dbo].[OccupancyWO]([WorkOrderID] ASC, [OccupancyDateTime] ASC);

