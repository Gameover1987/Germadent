CREATE TABLE [dbo].[HistoryWorkOrder] (
    [HistoryDateTime]     DATETIME       NULL,
    [HistoryEditor]       NVARCHAR (50)  NULL,
    [HistoryOperate]      VARCHAR (20)   NULL,
    [WorkOrderID]         INT            NULL,
    [BranchTypeID]        INT            NULL,
    [Status]              INT            NULL,
    [DocNumber]           NVARCHAR (10)  NULL,
    [CustomerID]          INT            NULL,
    [CustomerName]        NVARCHAR (100) NULL,
    [ResponsiblePersonID] INT            NULL,
    [PatientID]           INT            NULL,
    [Created]             DATETIME       NULL,
    [DateDelivery]        DATETIME       NULL,
    [ProstheticArticul]   NVARCHAR (50)  NULL,
    [WorkDescription]     NVARCHAR (250) NULL,
    [FlagWorkAccept]      BIT            NULL,
    [OfficeAdminID]       INT            NULL,
    [OfficeAdminName]     NVARCHAR (50)  NULL,
    [Closed]              DATETIME       NULL
);



