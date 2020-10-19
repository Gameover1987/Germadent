CREATE TABLE [dbo].[Rights] (
    [RightID]       INT            IDENTITY (1, 1) NOT NULL,
    [ApplicationID] INT            NOT NULL,
    [RightName]     NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_Rights] PRIMARY KEY CLUSTERED ([RightID] ASC),
    CONSTRAINT [FK_Rights_Applications] FOREIGN KEY ([ApplicationID]) REFERENCES [dbo].[Applications] ([ApplicationID])
);



