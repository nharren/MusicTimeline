using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace NathanHarrenstein.Controls
{
    public class AutoCompleteBox : Control, INotifyPropertyChanged
    {
        private ListBox listBox;
        private IEnumerable<string> suggestions;
        private string text;
        private TextBox textBox;
        private Popup popup;

        public AutoCompleteBox()
        {
            FocusVisualStyle = null;

            textBox = new TextBox();
            textBox.PreviewKeyDown += textBox_KeyDown;
            textBox.TextChanged += textBox_TextChanged;

            listBox = new ListBox();
            listBox.SelectionMode = SelectionMode.Single;
            listBox.Visibility = Visibility.Collapsed;
            listBox.IsHitTestVisible = false;
            listBox.PreviewKeyDown += listBox_PreviewKeyDown;
            listBox.FocusVisualStyle = null;

            var style = new Style(typeof(ListBoxItem));
            var setter = new Setter(ListBoxItem.FocusVisualStyleProperty, null);
            style.Setters.Add(setter);
            listBox.ItemContainerStyle = style;

            AddVisualChild(textBox);

            popup = new Popup();
            popup.AllowsTransparency = true;
            popup.Child = listBox;
            popup.IsOpen = true;
            popup.PlacementTarget = textBox;
            popup.Placement = PlacementMode.Bottom;

            var popupWidthBinding = new Binding("ActualWidth");
            popupWidthBinding.Source = textBox;
            listBox.SetBinding(ListBox.WidthProperty, popupWidthBinding);

            var backgroundBinding = new Binding("Background");
            backgroundBinding.Source = this;
            listBox.SetBinding(ListBox.BackgroundProperty, backgroundBinding);
            textBox.SetBinding(TextBox.BackgroundProperty, backgroundBinding);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected override void OnInitialized(EventArgs e)
        {
            textBox.BorderThickness = BorderThickness;
            textBox.BorderBrush = BorderBrush;
            //textBox.Background = Background;
            textBox.Foreground = Foreground;
            textBox.CaretBrush = Foreground;
            textBox.FontFamily = FontFamily;
            textBox.FontSize = FontSize;

            //listBox.Background = Background;
            listBox.Foreground = Foreground;
            listBox.FontFamily = FontFamily;
            listBox.FontSize = FontSize;

            base.OnInitialized(e);
        }

        public IEnumerable<string> Suggestions
        {
            get { return suggestions; }
            set
            {
                suggestions = value;
                OnPropertyChanged("Suggestions");
                InvalidateArrange();
            }
        }

        public string Text
        {
            get { return text; }
            set
            {
                text = value;
                OnPropertyChanged("Text");
                textBox.Text = Text;
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
            textBox.Arrange(new Rect(arrangeBounds));

            return base.ArrangeOverride(arrangeBounds);
        }

        protected override Geometry GetLayoutClip(Size layoutSlotSize)
        {
            return null;
        }

        protected override Visual GetVisualChild(int index)
        {
            return textBox;
        }

        protected override Size MeasureOverride(Size constraint)
        {
            textBox.Measure(constraint);
            listBox.Measure(new Size(constraint.Height, textBox.DesiredSize.Width));

            return base.MeasureOverride(constraint);
        }

        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        private void listBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up && listBox.SelectedIndex == 0)
            {
                listBox.SelectedIndex = -1;

                Keyboard.Focus(textBox);
            }

            if (e.Key == Key.Enter)
            {
                textBox.Text = (string)listBox.SelectedItem;

                listBox.SelectedIndex = -1;
                listBox.Visibility = Visibility.Collapsed;

                Keyboard.Focus(textBox);
                textBox.CaretIndex = textBox.Text.Length;
            }
        }

        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down)
            {
                listBox.Items.Refresh(); // Necessary for correct behavior when switching focus from TextBox to ListBox using the down arrow key.

                Keyboard.Focus(listBox);
            }
            else if (e.Key == Key.Enter)
            {
                textBox.Text = (string)listBox.SelectedItem;

                Text = textBox.Text;

                listBox.SelectedIndex = -1;
                listBox.Visibility = Visibility.Collapsed;
            }
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.Equals(textBox.Text, text, StringComparison.CurrentCultureIgnoreCase))
            {
                Text = textBox.Text;

                UpdateSuggestions();
            }
        }

        private void UpdateSuggestions()
        {
            if (suggestions == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(textBox.Text))
            {
                listBox.SelectedIndex = -1;
                listBox.Visibility = Visibility.Collapsed;

                return;
            }

            var query = suggestions.Where(s => s.StartsWith(textBox.Text, StringComparison.CurrentCultureIgnoreCase));

            listBox.ItemsSource = query;

            if (query == null || query.Count() == 0)
            {
                listBox.SelectedIndex = -1;
                listBox.Visibility = Visibility.Collapsed;

                return;
            }

            listBox.Visibility = Visibility.Visible;
        }
    }
}