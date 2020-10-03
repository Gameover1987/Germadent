CREATE TABLE [dbo].[TypesOfProsthetics] (
    [ProstheticsID]   INT            IDENTITY (1, 1) NOT NULL,
    [ProstheticsName] NVARCHAR (150) NOT NULL,
    CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED ([ProstheticsID] ASC)
);

