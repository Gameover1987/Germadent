CREATE TABLE [dbo].[Customers] (
    [CustomerID]          INT            IDENTITY (1, 1) NOT NULL,
    [CustomerName]        NVARCHAR (70)  NOT NULL,
    [CustomerPhone]       NVARCHAR (250) NULL,
    [CustomerEmail]       NVARCHAR (250) NULL,
    [CustomerWebSite]     NVARCHAR (250) NULL,
    [CustomerDescription] NVARCHAR (250) NULL,
    CONSTRAINT [PK_Customers] PRIMARY KEY CLUSTERED ([CustomerID] ASC)
);












GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Customers]
    ON [dbo].[Customers]([CustomerName] ASC);



