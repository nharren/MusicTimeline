using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace NathanHarrenstein.MusicTimeline.Controls
{
    public class BulletStyleDropdownButton : Control
    {
        private List<BulletStyle> defaultBulletStyles = new List<BulletStyle>
        {
            new BulletStyle(TextMarkerStyle.Decimal, "Arabic Numeral", '1', new FontFamily("Segoe UI")),
            new BulletStyle(TextMarkerStyle.Disc, "Circle (Filled)", '\x9f', new FontFamily("Wingdings")),
            new BulletStyle(TextMarkerStyle.Circle, "Circle (Hollow)", '\xa1', new FontFamily("Wingdings")),
            new BulletStyle(TextMarkerStyle.LowerLatin, "Letter (Lowercase)", 'a', new FontFamily("Segoe UI")),
            new BulletStyle(TextMarkerStyle.UpperLatin, "Letter (Uppercase)", 'A', new FontFamily("Segoe UI")),
            new BulletStyle(TextMarkerStyle.LowerRoman, "Roman Numeral (Lowercase)", 'i', new FontFamily("Segoe UI")),
            new BulletStyle(TextMarkerStyle.UpperRoman, "Roman Numeral (Uppercase)", 'I', new FontFamily("Segoe UI")),
            new BulletStyle(TextMarkerStyle.Box, "Square (Filled)", '\xa7', new FontFamily("Wingdings")),
            new BulletStyle(TextMarkerStyle.Square, "Square (Hollow)", '\x71', new FontFamily("Wingdings"))
        };

        private bool isTemplateApplied;
        private ListBox listBox;
        private Popup popup;
        private TextMarkerStyle selectedTextMarkerStyle;
        private ToggleButton toggleDropdown;
        private ToggleButton toggleOnOff;

        static BulletStyleDropdownButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BulletStyleDropdownButton), new FrameworkPropertyMetadata(typeof(BulletStyleDropdownButton)));
        }

        public BulletStyleDropdownButton()
        {
        }

        public event EventHandler SelectionChanged;

        public TextMarkerStyle DefaultTextMarkerStyle { get; set; } = TextMarkerStyle.Disc;

        public TextMarkerStyle SelectedTextMarkerStyle
        {
            get
            {
                return selectedTextMarkerStyle;
            }
            set
            {
                selectedTextMarkerStyle = value;

                if (listBox != null)
                {
                    if (value == TextMarkerStyle.None)
                    {
                        toggleOnOff.Checked -= toggleOnOff_Checked;
                        toggleOnOff.IsChecked = false;
                        toggleOnOff.Checked += toggleOnOff_Checked;

                        listBox.SelectionChanged -= listBox_SelectionChanged;
                        listBox.SelectedItem = null;
                        listBox.SelectionChanged += listBox_SelectionChanged;
                    }
                    else
                    {
                        toggleOnOff.Checked -= toggleOnOff_Checked;
                        toggleOnOff.IsChecked = true;
                        toggleOnOff.Checked += toggleOnOff_Checked;

                        listBox.SelectionChanged -= listBox_SelectionChanged;
                        listBox.SelectedItem = defaultBulletStyles.FirstOrDefault(b => b.TextMarkerStyle == value);
                        listBox.SelectionChanged += listBox_SelectionChanged;
                    }

                    OnSelectionChanged(null);
                }
            }
        }

        public override void OnApplyTemplate()
        {
            if (isTemplateApplied)
            {
                return;
            }

            listBox = (ListBox)Template.FindName("listBox", this);
            listBox.ItemsSource = defaultBulletStyles;
            listBox.SelectionChanged += listBox_SelectionChanged;

            popup = (Popup)Template.FindName("popup", this);

            toggleDropdown = (ToggleButton)Template.FindName("toggleDropdown", this);
            toggleDropdown.LostFocus += toggleDropdown_LostFocus;

            toggleOnOff = (ToggleButton)Template.FindName("toggleOnOff", this);
            toggleOnOff.Checked += toggleOnOff_Checked;
            toggleOnOff.Unchecked += toggleOnOff_Unchecked;

            // "Application.Current.MainWindow" is null when called by the designer.
            if (Application.Current.MainWindow != null)
            {
                Application.Current.MainWindow.LocationChanged += (o, e) => RepositionPopup();
                Application.Current.MainWindow.SizeChanged += (o, e) => RepositionPopup();
            }

            isTemplateApplied = true;
        }

        private void toggleDropdown_LostFocus(object sender, RoutedEventArgs e)
        {
            toggleDropdown.IsChecked = false;
        }

        protected virtual void OnSelectionChanged(EventArgs e)
        {
            SelectionChanged?.Invoke(this, e);
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listBox.SelectedItem == null)
            {
                return;
            }

            popup.IsOpen = false;

            UpdateSelectionFromListBox();
        }

        private void RepositionPopup()
        {
            if (popup.IsOpen)
            {
                var repositionMethodInfo = typeof(Popup).GetMethod("Reposition", BindingFlags.NonPublic | BindingFlags.Instance);
                repositionMethodInfo.Invoke(popup, null);
            }
        }

        private void toggleOnOff_Checked(object sender, RoutedEventArgs e)
        {
            SelectedTextMarkerStyle = DefaultTextMarkerStyle;
        }

        private void toggleOnOff_Unchecked(object sender, RoutedEventArgs e)
        {
            selectedTextMarkerStyle = TextMarkerStyle.None;
            OnSelectionChanged(null);

            listBox.SelectedItem = null;
        }

        private void UpdateSelectionFromListBox()
        {
            toggleOnOff.Checked -= toggleOnOff_Checked;
            toggleOnOff.IsChecked = true;
            toggleOnOff.Checked += toggleOnOff_Checked;

            var selectedBulletStyle = (BulletStyle)listBox.SelectedItem;
            selectedTextMarkerStyle = selectedBulletStyle.TextMarkerStyle;

            OnSelectionChanged(null);
        }
    }
}