using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Germadent.Rma.App.Infrastructure;
using Germadent.Rma.App.ViewModels;
using Germadent.UI.Helpers;

namespace Germadent.Rma.App.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IMainViewModel _mainViewModel;

        public MainWindow()
        {
            InitializeComponent();

            Loaded += OnLoaded;
            Closing += OnClosing;
        }

        private void MainViewModelOnColumnSettingsChanged(object? sender, EventArgs e)
        {
            UpdateColumns(_mainViewModel.SettingsManager.Columns);
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            var columns = GetColumnSettings();
            _mainViewModel.SettingsManager.Columns = columns;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            

            UpdateColumns(_mainViewModel.SettingsManager.Columns);
        }

        private void OnOrderRowDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var mainViewModel = (IMainViewModel)DataContext;
            mainViewModel.OpenOrderCommand.TryExecute();
        }

        private void MainWindow_OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            _mainViewModel = (IMainViewModel)DataContext;
            _mainViewModel.Initialize();

            var columns = GetColumnSettings();

            _mainViewModel.ColumnSettingsChanged += MainViewModelOnColumnSettingsChanged;
        }

        private void OrdersGrid_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_mainViewModel.SelectedOrder == null)
                return;

            OrdersGrid.ScrollIntoView(_mainViewModel.SelectedOrder);
        }

        private void UpdateColumns(ColumnInfo[] columns)
        {
            foreach (var columnInfo in columns)
            {
                var dataGridColumn = OrdersGrid.Columns.FirstOrDefault(x => DataGridColumnNameHelper.GetName(x) == columnInfo.Name);
                if (dataGridColumn == null)
                    continue;
                if (columnInfo.Width > 0)
                    dataGridColumn.Width = columnInfo.Width;
                dataGridColumn.Visibility = columnInfo.IsVisible ? Visibility.Visible : Visibility.Collapsed;
                dataGridColumn.SortDirection = columnInfo.SortDirection;
                dataGridColumn.DisplayIndex = columnInfo.DisplayIndex;
            }
        }

        public ColumnInfo[] GetColumnSettings()
        {
            var columnInfos = new List<ColumnInfo>();
            foreach (var dataGridColumn in OrdersGrid.Columns)
            {
                var columnInfo = new ColumnInfo
                {
                    DisplayIndex = dataGridColumn.DisplayIndex,
                    IsVisible = dataGridColumn.Visibility == Visibility.Visible,
                    Name = DataGridColumnNameHelper.GetName(dataGridColumn),
                    DisplayName = dataGridColumn.Header.ToString(),
                    SortDirection = dataGridColumn.SortDirection,
                    Width = dataGridColumn.ActualWidth
                };
                columnInfos.Add(columnInfo);
            }

            return columnInfos.ToArray();
        }
    }
}
