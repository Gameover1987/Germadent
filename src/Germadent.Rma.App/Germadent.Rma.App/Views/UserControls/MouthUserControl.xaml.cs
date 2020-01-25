using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Germadent.Rma.App.ViewModels.Wizard;

namespace Germadent.Rma.App.Views.UserControls
{
    /// <summary>
    /// Interaction logic for MouthUserControl.xaml
    /// </summary>
    public partial class MouthUserControl : UserControl
    {
        private readonly List<Point> _points = new List<Point>();

        public MouthUserControl()
        {
            InitializeComponent();

            Loaded += OnLoaded;
            SizeChanged += OnSizeChanged;
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (!IsLoaded)
                return;

            RenderMouth(DataContext as IMouthProvider);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            RenderMouth(DataContext as IMouthProvider);
        }

        private void RenderMouth(IMouthProvider mouth)
        {
            if (mouth == null)
                return;

            var canvasWidth = _mouthListBox.ActualWidth;
            var canvasHeight = _mouthListBox.ActualHeight - 40;

            var a = canvasWidth / 2.0;
            var b = canvasHeight / 2.0;

            var stepByWidth = ((canvasWidth / 2.0) - 10) / 8;

            _points.Clear();
            var points1 = new List<Point>();
            var points2 = new List<Point>();
            var points3 = new List<Point>();
            var points4 = new List<Point>();

            for (int i = 0; i < 8; i++)
            {
                var x = 20 + stepByWidth * i;
                var y = b * Math.Sqrt(1 - Math.Pow(x, 2) / Math.Pow(a, 2));
                var point1 = new Point(a + x, b - y);
                var point2 = new Point(a + x, b + y);
                var point3 = new Point(a - x, b + y);
                var point4 = new Point(a - x, b - y);

                points1.Add(point1);
                points2.Add(point2);
                points3.Add(point3);
                points4.Add(point4);

                PlaceTeeth(i, point1);
                PlaceTeeth(i + 8, point2);
                PlaceTeeth(i + 16, point3);
                PlaceTeeth(i + 24, point4);
            }

            _points.AddRange(points1);
            _points.AddRange(points2);
            _points.AddRange(points3);
            _points.AddRange(points4);
        }

        private void PlaceTeeth(int i, Point point)
        {
            var listBoxItem1 = (ListBoxItem)_mouthListBox.ItemContainerGenerator.ContainerFromIndex(i);

            Canvas.SetLeft(listBoxItem1, point.X);
            Canvas.SetTop(listBoxItem1, point.Y);
        }

        private class Bridge
        {
            public Bridge()
            {
                Points = new List<Point>();
            }

            public List<Point> Points { get; }
        }
    }
}
