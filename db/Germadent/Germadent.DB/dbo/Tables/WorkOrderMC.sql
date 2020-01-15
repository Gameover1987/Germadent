CREATE TABLE [dbo].[WorkOrderMC] (
    [WorkOrderMCID]                INT            NOT NULL,
    [TechnicFullName]              NVARCHAR (150) NULL,
    [TechnicPhone]                 VARCHAR (20)   NULL,
    [AdditionalInfo]               NVARCHAR (70)  NULL,
    [CarcassColor]                 NVARCHAR (30)  NULL,
    [ImplantSystem]                NVARCHAR (70)  NULL,
    [IndividualAbutmentProcessing] NVARCHAR (80)  NULL,
    [Understaff]                   NVARCHAR (100) NULL,
    CONSTRAINT [PK_WorkOrderMC] PRIMARY KEY CLUSTERED ([WorkOrderMCID] ASC),
    CONSTRAINT [FK_WorkOrderMC_WorkOrder] FOREIGN KEY ([WorkOrderMCID]) REFERENCES [dbo].[WorkOrder] ([WorkOrderID])
);








GO
-- =============================================
-- Author:		Алексей Колосенок
-- Create date: 12.01.2020
-- Description:	История действий пользователей с заказ-нарядами ФЦ
-- =============================================
CREATE TRIGGER [dbo].[WorkOrderMCHistory] 
   ON  [dbo].[WorkOrderMC] 
   AFTER INSERT,UPDATE,DELETE
AS 
BEGIN
	
	SET NOCOUNT ON;
	
	IF EXISTS 
	(
		SELECT * FROM inserted i, deleted d WHERE
			ISNULL(i.AdditionalInfo, 'empty') = ISNULL(d.AdditionalInfo, 'empty')
		AND ISNULL(i.CarcassColor, 'empty') = ISNULL(d.CarcassColor, 'empty')
		AND ISNULL(i.ImplantSystem, 'empty') = ISNULL(d.ImplantSystem, 'empty')
		AND ISNULL(i.IndividualAbutmentProcessing, 'empty') = ISNULL(d.IndividualAbutmentProcessing, 'empty')
		AND ISNULL(i.TechnicFullName, 'empty') = ISNULL(d.TechnicFullName, 'empty')
		AND ISNULL(i.TechnicPhone, 'empty') = ISNULL(d.TechnicPhone, 'empty')
		AND ISNULL(i.Understaff, 'empty') = ISNULL(d.Understaff, 'empty')
		AND i.WorkOrderMCID = d.WorkOrderMCID
	) BEGIN
		RETURN
	END


    DECLARE  @Update VARCHAR(10)
    SET @Update = CASE WHEN EXISTS(SELECT * FROM inserted ) AND EXISTS(SELECT * FROM deleted ) THEN 'UPDATE ' ELSE '' END
    IF EXISTS(SELECT * FROM deleted ) BEGIN
        INSERT dbo.HistoryWorkOrderMC
        SELECT GetDate(), SUSER_SNAME(), @Update + 'DELETE' , *
        FROM deleted
    END
    IF EXISTS(SELECT * FROM inserted ) BEGIN
        INSERT dbo.HistoryWorkOrderMC
        SELECT  GetDate(), SUSER_SNAME(), @Update + 'INSERT' , *
        FROM inserted
    END

END