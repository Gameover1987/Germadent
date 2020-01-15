CREATE TABLE [dbo].[HistoryWorkOrderMC] (
    [HistoryDateTime]              DATETIME       NULL,
    [HistoryEditor]                NVARCHAR (30)  NULL,
    [HistoryOperate]               VARCHAR (20)   NULL,
    [WorkOrderMCID]                INT            NULL,
    [TechnicFullName]              NVARCHAR (150) NULL,
    [TechnicPhone]                 VARCHAR (20)   NULL,
    [AdditionalInfo]               NVARCHAR (70)  NULL,
    [CarcassColor]                 NVARCHAR (30)  NULL,
    [ImplantSystem]                NVARCHAR (70)  NULL,
    [IndividualAbutmentProcessing] NVARCHAR (80)  NULL,
    [Understaff]                   NVARCHAR (100) NULL
);

