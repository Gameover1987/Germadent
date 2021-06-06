CREATE TABLE [dbo].[HistoryWorkOrder] (
    [HistoryDateTime]     DATETIME       NULL,
    [HistoryEditor]       NVARCHAR (50)  NULL,
    [HistoryOperate]      VARCHAR (20)   NULL,
    [WorkOrderID]         INT            NULL,
    [BranchTypeID]        INT            NULL,
    [DocNumber]           NVARCHAR (10)  NULL,
    [CustomerID]          INT            NULL,
    [ResponsiblePersonID] INT            NULL,
    [PatientFullName]     NVARCHAR (150) NULL,
    [PatientGender]       BIT            NULL,
    [PatientAge]          TINYINT        NULL,
    [FittingDate]         DATETIME       NULL,
    [DateOfCompletion]    DATETIME       NULL,
    [DateComment]         NVARCHAR (50)  NULL,
    [ProstheticArticul]   NVARCHAR (50)  NULL,
    [WorkDescription]     NVARCHAR (250) NULL,
    [UrgencyRatio]        FLOAT (53)     NULL,
    [FlagWorkAccept]      BIT            NULL,
    [FlagStl]             BIT            NULL,
    [FlagCashless]        BIT            NULL
);



























