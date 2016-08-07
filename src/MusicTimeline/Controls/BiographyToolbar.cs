using NathanHarrenstein.MusicTimeline.Utilities;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;

namespace NathanHarrenstein.MusicTimeline.Controls
{
    public class BiographyToolbar : SaveToolbar
    {
        public static readonly DependencyProperty RichTextBoxProperty = DependencyProperty.Register("RichTextBox", typeof(RichTextBox), typeof(BiographyToolbar), new PropertyMetadata(null, RichTextBoxChanged));

        private ToggleButton boldToggleButton;
        private ComboBox fontSizeComboBox;
        private ToggleButton italicToggleButton;
        private string[] systemFontSizes = { "8", "9", "10", "11", "12", "14", "16", "18", "20", "22", "24", "26", "28", "36", "48", "72" };
        private ToggleButton underlineToggleButton;
        private bool isTemplateApplied;
        private BulletStyleDropdownButton bulletStyleDropdownButton;

        static BiographyToolbar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BiographyToolbar), new FrameworkPropertyMetadata(typeof(BiographyToolbar)));
        }

        public RichTextBox RichTextBox
        {
            get
            {
                return (RichTextBox)GetValue(RichTextBoxProperty);
            }

            set
            {
                SetValue(RichTextBoxProperty, value);
            }
        }

        public override void OnApplyTemplate()
        {
            if (isTemplateApplied)
            {
                return;
            }

            fontSizeComboBox = (ComboBox)Template.FindName("PART_FontSizeComboBox", this);
            boldToggleButton = (ToggleButton)Template.FindName("PART_BoldToggleButton", this);
            italicToggleButton = (ToggleButton)Template.FindName("PART_ItalicToggleButton", this);
            underlineToggleButton = (ToggleButton)Template.FindName("PART_UnderlineToggleButton", this);
            bulletStyleDropdownButton = (BulletStyleDropdownButton)Template.FindName("PART_BulletStyleDropdownButton", this);

            fontSizeComboBox.ItemsSource = systemFontSizes.ToList();

            fontSizeComboBox.LostFocus += FontSizeComboBox_LostFocus;
            fontSizeComboBox.PreviewKeyDown += FontSizeComboBox_PreviewKeyDown;
            fontSizeComboBox.SelectionChanged += FontSizeComboBox_SelectionChanged;

            bulletStyleDropdownButton.SelectionChanged += bulletStyleDropdownButton_SelectionChanged;

            isTemplateApplied = true;

            base.OnApplyTemplate();
        }

        private void bulletStyleDropdownButton_SelectionChanged(object sender, EventArgs e)
        {
            if (!isTemplateApplied)
            {
                return;
            }

            var textMarkerStyle = GetTextMarkerStyle();

            if (textMarkerStyle == bulletStyleDropdownButton.SelectedTextMarkerStyle)
            {
                return;
            }

            if (textMarkerStyle == TextMarkerStyle.None)
            {
                EditingCommands.ToggleBullets.Execute(null, RichTextBox);
            }

            SetTextMarkerStyle(bulletStyleDropdownButton.SelectedTextMarkerStyle);
        }

        private static void RichTextBoxChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var biographyToolbar = (BiographyToolbar)d;

            biographyToolbar.DetachRichTextBox((RichTextBox)e.OldValue);

            if (biographyToolbar.RichTextBox != null)
            {
                biographyToolbar.AttachRichTextBox();
            }
        }

        private void AttachRichTextBox()
        {
            RichTextBox.PreviewMouseUp += RichTextBox_PreviewMouseUp;
        }

        private void DetachRichTextBox(RichTextBox richTextBox)
        {
            if (richTextBox == null)
            {
                return;
            }

            richTextBox.PreviewMouseUp -= RichTextBox_PreviewMouseUp;
        }

        private void FontSizeComboBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!isTemplateApplied)
            {
                return;
            }

            var oldFontSizeObject = RichTextBox.Selection.GetPropertyValue(TextElement.FontSizeProperty);

            double oldFontSize;
            double newFontSize;

            if (double.TryParse(fontSizeComboBox.Text, out newFontSize))
            {
                newFontSize *= (96.0 / 72.0);

                if (oldFontSizeObject != DependencyProperty.UnsetValue)
                {
                    oldFontSize = (double)oldFontSizeObject;

                    if (oldFontSize == newFontSize)
                    {
                        return;
                    }
                }

                RichTextBox.Selection.ApplyPropertyValue(TextElement.FontSizeProperty, newFontSize);
                RichTextBox.UpdateLayout();
            }
        }

        private void FontSizeComboBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (!isTemplateApplied || e.Key != Key.Enter)
            {
                return;
            }

            var oldFontSizeObject = RichTextBox.Selection.GetPropertyValue(TextElement.FontSizeProperty);

            double oldFontSize;
            double newFontSize;

            if (double.TryParse(fontSizeComboBox.Text, out newFontSize))
            {
                newFontSize *= (96.0 / 72.0);

                if (oldFontSizeObject != DependencyProperty.UnsetValue)
                {
                    oldFontSize = (double)oldFontSizeObject;

                    if (oldFontSize == newFontSize)
                    {
                        return;
                    }
                }

                RichTextBox.Selection.ApplyPropertyValue(TextElement.FontSizeProperty, newFontSize);
                RichTextBox.UpdateLayout();
            }
        }

        private void FontSizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!isTemplateApplied)
            {
                return;
            }

            var oldFontSizeObject = RichTextBox.Selection.GetPropertyValue(TextElement.FontSizeProperty);

            double oldFontSize;
            double newFontSize;

            if (double.TryParse((string)fontSizeComboBox.SelectedItem, out newFontSize))
            {
                newFontSize *= (96.0 / 72.0);

                if (oldFontSizeObject != DependencyProperty.UnsetValue)
                {
                    oldFontSize = (double)oldFontSizeObject;

                    if (oldFontSize == newFontSize)
                    {
                        return;
                    }
                }

                RichTextBox.Selection.ApplyPropertyValue(TextElement.FontSizeProperty, newFontSize);
                RichTextBox.UpdateLayout();
            }
        }

        private void RichTextBox_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!isTemplateApplied)
            {
                return;
            }

            var fontSizeObject = RichTextBox.Selection.GetPropertyValue(TextElement.FontSizeProperty);

            if (fontSizeObject == DependencyProperty.UnsetValue || fontSizeObject == null)
            {
                fontSizeComboBox.Text = string.Empty;
            }
            else
            {
                var fontSize = (double)fontSizeObject;
                var fontSizeInPts = fontSize * (72.0 / 96.0);
                var roundedFontSize = Math.Round(fontSizeInPts, 1);
                fontSizeComboBox.Text = roundedFontSize.ToString();
            }

            var boldObject = RichTextBox.Selection.GetPropertyValue(TextElement.FontWeightProperty);

            if (boldObject == DependencyProperty.UnsetValue || (FontWeight)boldObject != FontWeights.Bold)
            {
                boldToggleButton.IsChecked = false;
            }
            else
            {
                boldToggleButton.IsChecked = true;
            }

            var italicObject = RichTextBox.Selection.GetPropertyValue(TextElement.FontStyleProperty);

            if (italicObject == DependencyProperty.UnsetValue || (FontStyle)italicObject != FontStyles.Italic)
            {
                italicToggleButton.IsChecked = false;
            }
            else
            {
                italicToggleButton.IsChecked = true;
            }

            bulletStyleDropdownButton.SelectedTextMarkerStyle = GetTextMarkerStyle();

            CommandManager.InvalidateRequerySuggested();
        }

        private TextMarkerStyle GetTextMarkerStyle()
        {
            List list = LogicalTreeUtility.FindLogicalParent<List>(RichTextBox.Selection.Start.Parent);

            if (list != null)
            {
                return list.MarkerStyle;
            }

            return TextMarkerStyle.None;
        }

        private void SetTextMarkerStyle(TextMarkerStyle textMarkerStyle)
        {
            List list = LogicalTreeUtility.FindLogicalParent<List>(RichTextBox.Selection.Start.Parent);

            if (list != null)
            {
                list.MarkerStyle = textMarkerStyle;
            }
        }
    }
}