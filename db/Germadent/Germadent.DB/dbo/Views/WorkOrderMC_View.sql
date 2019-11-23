CREATE VIEW dbo.WorkOrderMC_View
AS
SELECT   wo.WorkOrderID, b.BranchName, wo.DocNumber, wo.Status, cs.CustomerName, p.FamilyName, wmc.AdditionalInfo, wmc.Carcass, wmc.ImplantSystem, wmc.IndividualAbutmentProcessing, e.FamilyName AS Администратор,
                    (SELECT   FamilyName
                     FROM      dbo.Employee AS em
                     WHERE   (EmployeeID = wp.EmployeeID) AND (tw.Position LIKE '%оператор%')) AS Оператор,
                    (SELECT   FamilyName
                     FROM      dbo.Employee AS em
                     WHERE   (EmployeeID = wp.EmployeeID) AND (tw.Position LIKE '%техник%')) AS Техник,
                    (SELECT   FamilyName
                     FROM      dbo.Employee AS em
                     WHERE   (EmployeeID = wp.EmployeeID) AND (tw.Position LIKE '%моделиров%')) AS Моделировщик
FROM      dbo.WorkOrder AS wo INNER JOIN
                dbo.WorkOrderMC AS wmc ON wo.WorkOrderID = wmc.WorkOrderMCID INNER JOIN
                dbo.Branches AS b ON wo.BrachID = b.BranchID INNER JOIN
                dbo.Customers AS cs ON wo.CustomerID = cs.CustomerID INNER JOIN
                dbo.Patients AS p ON wo.PatientID = p.PatientID INNER JOIN
                dbo.Users AS u ON wo.UserIDCreated = u.UserID AND wo.UserIDLastUpdated = u.UserID INNER JOIN
                dbo.Employee AS e ON wo.AdministratorID = e.EmployeeID INNER JOIN
                dbo.WorkProcesses AS wp ON wo.WorkOrderID = wp.WorkOrderID INNER JOIN
                dbo.TypesOfWorks AS tw ON wp.WorkID = tw.WorkID

GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[50] 4[26] 2[24] 3) )"
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
         Begin Table = "wmc"
            Begin Extent = 
               Top = 19
               Left = 94
               Bottom = 197
               Right = 358
            End
            DisplayFlags = 280
            TopColumn = 1
         End
         Begin Table = "b"
            Begin Extent = 
               Top = 216
               Left = 95
               Bottom = 331
               Right = 276
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cs"
            Begin Extent = 
               Top = 350
               Left = 63
               Bottom = 478
               Right = 244
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "p"
            Begin Extent = 
               Top = 495
               Left = 61
               Bottom = 710
               Right = 242
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "u"
            Begin Extent = 
               Top = 741
               Left = 122
               Bottom = 906
               Right = 303
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "e"
            Begin Extent = 
               Top = 203
               Left = 702
               Bottom = 380
               Right = 883
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "wp"
            Begin Extent = 
               Top = 23
               Left = 742
               Bottom = 186
               Right = 923
            End
            DisplayFlags = 280
            TopColumn = 0
         ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'WorkOrderMC_View';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'End
         Begin Table = "tw"
            Begin Extent = 
               Top = 6
               Left = 398
               Bottom = 128
               Right = 579
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "wo"
            Begin Extent = 
               Top = 153
               Left = 428
               Bottom = 570
               Right = 635
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
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'WorkOrderMC_View';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'WorkOrderMC_View';

