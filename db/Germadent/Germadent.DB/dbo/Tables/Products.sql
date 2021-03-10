CREATE TABLE [dbo].[Products] (
    [ProductID]         INT            IDENTITY (1, 1) NOT NULL,
    [ProductName]       NVARCHAR (150) NOT NULL,
    [IsObsoleteProduct] BIT            NULL,
    CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED ([ProductID] ASC)
);



