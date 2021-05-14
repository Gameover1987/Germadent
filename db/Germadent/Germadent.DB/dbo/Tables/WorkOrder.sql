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
    [Created]             DATETIME       NULL,
    [FittingDate]         DATETIME       NULL,
    [DateOfCompletion]    DATETIME       NULL,
    [DateComment]         NVARCHAR (50)  NULL,
    [ProstheticArticul]   NVARCHAR (50)  NULL,
    [WorkDescription]     NVARCHAR (250) NULL,
    [UrgencyRatio]        FLOAT (53)     NULL,
    [FlagWorkAccept]      BIT            CONSTRAINT [DF_WorkOrder_WorkAccept] DEFAULT ((1)) NOT NULL,
    [FlagStl]             BIT            NULL,
    [FlagCashless]        BIT            NULL,
    [CreatorID]           INT            NULL,
    [Closed]              DATETIME       NULL,
    CONSTRAINT [PK_WorkOrder] PRIMARY KEY CLUSTERED ([WorkOrderID] ASC),
    CONSTRAINT [FK_WorkOrder_BranchTypes] FOREIGN KEY ([BranchTypeID]) REFERENCES [dbo].[BranchTypes] ([BranchTypeID]),
    CONSTRAINT [FK_WorkOrder_Customers] FOREIGN KEY ([CustomerID]) REFERENCES [dbo].[Customers] ([CustomerID]),
    CONSTRAINT [FK_WorkOrder_ResponsiblePersons] FOREIGN KEY ([ResponsiblePersonID]) REFERENCES [dbo].[ResponsiblePersons] ([ResponsiblePersonID]),
    CONSTRAINT [FK_WorkOrder_Users] FOREIGN KEY ([CreatorID]) REFERENCES [dbo].[Users] ([UserID])
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
		i.WorkOrderID = d.WorkOrderID
		AND i.BranchTypeID = d.BranchTypeID
		AND i.Status = d.Status
		AND ISNULL(i.DocNumber, 'empty') = ISNULL(d.DocNumber, 'empty')
		AND ISNULL(i.CustomerID, -19999) = ISNULL(d.CustomerID, -19999)
		AND ISNULL(i.ResponsiblePersonID, -19999) = ISNULL(d.ResponsiblePersonID, -19999)
		AND ISNULL(i.PatientFullName, 'empty') = ISNULL(d.PatientFullName, 'empty')
		AND ISNULL(i.PatientAge, 0) = ISNULL(d.PatientAge, 0)
		AND ISNULL(i.Created, '17530101') = ISNULL(d.Created, '17530101')		
		AND ISNULL(i.FittingDate, '17530101') = ISNULL(d.FittingDate, '17530101')
		AND ISNULL(i.DateOfCompletion, '17530101') = ISNULL(d.DateOfCompletion, '17530101')
		AND ISNULL(i.DateComment, 'empty') = ISNULL(d.DateComment, 'empty')
		AND ISNULL(i.ProstheticArticul, 'empty') = ISNULL(d.ProstheticArticul, 'empty')
		AND ISNULL(i.WorkDescription, 'empty') = ISNULL(d.WorkDescription, 'empty')
		AND i.UrgencyRatio = d.UrgencyRatio
		AND i.FlagWorkAccept = d.FlagWorkAccept
		AND i.FlagStl = d.FlagStl
		AND i.FlagCashless = d.FlagCashless
		AND ISNULL(i.CreatorID, -19999) = ISNULL(d.CreatorID, -19999)
		AND ISNULL(i.Closed, '17530101') = ISNULL(d.Closed, '17530101')		
		 
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

