CREATE TABLE [dbo].[Works] (
    [WorkID]   INT           IDENTITY (1, 1) NOT NULL,
    [WorkName] NVARCHAR (50) NOT NULL,
    [Position] NVARCHAR (30) NOT NULL,
    CONSTRAINT [PK_Works] PRIMARY KEY CLUSTERED ([WorkID] ASC)
);

