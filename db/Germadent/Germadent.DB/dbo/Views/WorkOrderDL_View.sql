CREATE VIEW dbo.WorkOrderDL_View
AS
SELECT   wo.WorkOrderID, wo.BranchTypeID, wo.DocNumber, wo.CustomerID, wo.CustomerName, wo.ResponsiblePersonID, wo.PatientID, wo.Created, wo.WorkDescription, wo.OfficeAdminID, wo.OfficeAdminName, wo.PatientFullName, 
                wdl.PatientAge, wdl.TransparenceID, wdl.FittingDate, wdl.DateOfCompletion, wdl.ColorAndFeatures, wo.Status, wo.DateDelivery, wo.FlagWorkAccept, wo.Closed, wo.ProstheticArticul
FROM      dbo.WorkOrder AS wo INNER JOIN
                dbo.WorkOrderDL AS wdl ON wo.WorkOrderID = wdl.WorkOrderDLID
GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 1, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'WorkOrderDL_View';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[41] 4[16] 2[37] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "wo"
            Begin Extent = 
               Top = 6
               Left = 40
               Bottom = 371
               Right = 253
            End
            DisplayFlags = 280
            TopColumn = 2
         End
         Begin Table = "wdl"
            Begin Extent = 
               Top = 5
               Left = 351
               Bottom = 257
               Right = 549
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 2723
         Alias = 898
         Table = 1169
         Output = 727
         Append = 1400
         NewValue = 1170
         SortType = 1354
         SortOrder = 1411
         GroupBy = 1350
         Filter = 1354
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'WorkOrderDL_View';



