using System;
using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace NathanHarrenstein.MusicTimeline.Controls
{
    public class AutoCompleteBox : Control
    {
        public static readonly DependencyProperty SuggestionsProperty = DependencyProperty.Register("Suggestions", typeof(IEnumerable), typeof(AutoCompleteBox), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsArrange));
        public static readonly DependencyProperty SuggestionTemplateProperty = DependencyProperty.Register("SuggestionTemplate", typeof(DataTemplate), typeof(AutoCompleteBox));
        public static readonly RoutedEvent TextChangedEvent = TextBoxBase.TextChangedEvent.AddOwner(typeof(AutoCompleteBox));
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(AutoCompleteBox));
        private Func<IEnumerable, IEnumerable> _filter;
        private ListBox _listBox;
        private Popup _popup;
        private Func<object, string> _stringSelector;
        private TextBox _textBox;

        static AutoCompleteBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AutoCompleteBox), new FrameworkPropertyMetadata(typeof(AutoCompleteBox)));
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

        public override void OnApplyTemplate()
        {
            _textBox = (TextBox)Template.FindName("PART_TextBox", this);
            _textBox.PreviewKeyDown += TextBox_KeyDown;
            _textBox.LostFocus += TextBox_LostFocus;
            _textBox.TextChanged += TextBox_TextChanged;

            _popup = (Popup)Template.FindName("PART_Popup", this);

            _listBox = (ListBox)Template.FindName("PART_ListBox", this);
            _listBox.PreviewKeyDown += ListBox_PreviewKeyDown;
            _listBox.LostFocus += ListBox_LostFocus;

            base.OnApplyTemplate();
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

        private void ListBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (FocusManager.GetFocusedElement(FocusManager.GetFocusScope(this)) == _textBox)
            {
                e.Handled = true;
            }
        }

        private void ListBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up && _listBox.SelectedIndex == 0)
            {
                _listBox.SelectedIndex = -1;

                Keyboard.Focus(_textBox);
            }

            if (e.Key == Key.Enter)
            {
                _textBox.Text = StringSelector(_listBox.SelectedItem);

                _listBox.SelectedIndex = -1;
                _popup.IsOpen = false;

                Keyboard.Focus(_textBox);

                _textBox.CaretIndex = _textBox.Text.Length;
            }

            if (e.Key == Key.Down && _listBox.SelectedIndex == _listBox.Items.Count - 1)
            {
                e.Handled = true;
            }            
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down)
            {
                _listBox.Items.Refresh();         // Necessary for correct behavior when switching focus from TextBox to ListBox using the down arrow key.

                Keyboard.Focus(_listBox);
            }
            else if (e.Key == Key.Enter)
            {
                _textBox.Text = StringSelector(_listBox.SelectedItem);

                Text = _textBox.Text;

                _listBox.SelectedIndex = -1;
                _popup.IsOpen = false;
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (FocusManager.GetFocusedElement(FocusManager.GetFocusScope(this)) == _listBox)
            {
                e.Handled = true;
            }
            else
            {
                _popup.IsOpen = false;
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateSuggestions();
        }

        private void UpdateSuggestions()
        {
            if (Suggestions == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(_textBox.Text))
            {
                _listBox.SelectedIndex = -1;
                _popup.IsOpen = false;

                return;
            }

            var query = Filter == null ? DefaultFilter(Suggestions) : Filter(Suggestions);

            _listBox.ItemsSource = query;

            if (query == null || query.Cast<object>().Count() == 0)
            {
                _listBox.SelectedIndex = -1;
                _popup.IsOpen = false;

                return;
            }

            _popup.IsOpen = true;
        }
    }
}