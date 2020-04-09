CREATE TABLE [dbo].[WorkOrder] (
    [WorkOrderID]         INT            IDENTITY (1, 1) NOT NULL,
    [BranchTypeID]        INT            NOT NULL,
    [Status]              INT            CONSTRAINT [DF_WorkOrder_Status] DEFAULT ((0)) NOT NULL,
    [DocNumber]           NVARCHAR (10)  NOT NULL,
    [CustomerID]          INT            NULL,
    [CustomerName]        NVARCHAR (100) NULL,
    [ResponsiblePersonID] INT            NULL,
    [PatientID]           INT            NULL,
    [PatientFullName]     NVARCHAR (150) NULL,
    [Created]             DATETIME       NULL,
    [DateDelivery]        DATETIME       NULL,
    [DateComment]         NVARCHAR (50)  NULL,
    [ProstheticArticul]   NVARCHAR (50)  NULL,
    [WorkDescription]     NVARCHAR (250) NULL,
    [FlagWorkAccept]      BIT            CONSTRAINT [DF_WorkOrder_WorkAccept] DEFAULT ((0)) NOT NULL,
    [OfficeAdminID]       INT            NULL,
    [OfficeAdminName]     NVARCHAR (50)  NULL,
    [Closed]              DATETIME       NULL,
    CONSTRAINT [PK_WorkOrder] PRIMARY KEY CLUSTERED ([WorkOrderID] ASC),
    CONSTRAINT [FK_WorkOrder_Branches] FOREIGN KEY ([BranchTypeID]) REFERENCES [dbo].[BranchTypes] ([BranchTypeID]) ON UPDATE CASCADE,
    CONSTRAINT [FK_WorkOrder_Customers] FOREIGN KEY ([CustomerID]) REFERENCES [dbo].[Customers] ([CustomerID])
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


GO
-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 12.01.2020
-- Description:	История действий пользователей с заказ-нарядами
-- =============================================
CREATE TRIGGER [dbo].[WorkOrderHistory] 
   ON  [dbo].[WorkOrder] 
   AFTER INSERT,UPDATE,DELETE
AS 
BEGIN
	
	SET NOCOUNT ON;

	IF EXISTS 
	(	SELECT * FROM inserted i, deleted d WHERE
		i.BranchTypeID = d.BranchTypeID
		AND ISNULL(i.Closed, '99991231') = ISNULL(d.Closed, '99991231')
		AND ISNULL(i.Created, '17530101') = ISNULL(d.Created, '17530101')
		AND ISNULL(i.CustomerID, -19999) = ISNULL(d.CustomerID, -19999)
		AND ISNULL(i.CustomerName, 'empty') = ISNULL(d.CustomerName, 'empty')
		AND ISNULL(i.DateDelivery, '99991231') = ISNULL(d.DateDelivery, '99991231')
		AND ISNULL(i.DocNumber, 'empty') = ISNULL(d.DocNumber, 'empty')
		AND ISNULL(i.ProstheticArticul, 'empty') = ISNULL(d.ProstheticArticul, 'empty')
		AND i.FlagWorkAccept = d.FlagWorkAccept
		AND ISNULL(i.OfficeAdminID, -19999) = ISNULL(d.OfficeAdminID, -19999)
		AND ISNULL(i.OfficeAdminName, 'empty') = ISNULL(d.OfficeAdminName, 'empty')
		AND ISNULL(i.PatientID, -19999) = ISNULL(d.PatientID, -19999)
		AND ISNULL(i.PatientFullName, 'empty') = ISNULL(d.PatientFullName, 'empty')
		AND ISNULL(i.ResponsiblePersonID, -19999) = ISNULL(d.ResponsiblePersonID, -19999)
		AND i.Status = d.Status
		AND ISNULL(i.WorkDescription, 'empty') = ISNULL(d.WorkDescription, 'empty')
		AND i.WorkOrderID = d.WorkOrderID
	) BEGIN
		RETURN
	END

    DECLARE  @Update VARCHAR(10)
       SET @Update = CASE WHEN EXISTS(SELECT * FROM inserted ) AND EXISTS(SELECT * FROM deleted ) THEN 'UPDATE ' ELSE '' END
       IF EXISTS(SELECT * FROM deleted) BEGIN
          INSERT dbo.HistoryWorkOrder
          SELECT GetDate(), SUSER_SNAME(), @Update + 'DELETE' , *
          FROM deleted
       END
       IF EXISTS(SELECT * FROM inserted) BEGIN
          INSERT dbo.HistoryWorkOrder
          SELECT  GetDate(), SUSER_SNAME(), @Update + 'INSERT' , *
          FROM inserted
       END

END