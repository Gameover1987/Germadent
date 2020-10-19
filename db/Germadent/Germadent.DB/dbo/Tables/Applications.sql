CREATE TABLE [dbo].[Applications] (
    [ApplicationID]          INT            IDENTITY (1, 1) NOT NULL,
    [ApplicationName]        NVARCHAR (MAX) NOT NULL,
    [ApplicationDescription] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Applications] PRIMARY KEY CLUSTERED ([ApplicationID] ASC)
);

