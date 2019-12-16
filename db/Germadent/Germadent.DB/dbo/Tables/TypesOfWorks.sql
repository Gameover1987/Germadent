CREATE TABLE [dbo].[TypesOfWorks] (
    [WorkID]   INT           IDENTITY (1, 1) NOT NULL,
    [WorkName] NVARCHAR (30) NULL,
    [Position] NVARCHAR (30) NULL,
    CONSTRAINT [PK_TypesOfWork] PRIMARY KEY CLUSTERED ([WorkID] ASC)
);



