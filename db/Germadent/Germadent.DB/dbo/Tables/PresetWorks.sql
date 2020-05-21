CREATE TABLE [dbo].[PresetWorks] (
    [ServiceID] INT NOT NULL,
    [WorkID]    INT NOT NULL,
    CONSTRAINT [FK_PresetWorks_Serv] FOREIGN KEY ([ServiceID]) REFERENCES [dbo].[Serv] ([ServiceID]),
    CONSTRAINT [FK_PresetWorks_Works] FOREIGN KEY ([WorkID]) REFERENCES [dbo].[Works] ([WorkID])
);

