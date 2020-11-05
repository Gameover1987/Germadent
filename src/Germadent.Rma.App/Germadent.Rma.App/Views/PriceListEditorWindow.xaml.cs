using System.Windows;
using Germadent.Rma.App.ViewModels.Pricing;

namespace Germadent.Rma.App.Views
{
    /// <summary>
    /// Interaction logic for PriceListEditorWindow.xaml
    /// </summary>
    public partial class PriceListEditorWindow : Window
    {
        public PriceListEditorWindow()
        {
            InitializeComponent();

            DataContextChanged += OnDataContextChanged;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var editorContainer = (IPriceListEditorContainerViewModel) DataContext;
            editorContainer?.Initialize();
        }
    }
}
