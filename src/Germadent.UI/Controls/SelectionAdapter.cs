using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Germadent.UI.Helpers;

namespace Germadent.UI.Controls
{
    public class SelectionAdapter
    {

        private Selector _selectorControl;
       

        public SelectionAdapter(Selector selector)
        {
            SelectorControl = selector;
            SelectorControl.PreviewMouseLeftButtonDown += SelectorControlOnPreviewMouseLeftButtonDown;
        }

        public delegate void CancelEventHandler();

        public delegate void CommitEventHandler();

        public delegate void SelectionChangedEventHandler();

        public event CancelEventHandler Cancel;
        public event CommitEventHandler Commit;
        public event SelectionChangedEventHandler SelectionChanged;

        public Selector SelectorControl
        {
            get { return _selectorControl; }
            set { _selectorControl = value; }
        }
       

        public void HandleKeyDown(KeyEventArgs key)
        {
            Debug.WriteLine(key.Key);
            switch (key.Key)
            {
                case Key.Down:
                    IncrementSelection();
                    break;
                case Key.Up:
                    DecrementSelection();
                    break;
                case Key.Enter:
                    Commit?.Invoke();

                    break;
                case Key.Escape:
                    Cancel?.Invoke();

                    break;
                case Key.Tab:
                    Commit?.Invoke();
                    break;
            }
        }

        private void DecrementSelection()
        {
            if (SelectorControl.SelectedIndex == -1)
            {
                SelectorControl.SelectedIndex = SelectorControl.Items.Count - 1;
            }
            else
            {
                SelectorControl.SelectedIndex -= 1;
            }
            if (SelectionChanged != null)
            {
                SelectionChanged();
            }
        }

        private void IncrementSelection()
        {
            if (SelectorControl.SelectedIndex == SelectorControl.Items.Count - 1)
            {
                SelectorControl.SelectedIndex = -1;
            }
            else
            {
                SelectorControl.SelectedIndex += 1;
            }
            if (SelectionChanged != null)
            {
                SelectionChanged();
            }
        }

        private void SelectorControlOnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var listBoxItem = UiSearchHelper.FindAncestor<ListBoxItem>(e.OriginalSource as FrameworkElement);
            if (listBoxItem == null)
                return;

            SelectorControl.SelectedItem = listBoxItem.DataContext;
            Commit?.Invoke();
        }
    }
}