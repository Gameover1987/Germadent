using System.Windows.Controls;
using System.Windows.Input;
using Germadent.Rma.App.ViewModels.Pricing;

namespace Germadent.Rma.App.Views.Pricing
{
    /// <summary>
    /// Interaction logic for PriceListEditorControl.xaml
    /// </summary>
    public partial class PriceListEditorControl : UserControl
    {
        public PriceListEditorControl()
        {
            InitializeComponent();
        }

        private void PriceGroupMouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                var editor = (IPriceListEditorViewModel) DataContext;
                editor.EditPriceGroupCommand.TryExecute();
            }
        }

        private void PricePositionMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var editor = (IPriceListEditorViewModel)DataContext;
            editor.EditPricePositionCommand.TryExecute();
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listBox = (ListBox) sender;
            if (listBox == null)
                return;

            listBox.ScrollIntoView(listBox.SelectedItem);
        }
    }
}
