CREATE TABLE [dbo].[WorkOrder] (
    [WorkOrderID]         INT            IDENTITY (1, 1) NOT NULL,
    [BranchTypeID]        INT            NOT NULL,
    [Status]              INT            CONSTRAINT [DF_WorkOrder_Status] DEFAULT ((0)) NOT NULL,
    [DocNumber]           NVARCHAR (10)  NOT NULL,
    [CustomerID]          INT            NOT NULL,
    [ResponsiblePersonID] INT            NULL,
    [PatientFullName]     NVARCHAR (150) NULL,
    [PatientGender]       BIT            NULL,
    [PatientAge]          TINYINT        NULL,
    [Created]             DATETIME       NOT NULL,
    [FittingDate]         DATETIME       NULL,
    [DateOfCompletion]    DATETIME       NULL,
    [DateComment]         NVARCHAR (50)  NULL,
    [ProstheticArticul]   NVARCHAR (50)  NULL,
    [WorkDescription]     NVARCHAR (250) NULL,
    [FlagWorkAccept]      BIT            CONSTRAINT [DF_WorkOrder_WorkAccept] DEFAULT ((1)) NOT NULL,
    [OfficeAdminID]       INT            NULL,
    [OfficeAdminName]     NVARCHAR (50)  NULL,
    [Closed]              DATETIME       NULL,
    [ReaderUserID]        INT            NULL,
    [ReadingDateTime]     DATETIME       NULL,
    CONSTRAINT [PK_WorkOrder] PRIMARY KEY CLUSTERED ([WorkOrderID] ASC),
    CONSTRAINT [FK_WorkOrder_BranchTypes] FOREIGN KEY ([BranchTypeID]) REFERENCES [dbo].[BranchTypes] ([BranchTypeID]),
    CONSTRAINT [FK_WorkOrder_Customers] FOREIGN KEY ([CustomerID]) REFERENCES [dbo].[Customers] ([CustomerID]),
    CONSTRAINT [FK_WorkOrder_ResponsiblePersons] FOREIGN KEY ([ResponsiblePersonID]) REFERENCES [dbo].[ResponsiblePersons] ([ResponsiblePersonID]),
    CONSTRAINT [FK_WorkOrder_Users] FOREIGN KEY ([ReaderUserID]) REFERENCES [dbo].[Users] ([UserID])
);






































GO



GO



GO



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
		AND ISNULL(i.DocNumber, 'empty') = ISNULL(d.DocNumber, 'empty')
		AND ISNULL(i.ProstheticArticul, 'empty') = ISNULL(d.ProstheticArticul, 'empty')
		AND i.FlagWorkAccept = d.FlagWorkAccept
		AND ISNULL(i.OfficeAdminID, -19999) = ISNULL(d.OfficeAdminID, -19999)
		AND ISNULL(i.OfficeAdminName, 'empty') = ISNULL(d.OfficeAdminName, 'empty')
		AND ISNULL(i.PatientFullName, 'empty') = ISNULL(d.PatientFullName, 'empty')
		AND ISNULL(i.DateOfCompletion, '17530101') = ISNULL(d.DateOfCompletion, '17530101')
		AND ISNULL(i.FittingDate, '17530101') = ISNULL(d.FittingDate, '17530101')
		AND ISNULL(i.PatientAge, 0) = ISNULL(d.PatientAge, 0)
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
GO
CREATE NONCLUSTERED INDEX [IX_WorkOrder_ResponsiblePersonID]
    ON [dbo].[WorkOrder]([ResponsiblePersonID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_WorkOrder_CustomerID]
    ON [dbo].[WorkOrder]([CustomerID] ASC);

