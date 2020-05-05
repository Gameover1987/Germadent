using System;
using System.Collections;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace Germadent.UI.Controls
{
    /// <summary>
    /// Interaction logic for AutoCompleteTextBox.xaml
    /// </summary>
    public partial class AutoCompleteTextBox
    {
        public static readonly DependencyProperty DelayProperty = DependencyProperty.Register("Delay", typeof(int), typeof(AutoCompleteTextBox), new FrameworkPropertyMetadata(200));
        public static readonly DependencyProperty DisplayMemberProperty = DependencyProperty.Register("DisplayMember", typeof(string), typeof(AutoCompleteTextBox), new FrameworkPropertyMetadata(string.Empty));
        public static readonly DependencyProperty IsDropDownOpenProperty = DependencyProperty.Register("IsDropDownOpen", typeof(bool), typeof(AutoCompleteTextBox), new FrameworkPropertyMetadata(false));
        public static readonly DependencyProperty IsLoadingProperty = DependencyProperty.Register("IsLoading", typeof(bool), typeof(AutoCompleteTextBox), new FrameworkPropertyMetadata(false));
        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(AutoCompleteTextBox), new FrameworkPropertyMetadata(false));
        public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(AutoCompleteTextBox), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty ItemTemplateSelectorProperty = DependencyProperty.Register("ItemTemplateSelector", typeof(DataTemplateSelector), typeof(AutoCompleteTextBox));
        public static readonly DependencyProperty ProviderProperty = DependencyProperty.Register("Provider", typeof(ISuggestionProvider), typeof(AutoCompleteTextBox), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(object), typeof(AutoCompleteTextBox), new FrameworkPropertyMetadata(null, OnSelectedItemChanged));
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(AutoCompleteTextBox), new FrameworkPropertyMetadata(string.Empty) { BindsTwoWayByDefault = true });

        private BindingEvaluator _bindingEvaluator;
        private DispatcherTimer _fetchTimer;
        private string _filter;
        private bool _isUpdatingText;
        private bool _isKeyboardInput;
        private SelectionAdapter _selectionAdapter;
        private bool _selectionCancelled;
        private SuggestionsAdapter _suggestionsAdapter;

        private DateTime _lastKeyDownOccured;

        public AutoCompleteTextBox()
        {
            InitializeComponent();

            GotFocus += OnGotFocus;
        }

        private void OnGotFocus(object sender, RoutedEventArgs e)
        {
            _editor.Focus();
        }

        public int Delay
        {
            get { return (int)GetValue(DelayProperty); }
            set { SetValue(DelayProperty, value); }
        }

        public string DisplayMember
        {
            get { return (string)GetValue(DisplayMemberProperty); }
            set { SetValue(DisplayMemberProperty, value); }
        }

        public bool IsDropDownOpen
        {
            get { return (bool)GetValue(IsDropDownOpenProperty); }
            set { SetValue(IsDropDownOpenProperty, value); }
        }

        public bool IsLoading
        {
            get { return (bool)GetValue(IsLoadingProperty); }
            set { SetValue(IsLoadingProperty, value); }
        }

        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }

            set { SetValue(ItemTemplateProperty, value); }
        }

        public DataTemplateSelector ItemTemplateSelector
        {
            get { return ((DataTemplateSelector)(GetValue(ItemTemplateSelectorProperty))); }
            set { SetValue(ItemTemplateSelectorProperty, value); }
        }

        public ISuggestionProvider Provider
        {
            get { return (ISuggestionProvider)GetValue(ProviderProperty); }

            set { SetValue(ProviderProperty, value); }
        }

        public object SelectedItem
        {
            get { return GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static void OnSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AutoCompleteTextBox act;
            act = d as AutoCompleteTextBox;
            if (act == null)
                return;
            if (!(act._editor != null & !act._isUpdatingText))
                return;

            act._isUpdatingText = true;
            act._editor.Text = act._bindingEvaluator.Evaluate(e.NewValue);
            act._isUpdatingText = false;
        }

        private bool IsKeyboardInput()
        {
            var dif = DateTime.Now - _lastKeyDownOccured;
            return dif.TotalMilliseconds < 100;
        }

        private void ScrollToSelectedItem()
        {
            if (_suggestionsListBox != null && _suggestionsListBox.SelectedItem != null)
                _suggestionsListBox.ScrollIntoView(_suggestionsListBox.SelectedItem);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _bindingEvaluator = new BindingEvaluator(new Binding(DisplayMember));

            _editor.TextChanged += OnEditorTextChanged;
            _editor.PreviewKeyDown += OnEditorKeyDown;
            _editor.LostFocus += OnEditorLostFocus;

            if (SelectedItem != null)
            {
                _editor.Text = _bindingEvaluator.Evaluate(SelectedItem);
            }

            _popup.StaysOpen = false;
            _popup.Opened += OnPopupOpened;
            _popup.Closed += OnPopupClosed;

            _selectionAdapter = new SelectionAdapter(_suggestionsListBox);
            _selectionAdapter.Commit += OnSelectionAdapterCommit;
            _selectionAdapter.Cancel += OnSelectionAdapterCancel;
            _selectionAdapter.SelectionChanged += OnSelectionAdapterSelectionChanged;
        }

        private string GetDisplayText(object dataItem)
        {
            if (_bindingEvaluator == null)
            {
                _bindingEvaluator = new BindingEvaluator(new Binding(DisplayMember));
            }
            if (dataItem == null)
            {
                return string.Empty;
            }
            if (string.IsNullOrEmpty(DisplayMember))
            {
                return dataItem.ToString();
            }
            return _bindingEvaluator.Evaluate(dataItem);
        }

        private readonly Key[] _StopKeys = new Key[]
        {
            Key.Left, Key.Right, Key.Up, Key.Down
        };

        private void OnEditorKeyDown(object sender, KeyEventArgs e)
        {
            _lastKeyDownOccured = DateTime.Now;

            if (_selectionAdapter == null)
                return;

            if (IsDropDownOpen)
            {
                _selectionAdapter.HandleKeyDown(e);
            }
            else
            {
                IsDropDownOpen = (e.Key == Key.Down || e.Key == Key.Up) && _suggestionsListBox.HasItems;
            }
        }

        private void OnEditorLostFocus(object sender, RoutedEventArgs e)
        {
            if (!IsKeyboardFocusWithin)
            {
                IsDropDownOpen = false;
            }
        }

        private void OnEditorTextChanged(object sender, TextChangedEventArgs e)
        {
            if (_isUpdatingText)
                return;

            _isKeyboardInput = IsKeyboardInput();
            if (!_isKeyboardInput)
                return;

            if (_fetchTimer == null)
            {
                _fetchTimer = new DispatcherTimer();
                _fetchTimer.Interval = TimeSpan.FromMilliseconds(Delay);
                _fetchTimer.Tick += OnFetchTimerTick;
            }
            _fetchTimer.IsEnabled = false;
            _fetchTimer.Stop();
            SetSelectedItem(null);
            if (_editor.Text.Length > 0)
            {
                IsLoading = true;
                _suggestionsListBox.ItemsSource = null;
                _fetchTimer.IsEnabled = true;
                _fetchTimer.Start();
            }
            else
            {
                IsDropDownOpen = false;
            }
        }

        private void OnFetchTimerTick(object sender, EventArgs e)
        {
            _fetchTimer.IsEnabled = false;
            _fetchTimer.Stop();
            if (Provider != null && _suggestionsListBox != null)
            {
                _filter = _editor.Text;
                if (_suggestionsAdapter == null)
                {
                    _suggestionsAdapter = new SuggestionsAdapter(this);
                }
                _suggestionsAdapter.GetSuggestions(_filter);
            }
        }

        private void OnPopupClosed(object sender, EventArgs e)
        {
            if (_selectionCancelled)
                return;

            OnSelectionAdapterCommit();
        }

        private void OnPopupOpened(object sender, EventArgs e)
        {
            _selectionCancelled = false;
            _suggestionsListBox.SelectedItem = SelectedItem;
        }

        private void OnSelectionAdapterCancel()
        {
            _isUpdatingText = true;
            _editor.Text = _filter;
            _editor.SelectionStart = _editor.Text.Length;
            _editor.SelectionLength = 0;
            _isUpdatingText = false;
            IsDropDownOpen = false;
            _selectionCancelled = true;
        }

        private void OnSelectionAdapterCommit()
        {
            if (_suggestionsListBox.SelectedItem == null)
                return;

            SelectedItem = _suggestionsListBox.SelectedItem;
            _isUpdatingText = true;
            _editor.Text = GetDisplayText(_suggestionsListBox.SelectedItem);
            _filter = _editor.Text;
            SetSelectedItem(_suggestionsListBox.SelectedItem);
            _isUpdatingText = false;
            IsDropDownOpen = false;
        }

        private void OnSelectionAdapterSelectionChanged()
        {
            _isUpdatingText = true;
            if (_suggestionsListBox.SelectedItem == null)
            {
                _editor.Text = _filter;
            }
            else
            {
                _editor.Text = GetDisplayText(_suggestionsListBox.SelectedItem);
            }
            _editor.SelectionStart = _editor.Text.Length;
            _editor.SelectionLength = 0;
            ScrollToSelectedItem();
            _isUpdatingText = false;
        }

        private void SetSelectedItem(object item)
        {
            _isUpdatingText = true;
            SelectedItem = item;
            _isUpdatingText = false;
        }

        private class SuggestionsAdapter
        {

            private AutoCompleteTextBox _actb;

            private string _filter;

            public SuggestionsAdapter(AutoCompleteTextBox actb)
            {
                _actb = actb;
            }

            public void GetSuggestions(string searchText)
            {
                _filter = searchText;
                _actb.IsLoading = true;

                var parameterizedThreadStart = new ParameterizedThreadStart(GetSuggestionsAsync);
                var thread = new Thread(parameterizedThreadStart);
                var parameters = new object[] {
                    searchText,
                    _actb.Provider
                };
                thread.Start(parameters);
            }

            private void DisplaySuggestions(IEnumerable suggestions, string filter)
            {
                if (_filter != filter)
                {
                    return;
                }

                _actb.IsLoading = false;
                _actb._suggestionsListBox.ItemsSource = suggestions;

                if (_actb._isKeyboardInput)
                    _actb.IsDropDownOpen = _actb._suggestionsListBox.HasItems;
            }

            private void GetSuggestionsAsync(object param)
            {
                var args = param as object[];
                var searchText = Convert.ToString(args[0]);
                var provider = args[1] as ISuggestionProvider;
                var list = provider.GetSuggestions(searchText);
                var parameters = new object[] {
                    list,
                    searchText
                };
                _actb.Dispatcher.BeginInvoke(new Action<IEnumerable, string>(DisplaySuggestions), DispatcherPriority.Background, parameters);
            }
        }
    }
}
