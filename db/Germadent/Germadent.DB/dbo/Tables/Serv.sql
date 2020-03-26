CREATE TABLE [dbo].[Serv] (
    [ServiceID]      INT            IDENTITY (1, 1) NOT NULL,
    [ServiceGroupID] INT            NOT NULL,
    [SeviceName]     NVARCHAR (MAX) NOT NULL,
    [FlagUnused]     BIT            NULL,
    CONSTRAINT [PK_Services] PRIMARY KEY CLUSTERED ([ServiceID] ASC),
    CONSTRAINT [FK_Services_ServiceGroups] FOREIGN KEY ([ServiceGroupID]) REFERENCES [dbo].[ServGroups] ([ServiceGroupID])
);


GO
CREATE NONCLUSTERED INDEX [IX_Services]
    ON [dbo].[Serv]([ServiceGroupID] ASC);

