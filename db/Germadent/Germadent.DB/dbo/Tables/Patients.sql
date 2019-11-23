CREATE TABLE [dbo].[Patients] (
    [PatientID]  INT           IDENTITY (1, 1) NOT NULL,
    [FamilyName] NVARCHAR (30) NOT NULL,
    [Name]       NVARCHAR (30) NULL,
    [Patronymic] NVARCHAR (30) NULL,
    [Gender]     BIT           NULL,
    [Birthday]   DATE          NULL,
    CONSTRAINT [PK_Patients] PRIMARY KEY CLUSTERED ([PatientID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Дата рождения', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Patients', @level2type = N'COLUMN', @level2name = N'Birthday';

