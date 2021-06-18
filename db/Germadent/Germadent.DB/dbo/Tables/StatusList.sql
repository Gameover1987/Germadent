CREATE TABLE [dbo].[StatusList] (
    [WorkOrderID]          INT            NOT NULL,
    [Status]               INT            NOT NULL,
    [StatusChangeDateTime] DATETIME       NOT NULL,
    [UserID]               INT            NULL,
    [Remark]               NVARCHAR (250) NULL,
    CONSTRAINT [FK_StatusList_StatusEnumeration] FOREIGN KEY ([Status]) REFERENCES [dbo].[StatusEnumeration] ([Status]),
    CONSTRAINT [FK_StatusList_Users] FOREIGN KEY ([UserID]) REFERENCES [dbo].[Users] ([UserID]),
    CONSTRAINT [FK_StatusList_WorkOrder] FOREIGN KEY ([WorkOrderID]) REFERENCES [dbo].[WorkOrder] ([WorkOrderID]) ON DELETE CASCADE
);






GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_StatusWorkOrder]
    ON [dbo].[StatusList]([WorkOrderID] ASC, [Status] ASC);

