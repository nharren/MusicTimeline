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
        private ListBox _suggestionListBox;
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
            _suggestionListBox.Measure(new Size(constraint.Height, _textBox.DesiredSize.Width));

            return base.MeasureOverride(constraint);
        }

        protected override void OnInitialized(EventArgs e)
        {
            _textBox.BorderThickness = BorderThickness;
            _textBox.BorderBrush = BorderBrush;
            _textBox.FontFamily = FontFamily;
            _textBox.FontSize = FontSize;

            _suggestionListBox.BorderThickness = new Thickness();
            _suggestionListBox.FontFamily = FontFamily;
            _suggestionListBox.FontSize = FontSize;

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
            _suggestionListBox = new ListBox();
            _suggestionListBox.SelectionMode = SelectionMode.Single;
            _suggestionListBox.Visibility = Visibility.Collapsed;
            _suggestionListBox.IsHitTestVisible = false;
            _suggestionListBox.PreviewKeyDown += listBox_PreviewKeyDown;
            _suggestionListBox.FocusVisualStyle = null;
            _suggestionListBox.LostFocus += (o, e) =>
            {
                if (FocusManager.GetFocusedElement(FocusManager.GetFocusScope(this)) == _textBox)
                {
                    e.Handled = true;
                }
            };

            var style = new Style(typeof(ListBoxItem));
            style.Setters.Add(new Setter(FocusVisualStyleProperty, null));
            _suggestionListBox.ItemContainerStyle = style;
        }

        private void InitializeSuggestionPopup()
        {
            _suggestionPopup = new Popup();
            _suggestionPopup.AllowsTransparency = true;
            _suggestionPopup.Child = _suggestionListBox;
            _suggestionPopup.IsOpen = true;
            _suggestionPopup.PlacementTarget = _textBox;
            _suggestionPopup.Placement = PlacementMode.Bottom;
        }

        private void InitializeTextBox()
        {
            _textBox = new TextBox();
            _textBox.PreviewKeyDown += textBox_KeyDown;
            _textBox.TextChanged += _textBox_TextChanged;
            _textBox.LostFocus += (o, e) =>
            {
                if (FocusManager.GetFocusedElement(FocusManager.GetFocusScope(this)) == _suggestionListBox)
                {
                    e.Handled = true;
                }
            };

            AddVisualChild(_textBox);
        }

        private void listBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up && _suggestionListBox.SelectedIndex == 0)
            {
                _suggestionListBox.SelectedIndex = -1;

                Keyboard.Focus(_textBox);
            }

            if (e.Key == Key.Enter)
            {
                _textBox.Text = StringSelector(_suggestionListBox.SelectedItem);

                _suggestionListBox.SelectedIndex = -1;
                _suggestionListBox.Visibility = Visibility.Collapsed;

                Keyboard.Focus(_textBox);

                _textBox.CaretIndex = _textBox.Text.Length;
            }
        }

        private void SetBindings()
        {
            _suggestionListBox.SetBinding(ItemsControl.ItemTemplateProperty, BindingUtility.Create(this, "SuggestionTemplate"));
            var binding = BindingUtility.Create(this, "Text");
            binding.Mode = BindingMode.TwoWay;
            binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            _textBox.SetBinding(TextBox.TextProperty, binding);
            _suggestionListBox.SetBinding(WidthProperty, BindingUtility.Create(_textBox, "ActualWidth"));

            var backgroundBinding = BindingUtility.Create(this, "Background");
            _suggestionListBox.SetBinding(BackgroundProperty, backgroundBinding);
            _textBox.SetBinding(BackgroundProperty, backgroundBinding);

            var foregroundBinding = BindingUtility.Create(this, "Foreground");
            _suggestionListBox.SetBinding(ForegroundProperty, foregroundBinding);
            _textBox.SetBinding(ForegroundProperty, foregroundBinding);
            _textBox.SetBinding(TextBoxBase.CaretBrushProperty, foregroundBinding);
        }

        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down)
            {
                _suggestionListBox.Items.Refresh();         // Necessary for correct behavior when switching focus from TextBox to ListBox using the down arrow key.

                Keyboard.Focus(_suggestionListBox);
            }
            else if (e.Key == Key.Enter)
            {
                _textBox.Text = StringSelector(_suggestionListBox.SelectedItem);

                Text = _textBox.Text;

                _suggestionListBox.SelectedIndex = -1;
                _suggestionListBox.Visibility = Visibility.Collapsed;
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
                _suggestionListBox.SelectedIndex = -1;
                _suggestionListBox.Visibility = Visibility.Collapsed;

                return;
            }

            var query = Filter == null ? DefaultFilter(Suggestions) : Filter(Suggestions);

            _suggestionListBox.ItemsSource = query;

            if (query == null || query.Cast<object>().Count() == 0)
            {
                _suggestionListBox.SelectedIndex = -1;
                _suggestionListBox.Visibility = Visibility.Collapsed;

                return;
            }

            _suggestionListBox.Visibility = Visibility.Visible;
        }
    }
}