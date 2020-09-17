CREATE TABLE [dbo].[Product] (
    [ProductID]   INT            IDENTITY (1, 1) NOT NULL,
    [ProductName] NVARCHAR (150) NOT NULL,
    CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED ([ProductID] ASC)
);

