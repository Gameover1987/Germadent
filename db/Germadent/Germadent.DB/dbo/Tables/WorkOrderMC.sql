CREATE TABLE [dbo].[WorkOrderMC] (
    [WorkOrderMCID]                INT            NOT NULL,
    [TechnicFullName]              NVARCHAR (150) NULL,
    [TechnicPhone]                 VARCHAR (20)   NULL,
    [AdditionalInfo]               NVARCHAR (70)  NULL,
    [CarcassColor]                 NVARCHAR (30)  NULL,
    [ImplantSystem]                NVARCHAR (70)  NULL,
    [IndividualAbutmentProcessing] NVARCHAR (80)  NULL,
    [Understaff]                   NVARCHAR (100) NULL,
    CONSTRAINT [PK_WorkOrderMC] PRIMARY KEY CLUSTERED ([WorkOrderMCID] ASC),
    CONSTRAINT [FK_WorkOrderMC_WorkOrder] FOREIGN KEY ([WorkOrderMCID]) REFERENCES [dbo].[WorkOrder] ([WorkOrderID])
);





