CREATE TABLE [dbo].[ResponsiblePersons] (
    [ResponsiblePersonID] INT            IDENTITY (1, 1) NOT NULL,
    [ResponsiblePerson]   NVARCHAR (150) NOT NULL,
    [RP_Position]         NVARCHAR (30)  NOT NULL,
    [RP_Phone]            NVARCHAR (150) NULL,
    [RP_Email]            NVARCHAR (150) NULL,
    [RP_Description]      NVARCHAR (250) NULL,
    [EnableNotify]        BIT            NULL,
    CONSTRAINT [PK_ResponsiblePersons] PRIMARY KEY CLUSTERED ([ResponsiblePersonID] ASC),
    CONSTRAINT [IX_ResponsiblePersons] UNIQUE NONCLUSTERED ([ResponsiblePerson] ASC)
);












GO


