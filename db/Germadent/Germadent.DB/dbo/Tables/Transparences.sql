CREATE TABLE [dbo].[Transparences] (
    [TransparenceID]   INT           IDENTITY (1, 1) NOT NULL,
    [TransparenceName] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_Transparences] PRIMARY KEY CLUSTERED ([TransparenceID] ASC)
);

