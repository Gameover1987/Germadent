CREATE TABLE [dbo].[LinksFileStreams] (
    [WorkOrderID] INT              NOT NULL,
    [stream_id]   UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [FK_FileStreams_StlAndPhotos] FOREIGN KEY ([stream_id]) REFERENCES [dbo].[StlAndPhotos] ([stream_id]),
    CONSTRAINT [FK_FileStreams_WorkOrder] FOREIGN KEY ([WorkOrderID]) REFERENCES [dbo].[WorkOrder] ([WorkOrderID]) ON DELETE CASCADE
);



