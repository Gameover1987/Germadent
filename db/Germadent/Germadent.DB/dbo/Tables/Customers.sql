CREATE TABLE [dbo].[Customers] (
    [CustomerID]        INT            IDENTITY (1, 1) NOT NULL,
    [CustomerName]      NVARCHAR (70)  NOT NULL,
    [ResponsiblePerson] NVARCHAR (100) NULL,
    [Position]          NVARCHAR (30)  NULL,
    [Phone]             NCHAR (20)     NULL,
    CONSTRAINT [PK_Customers] PRIMARY KEY CLUSTERED ([CustomerID] ASC)
);

