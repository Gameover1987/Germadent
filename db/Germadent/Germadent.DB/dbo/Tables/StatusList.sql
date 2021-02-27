CREATE TABLE [dbo].[StatusList] (
    [WorkOrderID]          INT            NOT NULL,
    [Status]               INT            NOT NULL,
    [StatusChangeDateTime] DATETIME       NOT NULL,
    [UserID]               INT            NOT NULL,
    [Remark]               NVARCHAR (250) NULL,
    CONSTRAINT [PK_StatusList] PRIMARY KEY CLUSTERED ([WorkOrderID] ASC),
    CONSTRAINT [FK_StatusList_Users] FOREIGN KEY ([UserID]) REFERENCES [dbo].[Users] ([UserID]),
    CONSTRAINT [FK_StatusList_WorkOrder] FOREIGN KEY ([WorkOrderID]) REFERENCES [dbo].[WorkOrder] ([WorkOrderID])
);

