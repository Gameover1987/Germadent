CREATE TABLE [dbo].[ResponsiblePersons] (
    [ResponsiblePersonID] INT           IDENTITY (1, 1) NOT NULL,
    [CustomerID]          INT           NOT NULL,
    [ResponsiblePerson]   NVARCHAR (50) NOT NULL,
    [RP_Position]         NVARCHAR (30) NOT NULL,
    [RP_Phone]            VARCHAR (15)  NULL,
    CONSTRAINT [PK_ResponsiblePersons] PRIMARY KEY CLUSTERED ([ResponsiblePersonID] ASC),
    CONSTRAINT [FK_ResponsiblePersons_Customers] FOREIGN KEY ([CustomerID]) REFERENCES [dbo].[Customers] ([CustomerID])
);



