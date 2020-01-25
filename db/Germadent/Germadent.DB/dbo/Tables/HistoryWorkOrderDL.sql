CREATE TABLE [dbo].[HistoryWorkOrderDL] (
    [HistoryDateTime]  DATETIME       NULL,
    [HistoryEditor]    NVARCHAR (30)  NULL,
    [HistoryOperate]   VARCHAR (20)   NULL,
    [WorkOrderDLID]    INT            NULL,
    [TransparenceID]   INT            NULL,
    [DoctorFullName]   NVARCHAR (150) NULL,
    [PatientFullName]  NVARCHAR (150) NULL,
    [PatientAge]       TINYINT        NULL,
    [FittingDate]      DATE           NULL,
    [DateOfCompletion] DATE           NULL,
    [ColorAndFeatures] NVARCHAR (100) NULL
);

