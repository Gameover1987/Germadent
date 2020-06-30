CREATE TABLE [dbo].[ServicesCost] (
    [WorkOrderID] INT   NOT NULL,
    [ServiceID]   INT   NOT NULL,
    [Price]       MONEY CONSTRAINT [DF_ServicesCost_Price] DEFAULT ((0)) NOT NULL,
    [Quantity]    INT   CONSTRAINT [DF_ServicesCost_Quantity] DEFAULT ((0)) NOT NULL,
    [Cost]        AS    ([Price]*[Quantity])
);



