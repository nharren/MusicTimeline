using NathanHarrenstein.MusicTimeline.Utilities;
using System;
using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace NathanHarrenstein.MusicTimeline.Controls
{
    public class AutoCompleteBox : Control
    {
        public static readonly DependencyProperty SuggestionsProperty = DependencyProperty.Register("Suggestions", typeof(IEnumerable), typeof(AutoCompleteBox), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsArrange));
        public static readonly DependencyProperty SuggestionTemplateProperty = DependencyProperty.Register("SuggestionTemplate", typeof(DataTemplate), typeof(AutoCompleteBox));
        public static readonly RoutedEvent TextChangedEvent = TextBoxBase.TextChangedEvent.AddOwner(typeof(AutoCompleteBox));
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(AutoCompleteBox));
        private Func<IEnumerable, IEnumerable> _filter;
        private Func<object, string> _stringSelector;
        private ListBox _suggestionlistBox;
        private Popup _suggestionPopup;
        private TextBox _textBox;

        public AutoCompleteBox()
        {
            FocusVisualStyle = null;

            InitializeTextBox();
            InitializeSuggestionListBox();
            InitializeSuggestionPopup();

            SetBindings();
        }

        public event TextChangedEventHandler TextChanged
        {
            add
            {
                AddHandler(TextChangedEvent, value);
            }

            remove
            {
                RemoveHandler(TextChangedEvent, value);
            }
        }

        public Func<IEnumerable, IEnumerable> Filter
        {
            get
            {
                return _filter;
            }
            set
            {
                _filter = value;
            }
        }

        public Func<object, string> StringSelector
        {
            get
            {
                return _stringSelector;
            }

            set
            {
                _stringSelector = value;
            }
        }

        public IEnumerable Suggestions
        {
            get
            {
                return (IEnumerable)GetValue(SuggestionsProperty);
            }
            set
            {
                SetValue(SuggestionsProperty, value);
            }
        }

        public DataTemplate SuggestionTemplate
        {
            get
            {
                return (DataTemplate)GetValue(SuggestionTemplateProperty);
            }
            set
            {
                SetValue(SuggestionTemplateProperty, value);
            }
        }

        public string Text
        {
            get
            {
                return (string)GetValue(TextProperty);
            }
            set
            {
                SetValue(TextProperty, value);
            }
        }

        protected override int VisualChildrenCount
        {
            get
            {
                return 1;
            }
        }

        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            _textBox.Arrange(new Rect(arrangeBounds));

            return base.ArrangeOverride(arrangeBounds);
        }

        protected override Geometry GetLayoutClip(Size layoutSlotSize)
        {
            return null;
        }

        protected override Visual GetVisualChild(int index)
        {
            return _textBox;
        }

        protected override Size MeasureOverride(Size constraint)
        {
            _textBox.Measure(constraint);
            _suggestionlistBox.Measure(new Size(constraint.Height, _textBox.DesiredSize.Width));

            return base.MeasureOverride(constraint);
        }

        protected override void OnInitialized(EventArgs e)
        {
            _textBox.BorderThickness = BorderThickness;
            _textBox.BorderBrush = BorderBrush;
            _textBox.FontFamily = FontFamily;
            _textBox.FontSize = FontSize;

            _suggestionlistBox.BorderThickness = new Thickness();
            _suggestionlistBox.FontFamily = FontFamily;
            _suggestionlistBox.FontSize = FontSize;

            base.OnInitialized(e);
        }

        private void _textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateSuggestions();
        }

        private IEnumerable DefaultFilter(IEnumerable suggestions)
        {
            foreach (var suggestion in suggestions)
            {
                var suggestionString = StringSelector(suggestion);

                if (suggestionString.StartsWith(_textBox.Text) && suggestionString != _textBox.Text)
                {
                    yield return suggestion;
                }
            }
        }

        private void InitializeSuggestionListBox()
        {
            _suggestionlistBox = new ListBox();
            _suggestionlistBox.SelectionMode = SelectionMode.Single;
            _suggestionlistBox.Visibility = Visibility.Collapsed;
            _suggestionlistBox.IsHitTestVisible = false;
            _suggestionlistBox.PreviewKeyDown += listBox_PreviewKeyDown;
            _suggestionlistBox.FocusVisualStyle = null;

            var style = new Style(typeof(ListBoxItem));
            style.Setters.Add(new Setter(FocusVisualStyleProperty, null));
            _suggestionlistBox.ItemContainerStyle = style;
        }

        private void InitializeSuggestionPopup()
        {
            _suggestionPopup = new Popup();
            _suggestionPopup.AllowsTransparency = true;
            _suggestionPopup.Child = _suggestionlistBox;
            _suggestionPopup.IsOpen = true;
            _suggestionPopup.PlacementTarget = _textBox;
            _suggestionPopup.Placement = PlacementMode.Bottom;
        }

        private void InitializeTextBox()
        {
            _textBox = new TextBox();
            _textBox.PreviewKeyDown += textBox_KeyDown;
            _textBox.TextChanged += _textBox_TextChanged;

            AddVisualChild(_textBox);
        }

        private void listBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up && _suggestionlistBox.SelectedIndex == 0)
            {
                _suggestionlistBox.SelectedIndex = -1;

                Keyboard.Focus(_textBox);
            }

            if (e.Key == Key.Enter)
            {
                _textBox.Text = StringSelector(_suggestionlistBox.SelectedItem);

                _suggestionlistBox.SelectedIndex = -1;
                _suggestionlistBox.Visibility = Visibility.Collapsed;

                Keyboard.Focus(_textBox);

                _textBox.CaretIndex = _textBox.Text.Length;
            }
        }

        private void SetBindings()
        {
            _suggestionlistBox.SetBinding(ItemsControl.ItemTemplateProperty, BindingUtility.Create(this, "SuggestionTemplate"));
            var binding = BindingUtility.Create(this, "Text");
            binding.Mode = BindingMode.TwoWay;
            binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            _textBox.SetBinding(TextBox.TextProperty, binding);
            _suggestionlistBox.SetBinding(WidthProperty, BindingUtility.Create(_textBox, "ActualWidth"));

            var backgroundBinding = BindingUtility.Create(this, "Background");
            _suggestionlistBox.SetBinding(BackgroundProperty, backgroundBinding);
            _textBox.SetBinding(BackgroundProperty, backgroundBinding);

            var foregroundBinding = BindingUtility.Create(this, "Foreground");
            _suggestionlistBox.SetBinding(ForegroundProperty, foregroundBinding);
            _textBox.SetBinding(ForegroundProperty, foregroundBinding);
            _textBox.SetBinding(TextBoxBase.CaretBrushProperty, foregroundBinding);
        }

        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down)
            {
                _suggestionlistBox.Items.Refresh();         // Necessary for correct behavior when switching focus from TextBox to ListBox using the down arrow key.

                Keyboard.Focus(_suggestionlistBox);
            }
            else if (e.Key == Key.Enter)
            {
                _textBox.Text = StringSelector(_suggestionlistBox.SelectedItem);

                Text = _textBox.Text;

                _suggestionlistBox.SelectedIndex = -1;
                _suggestionlistBox.Visibility = Visibility.Collapsed;
            }
        }

        private void UpdateSuggestions()
        {
            if (Suggestions == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(_textBox.Text))
            {
                _suggestionlistBox.SelectedIndex = -1;
                _suggestionlistBox.Visibility = Visibility.Collapsed;

                return;
            }

            var query = Filter == null ? DefaultFilter(Suggestions) : Filter(Suggestions);

            _suggestionlistBox.ItemsSource = query;

            if (query == null || query.Cast<object>().Count() == 0)
            {
                _suggestionlistBox.SelectedIndex = -1;
                _suggestionlistBox.Visibility = Visibility.Collapsed;

                return;
            }

            _suggestionlistBox.Visibility = Visibility.Visible;
        }
    }
}