CREATE TABLE [dbo].[LinksFileStreams] (
    [UserID]    INT              NOT NULL,
    [stream_id] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [FK_FileStreams_StlAndPhotos] FOREIGN KEY ([stream_id]) REFERENCES [dbo].[StlAndPhotos] ([stream_id]),
    CONSTRAINT [FK_LinksFileStreams_Users] FOREIGN KEY ([UserID]) REFERENCES [dbo].[Users] ([UserID])
);





