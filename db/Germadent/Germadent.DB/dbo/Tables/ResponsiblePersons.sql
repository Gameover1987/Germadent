CREATE TABLE [dbo].[ResponsiblePersons] (
    [ResponsiblePersonID] INT            IDENTITY (1, 1) NOT NULL,
    [CustomerID]          INT            NOT NULL,
    [ResponsiblePerson]   NVARCHAR (150) NOT NULL,
    [RP_Position]         NVARCHAR (30)  NOT NULL,
    [RP_Phone]            NVARCHAR (150) NULL,
    [RP_Email]            NVARCHAR (150) NULL,
    [RP_Description]      NVARCHAR (250) NULL,
    CONSTRAINT [PK_ResponsiblePersons] PRIMARY KEY CLUSTERED ([ResponsiblePersonID] ASC),
    CONSTRAINT [FK_ResponsiblePersons_Customers] FOREIGN KEY ([CustomerID]) REFERENCES [dbo].[Customers] ([CustomerID])
);








GO
CREATE NONCLUSTERED INDEX [IX_ResponsiblePersons]
    ON [dbo].[ResponsiblePersons]([CustomerID] ASC);

