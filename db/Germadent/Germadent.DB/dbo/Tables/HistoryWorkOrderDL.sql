CREATE TABLE [dbo].[HistoryWorkOrderDL] (
    [HistoryDateTime]  DATETIME       NULL,
    [HistoryEditor]    NVARCHAR (30)  NULL,
    [HistoryOperate]   VARCHAR (20)   NULL,
    [WorkOrderDLID]    INT            NULL,
    [TransparenceID]   INT            NULL,
    [PatientGender]    BIT            NULL,
    [PatientAge]       TINYINT        NULL,
    [FittingDate]      DATE           NULL,
    [DateOfCompletion] DATE           NULL,
    [ColorAndFeatures] NVARCHAR (100) NULL
);







