CREATE TABLE [dbo].[Customers] (
    [CustomerID]   INT           IDENTITY (1, 1) NOT NULL,
    [CustomerName] NVARCHAR (70) NOT NULL,
    CONSTRAINT [PK_Customers] PRIMARY KEY CLUSTERED ([CustomerID] ASC)
);








GO
CREATE NONCLUSTERED INDEX [IX_Customers]
    ON [dbo].[Customers]([CustomerName] ASC);

