CREATE TABLE [dbo].[HistoryWorkOrderDL] (
    [HistoryDateTime]  DATETIME       NULL,
    [HistoryEditor]    NVARCHAR (30)  NULL,
    [HistoryOperate]   VARCHAR (20)   NULL,
    [WorkOrderDLID]    INT            NULL,
    [DoctorFullName]   NVARCHAR (150) NULL,
    [TransparenceID]   INT            NULL,
    [PatientAge]       TINYINT        NULL,
    [FittingDate]      DATE           NULL,
    [DateOfCompletion] DATE           NULL,
    [ColorAndFeatures] NVARCHAR (100) NULL
);



