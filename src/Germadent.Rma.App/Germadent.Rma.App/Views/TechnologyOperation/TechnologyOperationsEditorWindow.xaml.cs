using System.Windows;
using System.Windows.Input;
using Germadent.Rma.App.ViewModels.TechnologyOperation;

namespace Germadent.Rma.App.Views.TechnologyOperation
{
    /// <summary>
    /// Interaction logic for TechnologyOperationsEditorWindow.xaml
    /// </summary>
    public partial class TechnologyOperationsEditorWindow : Window
    {
        public TechnologyOperationsEditorWindow()
        {
            InitializeComponent();

            DataContextChanged += OnDataContextChanged;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var editor = (ITechnologyOperationsEditorViewModel) DataContext;
            editor.Initialize();
        }

        private void OnTechOperationMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var editor = (ITechnologyOperationsEditorViewModel)DataContext;
            editor.EditTechnologyOperationCommand.TryExecute();
        }
    }
}
