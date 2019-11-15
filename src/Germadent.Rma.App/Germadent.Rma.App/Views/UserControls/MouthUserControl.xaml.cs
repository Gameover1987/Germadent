using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Germadent.Rma.App.ViewModels;

namespace Germadent.Rma.App.Views.UserControls
{
    /// <summary>
    /// Interaction logic for MouthUserControl.xaml
    /// </summary>
    public partial class MouthUserControl : UserControl
    {
        private IOrderViewModel _orderViewModel;

        public MouthUserControl()
        {
            InitializeComponent();

            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _orderViewModel = DataContext as IOrderViewModel;
            if (_orderViewModel == null)
                return;

            RenderMouth();
        }

        private void RenderMouth()
        {
            var canvasWidth = _mouthListBox.ActualWidth;
            var canvasHeight = _mouthListBox.ActualHeight - 40;

            var a = canvasWidth / 2.0;
            var b = canvasHeight / 2.0;

            var stepByWidth = ((canvasWidth / 2.0) - 10) / 8;
            var points1 = new List<Point>();
            for (int i = 0; i < 8; i++)
            {
                var x = 20 + stepByWidth * i;
                var y = b * Math.Sqrt(1 - Math.Pow(x, 2) / Math.Pow(a, 2));
                var point1 = new Point(a + x, b - y);
                var point2 = new Point(a + x, b + y);
                var point3 = new Point(a - x, b + y);
                var point4 = new Point(a - x, b - y);
                points1.Add(point1);

                PlaceTeeth(i, point1);
                PlaceTeeth(i + 8, point2);
                PlaceTeeth(i + 16, point3);
                PlaceTeeth(i + 24, point4);
            }
        }

        private void PlaceTeeth(int i, Point point)
        {
            var listBoxItem1 = (ListBoxItem)_mouthListBox.ItemContainerGenerator.ContainerFromIndex(i);

            Canvas.SetLeft(listBoxItem1, point.X);
            Canvas.SetTop(listBoxItem1, point.Y);
        }
    }
}
