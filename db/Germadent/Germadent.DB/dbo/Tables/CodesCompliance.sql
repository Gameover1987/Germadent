CREATE TABLE [dbo].[CodesCompliance] (
    [ComplianceID] INT           IDENTITY (1, 1) NOT NULL,
    [CodeDL]       NVARCHAR (20) NOT NULL,
    [CodeMC]       NVARCHAR (20) NOT NULL,
    CONSTRAINT [UQ_CodesCompliance] UNIQUE NONCLUSTERED ([CodeDL] ASC, [CodeMC] ASC)
);

