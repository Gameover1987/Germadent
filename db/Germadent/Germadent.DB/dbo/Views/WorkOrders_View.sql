CREATE VIEW dbo.WorkOrders_View
AS
SELECT   wo.WorkOrderID, wo.BranchTypeID, wo.Status, wo.DocNumber, wo.CustomerID, cs.CustomerName, wo.ResponsiblePersonID, wo.Created, wo.WorkDescription, wo.FlagWorkAccept, wo.OfficeAdminID, wo.OfficeAdminName, wo.Closed, 
                wdl.TransparenceID, rp.RP_Position, rp.ResponsiblePerson, wo.PatientFullName, wo.PatientAge, wo.FittingDate, wo.DateOfCompletion, wdl.ColorAndFeatures, wmc.AdditionalInfo, wmc.CarcassColor, wmc.ImplantSystem, 
                wmc.IndividualAbutmentProcessing, wmc.Understaff, wo.DateComment, wo.ProstheticArticul, wo.PatientGender, e.EquipmentName, ae.QuantityIn, ae.QuantityOut
FROM      dbo.WorkOrder AS wo INNER JOIN
                dbo.Customers AS cs ON wo.CustomerID = cs.CustomerID LEFT OUTER JOIN
                dbo.ResponsiblePersons AS rp ON wo.ResponsiblePersonID = rp.ResponsiblePersonID LEFT OUTER JOIN
                dbo.WorkOrderDL AS wdl ON wo.WorkOrderID = wdl.WorkOrderDLID LEFT OUTER JOIN
                dbo.WorkOrderMC AS wmc ON wo.WorkOrderID = wmc.WorkOrderMCID LEFT OUTER JOIN
                dbo.AdditionalEquipment AS ae ON wo.WorkOrderID = ae.WorkOrderID LEFT OUTER JOIN
                dbo.Equipments AS e ON ae.EquipmentID = e.EquipmentID
GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'WorkOrders_View';




GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[60] 4[10] 2[18] 3) )"
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
               Left = 286
               Bottom = 517
               Right = 485
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cs"
            Begin Extent = 
               Top = 7
               Left = 11
               Bottom = 191
               Right = 223
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "rp"
            Begin Extent = 
               Top = 203
               Left = 10
               Bottom = 408
               Right = 223
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "wdl"
            Begin Extent = 
               Top = 287
               Left = 536
               Bottom = 426
               Right = 711
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "wmc"
            Begin Extent = 
               Top = 7
               Left = 685
               Bottom = 193
               Right = 936
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ae"
            Begin Extent = 
               Top = 211
               Left = 743
               Bottom = 354
               Right = 924
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "e"
            Begin Extent = 
               Top = 228
               Left = 1039
               Bottom = 336
               Right = 1214
            End
            DisplayFlags = 280
            TopColumn = 0
     ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'WorkOrders_View';




















GO
GRANT SELECT
    ON OBJECT::[dbo].[WorkOrders_View] TO [gdl_user]
    AS [dbo];


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'    End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1426
         Width = 1426
         Width = 1426
         Width = 1426
         Width = 1426
         Width = 1426
         Width = 1426
         Width = 1426
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
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
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'WorkOrders_View';







