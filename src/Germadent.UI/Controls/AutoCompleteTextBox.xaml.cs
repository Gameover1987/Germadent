using System;
using System.Collections;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using Germadent.Common.Extensions;

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
        public static readonly DependencyProperty ProviderProperty = DependencyProperty.Register("Provider", typeof(ISuggestionProvider), typeof(AutoCompleteTextBox), new FrameworkPropertyMetadata(null, PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(object), typeof(AutoCompleteTextBox), new FrameworkPropertyMetadata(null, OnSelectedItemChanged));
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(AutoCompleteTextBox), new FrameworkPropertyMetadata(string.Empty));

        public static readonly DependencyProperty WatermarkProperty = DependencyProperty.Register("Watermark", typeof(string), typeof(AutoCompleteTextBox), new FrameworkPropertyMetadata(string.Empty));
        private BindingEvaluator _bindingEvaluator;

        private DispatcherTimer _fetchTimer;

        private string _filter;

        private bool _isUpdatingText;

        private SelectionAdapter _selectionAdapter;

        private bool _selectionCancelled;

        private SuggestionsAdapter _suggestionsAdapter;

        public AutoCompleteTextBox()
        {
            InitializeComponent();
        }

        public BindingEvaluator BindingEvaluator
        {
            get { return _bindingEvaluator; }
            set { _bindingEvaluator = value; }
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

        public string Filter
        {
            get { return _filter; }
            set { _filter = value; }
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
            get { return ((DataTemplateSelector)(GetValue(AutoCompleteTextBox.ItemTemplateSelectorProperty))); }
            set { SetValue(AutoCompleteTextBox.ItemTemplateSelectorProperty, value); }
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

        public SelectionAdapter SelectionAdapter
        {
            get { return _selectionAdapter; }
            set { _selectionAdapter = value; }
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }

            set { SetValue(TextProperty, value); }
        }

        public string Watermark
        {
            get { return (string)GetValue(WatermarkProperty); }

            set { SetValue(WatermarkProperty, value); }
        }

        public static void OnSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AutoCompleteTextBox act = null;
            act = d as AutoCompleteTextBox;
            if (act != null)
            {
                if (act._editor != null & !act._isUpdatingText)
                {
                    act._isUpdatingText = true;
                    act._editor.Text = act.BindingEvaluator.Evaluate(e.NewValue);
                    act._isUpdatingText = false;
                }
            }
        }

        private void ScrollToSelectedItem()
        {
            if (_selector != null && _selector.SelectedItem != null)
                _selector.ScrollIntoView(_selector.SelectedItem);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            BindingEvaluator = new BindingEvaluator(new Binding(DisplayMember));

            if (_editor != null)
            {
                _editor.TextChanged += OnEditorTextChanged;
                _editor.PreviewKeyDown += OnEditorKeyDown;
                _editor.LostFocus += OnEditorLostFocus;

                if (SelectedItem != null)
                {
                    _editor.Text = BindingEvaluator.Evaluate(SelectedItem);
                }

            }

            if (_popup != null)
            {
                _popup.StaysOpen = false;
                _popup.Opened += OnPopupOpened;
                _popup.Closed += OnPopupClosed;
            }
            if (_selector != null)
            {
                SelectionAdapter = new SelectionAdapter(_selector);
                SelectionAdapter.Commit += OnSelectionAdapterCommit;
                SelectionAdapter.Cancel += OnSelectionAdapterCancel;
                SelectionAdapter.SelectionChanged += OnSelectionAdapterSelectionChanged;
            }
        }
        private string GetDisplayText(object dataItem)
        {
            if (BindingEvaluator == null)
            {
                BindingEvaluator = new BindingEvaluator(new Binding(DisplayMember));
            }
            if (dataItem == null)
            {
                return string.Empty;
            }
            if (string.IsNullOrEmpty(DisplayMember))
            {
                return dataItem.ToString();
            }
            return BindingEvaluator.Evaluate(dataItem);
        }

        private void OnEditorKeyDown(object sender, KeyEventArgs e)
        {
            if (SelectionAdapter != null)
            {
                if (IsDropDownOpen)
                    SelectionAdapter.HandleKeyDown(e);
                else
                    IsDropDownOpen = e.Key == Key.Down || e.Key == Key.Up;
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
                //IsDropDownOpen = true;
                _selector.ItemsSource = null;
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
            if (Provider != null && _selector != null)
            {
                Filter = _editor.Text;
                if (_suggestionsAdapter == null)
                {
                    _suggestionsAdapter = new SuggestionsAdapter(this);
                }
                _suggestionsAdapter.GetSuggestions(Filter);
            }
        }

        private void OnPopupClosed(object sender, EventArgs e)
        {
            if (!_selectionCancelled)
            {
                OnSelectionAdapterCommit();
            }
        }

        private void OnPopupOpened(object sender, EventArgs e)
        {
            _selectionCancelled = false;
            _selector.SelectedItem = SelectedItem;
        }

        private void OnSelectionAdapterCancel()
        {
            _isUpdatingText = true;
            _editor.Text = SelectedItem == null ? Filter : GetDisplayText(SelectedItem);
            _editor.SelectionStart = _editor.Text.Length;
            _editor.SelectionLength = 0;
            _isUpdatingText = false;
            IsDropDownOpen = false;
            _selectionCancelled = true;
        }

        private void OnSelectionAdapterCommit()
        {
            if (_selector.SelectedItem != null)
            {
                SelectedItem = _selector.SelectedItem;
                _isUpdatingText = true;
                _editor.Text = GetDisplayText(_selector.SelectedItem);
                SetSelectedItem(_selector.SelectedItem);
                _isUpdatingText = false;
                IsDropDownOpen = false;
            }
        }

        private void OnSelectionAdapterSelectionChanged()
        {
            _isUpdatingText = true;
            if (_selector.SelectedItem == null)
            {
                _editor.Text = Filter;
            }
            else
            {
                _editor.Text = GetDisplayText(_selector.SelectedItem);
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

            #region "Fields"

            private AutoCompleteTextBox _actb;

            private string _filter;
            #endregion

            #region "Constructors"

            public SuggestionsAdapter(AutoCompleteTextBox actb)
            {
                _actb = actb;
            }

            #endregion

            #region "Methods"

            public async void GetSuggestions(string searchText)
            {
                _filter = searchText;
                _actb.IsLoading = true;

                //IEnumerable suggestions = null;
                //await ThreadTaskExtensions.Run(() =>
                //{
                //    suggestions = _actb.Provider.GetSuggestions(searchText);
                //});

                //DisplaySuggestions(suggestions);

                ParameterizedThreadStart thInfo = new ParameterizedThreadStart(GetSuggestionsAsync);
                Thread th = new Thread(thInfo);
                th.Start(new object[] {
                    searchText,
                    _actb.Provider
                });
            }

            private void DisplaySuggestions(IEnumerable suggestions, string filter)
            {
                if (_filter != filter)
                {
                    return;
                }

                _actb.IsLoading = false;
                _actb._selector.ItemsSource = suggestions;
                _actb.IsDropDownOpen = _actb._selector.HasItems;
            }

            private void GetSuggestionsAsync(object param)
            {
                object[] args = param as object[];
                string searchText = Convert.ToString(args[0]);
                ISuggestionProvider provider = args[1] as ISuggestionProvider;
                IEnumerable list = provider.GetSuggestions(searchText);
                _actb.Dispatcher.BeginInvoke(new Action<IEnumerable, string>(DisplaySuggestions), DispatcherPriority.Background, new object[] {
                    list,
                    searchText
                });
            }

            #endregion

        }
    }
}
