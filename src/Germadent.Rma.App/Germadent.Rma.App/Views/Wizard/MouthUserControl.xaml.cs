using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Germadent.Rma.App.ViewModels.ToothCard;

namespace Germadent.Rma.App.Views.Wizard
{
    /// <summary>
    /// Interaction logic for MouthUserControl.xaml
    /// </summary>
    public partial class MouthUserControl : UserControl
    {
        private static readonly int[] StopNumbers = new int[] { 18, 48, 28, 38 };

        private readonly List<Bridge> _bridges = new List<Bridge>();
        private IToothCardViewModel _toothCard;

        public MouthUserControl()
        {
            InitializeComponent();

            Loaded += OnLoaded;
            SizeChanged += OnSizeChanged;
            DataContextChanged += OnDataContextChanged;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (_toothCard != null)
            {
                _toothCard.RenderRequest -= ToothCardOnRenderRequest;
            }

            if (DataContext == null)
                return;

            _toothCard = ((IToothCardContainer)DataContext).ToothCard;
            _toothCard.RenderRequest += ToothCardOnRenderRequest;

        }

        private void ToothCardOnRenderRequest(object sender, EventArgs e)
        {
            RenderMouth();
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (!IsLoaded)
                return;

            RenderMouth();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            RenderMouth();
        }

        private void RenderMouth()
        {
            if (_toothCard == null)
                return;

            var canvasWidth = _mouthListBox.ActualWidth - 6;



            var stepByWidth = canvasWidth / 16;
            var horizontalMargin = 5;
            var verticalMargin = 10;

            var allPlacements = new List<TeethPlacement>();
            var placements1 = new List<TeethPlacement>();
            for (int i = 0; i < 16; i++)
            {
                var point = new Point((stepByWidth * i) + horizontalMargin, verticalMargin);
                placements1.Add(PlaceTeeth(i, point));
            }

            var placements2 = new List<TeethPlacement>();
            var decrement = 15;
            for (int i = 16; i < 32; i++)
            {
                var point = new Point((stepByWidth * decrement) + horizontalMargin, verticalMargin + 45);
                placements2.Add(PlaceTeeth(i, point));
                decrement--;
            }

            allPlacements.AddRange(placements1);
            allPlacements.AddRange(placements2);

            foreach (var teethPlacement in allPlacements)
            {
                Canvas.SetLeft(teethPlacement.ListBoxItem, teethPlacement.Position.X);
                Canvas.SetTop(teethPlacement.ListBoxItem, teethPlacement.Position.Y);
            }

            DrawBridges(allPlacements);
        }

        private void DrawBridges(List<TeethPlacement> placements)
        {
            _bridges.Clear();
            Bridge newBridge = null;
            for (int i = 0; i < placements.Count; i++)
            {
                var currentPlacement = placements[i];
                if (currentPlacement.Teeth.HasBridge)
                {
                    if (newBridge == null)
                        newBridge = new Bridge();
                    newBridge.TeethPlacements.Add(currentPlacement);

                    if (!_bridges.Contains(newBridge))
                        _bridges.Add(newBridge);
                }
                else
                {
                    newBridge = null;
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
                TeethPlacements = new List<TeethPlacement>();
            }

            public List<TeethPlacement> TeethPlacements { get; }

            public Geometry Draw()
            {
                var geometryGroup = new GeometryGroup();

                for (int i = 0; i < TeethPlacements.Count - 1; i++)
                {
                    var teethPlacement = TeethPlacements[i];
                    var nextTeethPlacemet = TeethPlacements[i + 1];
                    if (IsStopNumber(teethPlacement) && IsStopNumber(nextTeethPlacemet))
                        continue;

                    geometryGroup.Children.Add(new LineGeometry(teethPlacement.CorrectedPosition, nextTeethPlacemet.CorrectedPosition));
                }

                return geometryGroup;
            }

            private bool IsStopNumber(TeethPlacement teethPlacement)
            {
                return StopNumbers.Contains(teethPlacement.Teeth.Number);
            }
        }

        private class TeethPlacement
        {
            public Point Position { get; set; }

            public ToothViewModel Teeth { get; set; }

            public ListBoxItem ListBoxItem { get; set; }

            public Point CorrectedPosition => new Point(
                Position.X + (ListBoxItem.ActualWidth / 2 + 1),
                Position.Y + (ListBoxItem.ActualHeight / 2 + 1));

            public override string ToString()
            {
                return Teeth.ToString() + " " + Position;
            }
        }

        private void _mouthListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_mouthListBox.SelectedItems == null || _mouthListBox.SelectedItems.Count == 0)
            {
                _toothCard.SelectedTeeth = null;
                return;
            }

            _toothCard.SelectedTeeth = _mouthListBox.SelectedItems.Cast<ToothViewModel>().ToArray();
        }
    }
}
