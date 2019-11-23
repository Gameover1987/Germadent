CREATE TABLE [dbo].[WorkOrderMC] (
    [WorkOrderMCID]                INT           NOT NULL,
    [AdditionalInfo]               NVARCHAR (70) NULL,
    [Carcass]                      INT           NULL,
    [ImplantSystem]                NVARCHAR (70) NULL,
    [IndividualAbutmentProcessing] NCHAR (70)    NULL,
    CONSTRAINT [PK_WorkOrderMC] PRIMARY KEY CLUSTERED ([WorkOrderMCID] ASC),
    CONSTRAINT [FK_WorkOrderMC_WorkOrder] FOREIGN KEY ([WorkOrderMCID]) REFERENCES [dbo].[WorkOrder] ([WorkOrderID])
);

