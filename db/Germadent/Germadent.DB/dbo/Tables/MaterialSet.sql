CREATE TABLE [dbo].[MaterialSet] (
    [WorkOrderID]     INT           NOT NULL,
    [WorkID]          INT           NOT NULL,
    [MaterialArticul] NVARCHAR (20) NOT NULL,
    [Quantity]        FLOAT (53)    NULL,
    CONSTRAINT [FK_MaterialSet_Works] FOREIGN KEY ([WorkID]) REFERENCES [dbo].[Works] ([WorkID])
);

