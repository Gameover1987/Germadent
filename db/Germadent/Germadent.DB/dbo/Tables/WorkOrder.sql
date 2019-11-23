CREATE TABLE [dbo].[WorkOrder] (
    [WorkOrderID]     INT            IDENTITY (1, 1) NOT NULL,
    [BrachID]         INT            NOT NULL,
    [Status]          TINYINT        CONSTRAINT [DF_WorkOrder_Status] DEFAULT ((0)) NOT NULL,
    [DocNumber]       NVARCHAR (10)  NOT NULL,
    [CustomerID]      INT            NOT NULL,
    [PatientID]       INT            NOT NULL,
    [Created]         DATETIME       NULL,
    [DateDelivery]    DATETIME       NULL,
    [WorkDescription] NVARCHAR (250) NULL,
    [WorkAccept]      BIT            CONSTRAINT [DF_WorkOrder_WorkAccept] DEFAULT ((0)) NOT NULL,
    [AdministratorID] INT            NULL,
    [LastUpdated]     DATETIME       NULL,
    CONSTRAINT [PK_WorkOrder] PRIMARY KEY CLUSTERED ([WorkOrderID] ASC),
    CONSTRAINT [FK_WorkOrder_Branches] FOREIGN KEY ([BrachID]) REFERENCES [dbo].[Branches] ([BranchID]) ON UPDATE CASCADE,
    CONSTRAINT [FK_WorkOrder_Customers] FOREIGN KEY ([CustomerID]) REFERENCES [dbo].[Customers] ([CustomerID]) ON UPDATE CASCADE,
    CONSTRAINT [FK_WorkOrder_Employee] FOREIGN KEY ([AdministratorID]) REFERENCES [dbo].[Employee] ([EmployeeID]) ON UPDATE CASCADE,
    CONSTRAINT [FK_WorkOrder_Patients] FOREIGN KEY ([PatientID]) REFERENCES [dbo].[Patients] ([PatientID])
);




GO
CREATE NONCLUSTERED INDEX [IX-CustomerID]
    ON [dbo].[WorkOrder]([CustomerID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX-PatientID]
    ON [dbo].[WorkOrder]([PatientID] ASC);

