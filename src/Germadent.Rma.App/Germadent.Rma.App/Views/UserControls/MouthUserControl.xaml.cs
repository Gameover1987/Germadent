using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Germadent.Rma.App.ViewModels;
using Germadent.Rma.App.ViewModels.Wizard;

namespace Germadent.Rma.App.Views.UserControls
{
    /// <summary>
    /// Interaction logic for MouthUserControl.xaml
    /// </summary>
    public partial class MouthUserControl : UserControl
    {
        private List<Bridge> _bridges = new List<Bridge>();

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

            RenderMouth(DataContext as IToothCardViewModel);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            RenderMouth(DataContext as IToothCardViewModel);
        }

        private void RenderMouth(IToothCardViewModel mouth)
        {
            if (mouth == null)
                return;

            var canvasWidth = _mouthListBox.ActualWidth;
            var canvasHeight = _mouthListBox.ActualHeight - 40;

            var a = canvasWidth / 2.0;
            var b = canvasHeight / 2.0;

            var stepByWidth = ((canvasWidth / 2.0) - 10) / 8;

            var placement1 = new List<TeethPlacement>();
            var placement2 = new List<TeethPlacement>();
            var placement3 = new List<TeethPlacement>();
            var placement4 = new List<TeethPlacement>();

            for (int i = 0; i < 8; i++)
            {
                var x = 20 + stepByWidth * i;
                var y = b * Math.Sqrt(1 - Math.Pow(x, 2) / Math.Pow(a, 2));
                var point1 = new Point(a + x, b - y);
                var point2 = new Point(a + x, b + y);
                var point3 = new Point(a - x, b + y);
                var point4 = new Point(a - x, b - y);

                placement1.Add(PlaceTeeth(i, point1));
                placement2.Add(PlaceTeeth(i + 8, point2));
                placement3.Add(PlaceTeeth(i + 16, point3));
                placement4.Add(PlaceTeeth(i + 24, point4));
            }

            var placements = new List<TeethPlacement>();
            placement4.Reverse();
            placements.AddRange(placement4);
            placements.AddRange(placement1);
            placements.AddRange(placement2);
            placements.AddRange(placement3);

            foreach (var teethPlacement in placements)
            {
                Canvas.SetLeft(teethPlacement.ListBoxItem, teethPlacement.Position.X);
                Canvas.SetTop(teethPlacement.ListBoxItem, teethPlacement.Position.Y);
            }

            DrawBridges(placements);
        }

        private void DrawBridges(List<TeethPlacement> placements)
        {
            _bridges.Clear();
            Bridge newBridge = null;
            for (int i = 0; i < placements.Count - 1; i++)
            {
                var currentPlacement = placements[i];
                if (currentPlacement.Teeth.HasBridge)
                {
                    if (newBridge == null)
                        newBridge = new Bridge();
                    newBridge.Teeths.Add(currentPlacement);
                }
                else
                {
                    if (newBridge != null)
                    {
                        _bridges.Add(newBridge);
                        newBridge = null;
                    }
                }
            }

            var allBridgeDrawings = new GeometryGroup();
            foreach (var bridge in _bridges)
            {
                allBridgeDrawings.Children.Add(bridge.Draw());
            }

            toothCardPath.Data = allBridgeDrawings;
        }

        private TeethPlacement PlaceTeeth(int i, Point point)
        {
            var listBoxItem = (ListBoxItem)_mouthListBox.ItemContainerGenerator.ContainerFromIndex(i);

            var teethViewModel = (ToothViewModel)listBoxItem.DataContext;

            return new TeethPlacement
            {
                Position = point,
                Teeth = teethViewModel,
                ListBoxItem = listBoxItem
            };
        }

        private class Bridge
        {
            public Bridge()
            {
                Teeths = new List<TeethPlacement>();
            }

            public List<TeethPlacement> Teeths { get; }

            public Geometry Draw()
            {
                var geometryGroup = new GeometryGroup();

                for (int i = 0; i < Teeths.Count - 1; i++)
                {
                    var teeth = Teeths[i];
                    var nextTeeth = Teeths[i + 1];
                    geometryGroup.Children.Add(new LineGeometry(teeth.CorrectedPosition, nextTeeth.CorrectedPosition));
                }

                return geometryGroup;
            }
        }

        private class TeethPlacement
        {
            public Point Position { get; set; }

            public ToothViewModel Teeth { get; set; }

            public ListBoxItem ListBoxItem { get; set; }

            public Point CorrectedPosition => new Point(Position.X + ListBoxItem.ActualWidth / 2, Position.Y + ListBoxItem.ActualHeight / 2);

            public override string ToString()
            {
                return Teeth.ToString() + " " + Position;
            }
        }
    }
}
