CREATE TABLE [dbo].[WorkOrder] (
    [WorkOrderID]         INT            IDENTITY (1, 1) NOT NULL,
    [BranchTypeID]        INT            NOT NULL,
    [Status]              INT            CONSTRAINT [DF_WorkOrder_Status] DEFAULT ((0)) NOT NULL,
    [DocNumber]           NVARCHAR (10)  NOT NULL,
    [CustomerID]          INT            NULL,
    [CustomerName]        NVARCHAR (100) NULL,
    [ResponsiblePersonID] INT            NULL,
    [PatientID]           INT            NULL,
    [Created]             DATETIME       NULL,
    [DateDelivery]        DATETIME       NULL,
    [WorkDescription]     NVARCHAR (250) NULL,
    [FlagWorkAccept]      BIT            CONSTRAINT [DF_WorkOrder_WorkAccept] DEFAULT ((0)) NOT NULL,
    [OfficeAdminID]       INT            NULL,
    [OfficeAdminName]     NVARCHAR (50)  NULL,
    [Closed]              DATETIME       NULL,
    CONSTRAINT [PK_WorkOrder] PRIMARY KEY CLUSTERED ([WorkOrderID] ASC),
    CONSTRAINT [FK_WorkOrder_Branches] FOREIGN KEY ([BranchTypeID]) REFERENCES [dbo].[BranchTypes] ([BranchTypeID]) ON UPDATE CASCADE
);












GO
CREATE NONCLUSTERED INDEX [IX-CustomerID]
    ON [dbo].[WorkOrder]([CustomerID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX-PatientID]
    ON [dbo].[WorkOrder]([PatientID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ResponsiblePerson]
    ON [dbo].[WorkOrder]([ResponsiblePersonID] ASC);

