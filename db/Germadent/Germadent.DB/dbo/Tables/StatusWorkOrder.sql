CREATE TABLE [dbo].[StatusWorkOrder] (
    [WorkOrderID]          INT            NOT NULL,
    [Status]               INT            NOT NULL,
    [StatusChangeDateTime] DATETIME       NOT NULL,
    [UserID]               INT            NULL,
    [Remark]               NVARCHAR (250) NULL,
    CONSTRAINT [FK_StatusWorkOrder_StatusList] FOREIGN KEY ([Status]) REFERENCES [dbo].[StatusList] ([Status]),
    CONSTRAINT [FK_StatusWorkOrder_Users] FOREIGN KEY ([UserID]) REFERENCES [dbo].[Users] ([UserID]),
    CONSTRAINT [FK_StatusWorkOrder_WorkOrder] FOREIGN KEY ([WorkOrderID]) REFERENCES [dbo].[WorkOrder] ([WorkOrderID]) ON DELETE CASCADE
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_StatusWorkOrder]
    ON [dbo].[StatusWorkOrder]([WorkOrderID] ASC, [Status] ASC);

