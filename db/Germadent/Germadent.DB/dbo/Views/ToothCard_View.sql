﻿CREATE VIEW dbo.ToothCard_View
AS
SELECT   tc.WorkOrderID, tc.ToothNumber, wo.DocNumber, tc.MaterialID AS Expr1, dbo.PricePositions.PricePositionCode, dbo.TypesOfProsthetics.ProstheticsName
FROM      dbo.ToothCard AS tc INNER JOIN
                dbo.Materials AS m ON tc.MaterialID = m.MaterialID INNER JOIN
                dbo.WorkOrder AS wo ON tc.WorkOrderID = wo.WorkOrderID INNER JOIN
                dbo.TypesOfProsthetics ON tc.ProstheticsID = dbo.TypesOfProsthetics.ProstheticsID INNER JOIN
                dbo.PricePositions ON tc.PricePositionID = dbo.PricePositions.PricePositionID AND m.MaterialID = dbo.PricePositions.MaterialID
WHERE   (m.FlagUnused IS NULL) OR
                (m.FlagUnused <> 1)
GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'ToothCard_View';




GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[43] 4[23] 2[16] 3) )"
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
         Begin Table = "tc"
            Begin Extent = 
               Top = 9
               Left = 289
               Bottom = 217
               Right = 470
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "m"
            Begin Extent = 
               Top = 105
               Left = 602
               Bottom = 242
               Right = 783
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "wo"
            Begin Extent = 
               Top = 8
               Left = 4
               Bottom = 360
               Right = 203
            End
            DisplayFlags = 280
            TopColumn = 1
         End
         Begin Table = "PricePositions"
            Begin Extent = 
               Top = 0
               Left = 841
               Bottom = 168
               Right = 1040
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "TypesOfProsthetics"
            Begin Extent = 
               Top = 168
               Left = 823
               Bottom = 271
               Right = 1011
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
         Column = 2338
         Alias = 898
         Table = 1540
         Output = 727
         Append = 1400
         NewValue = 1170
         SortType = 1354
         SortOrder = 1411
         GroupBy = 1350
         Filter = 1354
         Or = 1350
         Or = 1350
         Or = 1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'ToothCard_View';












GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'350
      End
   End
End
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'ToothCard_View';



