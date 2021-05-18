﻿CREATE TABLE [dbo].[WorkList] (
    [WorkID]                INT            IDENTITY (1, 1) NOT NULL,
    [WorkOrderID]           INT            NOT NULL,
    [ProductID]             INT            NULL,
    [TechnologyOperationID] INT            NOT NULL,
    [EmployeeID]            INT            NOT NULL,
    [Rate]                  MONEY          NOT NULL,
    [Quantity]              INT            NOT NULL,
    [OperationCost]         MONEY          NOT NULL,
    [WorkStarted]           DATETIME       NULL,
    [WorkCompleted]         DATETIME       NULL,
    [Remark]                NVARCHAR (250) NULL,
    [LastEditor]            INT            NULL,
    CONSTRAINT [PK_WorkList] PRIMARY KEY CLUSTERED ([WorkID] ASC),
    CONSTRAINT [FK_WorkList_Products] FOREIGN KEY ([ProductID]) REFERENCES [dbo].[Products] ([ProductID]),
    CONSTRAINT [FK_WorkList_TechnologyOperations] FOREIGN KEY ([TechnologyOperationID]) REFERENCES [dbo].[TechnologyOperations] ([TechnologyOperationID]),
    CONSTRAINT [FK_WorkList_Users] FOREIGN KEY ([EmployeeID]) REFERENCES [dbo].[Users] ([UserID]),
    CONSTRAINT [FK_WorkList_Users1] FOREIGN KEY ([LastEditor]) REFERENCES [dbo].[Users] ([UserID]),
    CONSTRAINT [FK_WorkList_WorkOrder] FOREIGN KEY ([WorkOrderID]) REFERENCES [dbo].[WorkOrder] ([WorkOrderID]) ON DELETE CASCADE
);












GO
CREATE NONCLUSTERED INDEX [IX_WorkList_WorkOrderID]
    ON [dbo].[WorkList]([WorkOrderID] ASC);

