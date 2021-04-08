CREATE TABLE [dbo].[StatusList] (
    [Status]     INT           NOT NULL,
    [StatusName] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_StatusList] PRIMARY KEY CLUSTERED ([Status] ASC)
);



