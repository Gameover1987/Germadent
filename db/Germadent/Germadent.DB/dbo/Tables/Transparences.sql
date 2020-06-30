CREATE TABLE [dbo].[Transparences] (
    [TransparencyID]   INT           IDENTITY (1, 1) NOT NULL,
    [TransparencyName] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_Transparences] PRIMARY KEY CLUSTERED ([TransparencyID] ASC)
);



