CREATE TABLE [dbo].[WorkOrderDL] (
    [WorkOrderDLID]    INT            IDENTITY (1, 1) NOT NULL,
    [TransparenceID]   INT            NULL,
    [TypeOfWork]       NCHAR (255)    NULL,
    [FittingDate]      DATE           NULL,
    [DateOfCompletion] DATE           NULL,
    [ColorAndFeatures] NVARCHAR (100) NULL,
    CONSTRAINT [PK_WorkOrderDL] PRIMARY KEY CLUSTERED ([WorkOrderDLID] ASC),
    CONSTRAINT [FK_WorkOrderDL_Transparences] FOREIGN KEY ([TransparenceID]) REFERENCES [dbo].[Transparences] ([TransparenceID]),
    CONSTRAINT [FK_WorkOrderDL_WorkOrder] FOREIGN KEY ([WorkOrderDLID]) REFERENCES [dbo].[WorkOrder] ([WorkOrderID])
);

