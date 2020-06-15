CREATE TABLE [dbo].[WorkOrderDL] (
    [WorkOrderDLID]    INT            NOT NULL,
    [TransparenceID]   INT            NULL,
    [ColorAndFeatures] NVARCHAR (100) NULL,
    CONSTRAINT [PK_WorkOrderDL] PRIMARY KEY CLUSTERED ([WorkOrderDLID] ASC),
    CONSTRAINT [FK_WorkOrderDL_Transparences] FOREIGN KEY ([TransparenceID]) REFERENCES [dbo].[Transparences] ([TransparenceID]),
    CONSTRAINT [FK_WorkOrderDL_WorkOrder] FOREIGN KEY ([WorkOrderDLID]) REFERENCES [dbo].[WorkOrder] ([WorkOrderID]) ON DELETE CASCADE
);




















GO
-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 12.01.2020
-- Description:	История действий пользователей с заказ-нарядами ЗТЛ
-- =============================================
CREATE TRIGGER [dbo].[WorkOrderDLHistory] 
   ON  [dbo].[WorkOrderDL] 
   AFTER INSERT,UPDATE,DELETE
AS 
BEGIN
	
	SET NOCOUNT ON;

	IF EXISTS 
	(	SELECT * FROM inserted i, deleted d WHERE
			ISNULL(i.ColorAndFeatures, 'empty') = ISNULL(d.ColorAndFeatures, 'empty')		
		AND ISNULL(i.TransparenceID, 0) = ISNULL(d.TransparenceID, 0)
		AND i.WorkOrderDLID = d.WorkOrderDLID
	) BEGIN
		RETURN
	END


    DECLARE  @Update VARCHAR(10)
    SET @Update = CASE WHEN EXISTS(SELECT * FROM inserted ) AND EXISTS(SELECT * FROM deleted ) THEN 'UPDATE ' ELSE '' END
    IF EXISTS(SELECT * FROM deleted ) BEGIN
        INSERT dbo.HistoryWorkOrderDL
        SELECT GetDate(), SUSER_SNAME(), @Update + 'DELETE' , *
        FROM deleted
    END
    IF EXISTS(SELECT * FROM inserted ) BEGIN
        INSERT dbo.HistoryWorkOrderDL
        SELECT  GetDate(), SUSER_SNAME(), @Update + 'INSERT' , *
        FROM inserted
    END

END