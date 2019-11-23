CREATE TABLE [dbo].[Users] (
    [UserID]   INT           IDENTITY (1, 1) NOT NULL,
    [Login]    NVARCHAR (30) NOT NULL,
    [Password] NVARCHAR (10) NULL,
    [FlagLock] BIT           CONSTRAINT [DF_Users_FlagLock] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([UserID] ASC)
);



